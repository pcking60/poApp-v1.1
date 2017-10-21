

using OfficeOpenXml;
using PostOffice.Model.Models;
using PostOffice.Service;
using PostOffice.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PostOffice.Web.Api
{
    [RoutePrefix("api/import")]
    [AllowAnonymous]
    public class ImportController : ApiControllerBase
    {
        private IPeriodService _periodService;
        public ImportController(IErrorService errorService, PeriodService periodService) : base(errorService)
        {
            _periodService = periodService;
        }
        [Route("period")]
        [HttpPost]
        public async Task<HttpResponseMessage> Import()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Định dạng không được server hỗ trợ");
            }

            var root = HttpContext.Current.Server.MapPath("~/UploadedFiles/Excels");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var provider = new MultipartFormDataStreamProvider(root);
            Stream reqStream = Request.Content.ReadAsStreamAsync().Result;
            MemoryStream tempStream = new MemoryStream();
            reqStream.CopyTo(tempStream);

            tempStream.Seek(0, SeekOrigin.End);
            StreamWriter writer = new StreamWriter(tempStream);
            writer.WriteLine();
            writer.Flush();
            tempStream.Position = 0;

            StreamContent streamContent = new StreamContent(tempStream);
            foreach (var header in Request.Content.Headers)
            {
                streamContent.Headers.Add(header.Key, header.Value);
            }
            try
            {
                // Read the form data.
                streamContent.LoadIntoBufferAsync().Wait();
                //This is where it bugs out
                var result = await streamContent.ReadAsMultipartAsync(provider);
                //Upload files
                int addedCount = 0;
                foreach (MultipartFileData fileData in result.FileData)
                {
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Yêu cầu không đúng định dạng");
                    }
                    string fileName = fileData.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }

                    var fullPath = Path.Combine(root, fileName);
                    File.Copy(fileData.LocalFileName, fullPath, true);

                    //insert to DB
                    //var
                    List<Period> listPeriod = new List<Period>();
                    listPeriod = this.ReadPeriodListFromExcel(fullPath);
                    if (listPeriod.Count > 0)
                    {
                        foreach (var period in listPeriod)
                        {
                            _periodService.Add(period);
                            addedCount++;
                        }
                        _periodService.Save();
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Đã nhập thành công " + addedCount + " sản phẩm thành công.");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        private List<Period> ReadPeriodListFromExcel(string fullPath)
        {
            using (var package = new ExcelPackage(new FileInfo(fullPath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                List<Period> listPeriod = new List<Period>();
                Period period;               

                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {

                    period = new Period();
                    try
                    {
                        period.Id = workSheet.Cells[i, 1].Value.ToString();
                        period.Description = workSheet.Cells[i, 2].Value.ToString();
                    }
                    catch(Exception e)
                    {
                        return null;
                    }
                    listPeriod.Add(period);
                }
                return listPeriod;
            }
        }

    }
}