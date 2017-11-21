using AutoMapper;
using OfficeOpenXml;
using PostOffice.Common;
using PostOffice.Common.ViewModels.ExportModel;
using PostOffice.Common.ViewModels.RankModel;
using PostOffice.Common.ViewModels.StatisticModel;
using PostOffice.Model.Models;
using PostOffice.Service;
using PostOffice.Web.Infrastructure.Core;
using PostOffice.Web.Infrastructure.Extensions;
using PostOffice.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PostOffice.Web.Api
{
    [RoutePrefix("api/tkbd")]
    [AllowAnonymous]
    public class TKBDController : ApiControllerBase
    {
        #region constructor
        private ITKBDService _tkbdService;
        private ITKBDHistoryService _tkbdHistoryService;
        private IApplicationUserService _applicationUserService;
        private IDistrictService _districtService;
        private IPOService _poService;
        private IApplicationUserService _userService;
        private IInterestRateService _interestRateService;
        #endregion
        public TKBDController(IInterestRateService interestRateService, IApplicationUserService userService, IDistrictService districtService, IPOService poService, IErrorService errorService, ITKBDService tkbdService, ITKBDHistoryService tkbdHistoryService, IApplicationUserService applicationUserService) : base(errorService)
        {
            this._tkbdService = tkbdService;
            this._tkbdHistoryService = tkbdHistoryService;
            _applicationUserService = applicationUserService;
            _districtService = districtService;
            _poService = poService;
            _userService = userService;
            _interestRateService = interestRateService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _tkbdService.GetAll().Where(x => x.Status == true);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.Id).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<TKBDAmount>, IEnumerable<TKBDAmountViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.Name = _tkbdHistoryService.GetByAccount(item.Account).FirstOrDefault().CustomerName;
                    item.TotalMoney = _tkbdHistoryService.GetByAccount(item.Account).Where(x => x.TransactionDate.Value.Month <= item.Month).Sum(x => x.Money);
                    var s = _tkbdHistoryService.GetByAccount(item.Account).Where(x => x.TransactionDate.Value.Month == item.Month).FirstOrDefault();
                    if (s == null)
                    {
                        item.TransactionDate = _tkbdHistoryService.GetByAccount(item.Account).OrderByDescending(x => x.TransactionDate).FirstOrDefault().TransactionDate;
                    }
                    else
                    {
                        item.TransactionDate = s.TransactionDate;
                    }

                    string userId = _tkbdHistoryService.GetByAccount(item.Account).FirstOrDefault().UserId;
                    item.TransactionUser = _applicationUserService.getByUserId(userId).FullName;
                }
                //ban test lai thu
                var paginationSet = new PaginationSet<TKBDAmountViewModel>//sai ne ban.
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("export")]
        [HttpGet]
        public async Task<HttpResponseMessage> Export(HttpRequestMessage request, int month, int year, int districtId, int functionId, int poId, string userId)
        {
            #region Config Export file

            string fileName = string.Concat("TKBD_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);

            #endregion Config Export file

            Export_Info_Template vm = new Export_Info_Template();
            try
            {
                #region customFill Test

                District district = new District();
                PO po = new PO();
                ApplicationUser user = new ApplicationUser();
                vm.Month = month;
                vm.Year = year;
                vm.CreatedBy = User.Identity.Name;

                //check param đầu vào

                #region data input

                if (districtId != 0)
                {
                    district = _districtService.GetById(districtId);
                    vm.District = district.Name;
                }
                if (poId != 0)
                {
                    po = _poService.GetByID(poId);
                    vm.Unit = po.Name;
                }
                if (!string.IsNullOrEmpty(userId))
                {
                    user = _userService.getByUserId(userId);
                    vm.user = user.FullName;
                }

                vm.Service = "Tiết kiệm bưu điện";

                #endregion data input

                string currentUser = User.Identity.Name;

                switch (functionId)
                {
                    #region case 1 Thống kê tổng hợp giao dịch phát sinh

                    case 1:
                        vm.FunctionName = "Thống kê tổng hợp giao dịch phát sinh";

                        var responseTKBD = _tkbdService.Export_TKBD_By_Condition(month, year, districtId, poId, currentUser, userId);
                        var dataSource = Mapper.Map<IEnumerable<TKBD_Export_Template>, IEnumerable<TKBD_Export_Template_ViewModel>>(responseTKBD);
                        foreach (var item in dataSource)
                        {
                            var fullName = _userService.getByUserName(item.CreatedBy).FullName;
                            if (fullName != null)
                            {
                                item.FullName = fullName;
                            }
                        }
                        await ReportHelper.TKBD_Export_General(dataSource.ToList(), fullPath, vm);

                        break;

                    #endregion case 1 Thống kê tổng hợp giao dịch phát sinh

                    #region case 2 Thống kê chi tiết giao dịch phát sinh

                    case 2:
                        vm.FunctionName = "Thống kê chi tiết giao dịch phát sinh";

                        var responseTKBD_Detail = _tkbdService.Export_TKBD_Detail_By_Condition(month, year, districtId, poId, currentUser, userId);
                        //var c = responseTKBD_Detail.Count();
                        //var result = responseTKBD_Detail.ToList();
                        var dataSource_Detail = Mapper.Map<IEnumerable<TKBD_Export_Detail_Template>, IEnumerable<TKBD_Export_Detail_Template_ViewModel>>(responseTKBD_Detail);
                        foreach (var item in dataSource_Detail)
                        {
                            var fullName = _userService.getByUserName(item.CreatedBy).FullName;
                            item.FullName = fullName;
                        }
                        await ReportHelper.TKBD_Export_Detail(dataSource_Detail.ToList(), fullPath, vm);
                        break;

                    #endregion case 2 Thống kê chi tiết giao dịch phát sinh

                    default:
                        vm.FunctionName = "Chức năng khác";
                        break;
                }

                #endregion customFill Test

                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("gethistorybycondition")]
        [HttpGet]
        public HttpResponseMessage GetByCondtion(HttpRequestMessage request, string fromDate, string toDate, int districtId, int poId, string userId, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                //get current user
                string currentUser = User.Identity.Name;

                int totalRow = 0;

                // get data by condition
                var model = _tkbdHistoryService.Get_By_Condition(fromDate, toDate, districtId, poId, currentUser, userId).ToList<TKBD_History_Statistic>();

                totalRow = model.Count();

                var query = model.OrderByDescending(x => x.Id).Skip(page * pageSize).Take(pageSize);
                var result = Mapper.Map<IEnumerable<TKBD_History_Statistic>, IEnumerable<TKBD_History_Statistic_ViewModel>>(query);
                foreach (var item in result)
                {
                    item.CreatedByName = _userService.getByUserName(item.CreatedBy).FullName;
                }
                var paginationSet = new PaginationSet<TKBD_History_Statistic_ViewModel>
                {
                    Items = result,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("getallhistory")]
        [HttpGet]
        public HttpResponseMessage GetAllHistory(HttpRequestMessage request, int page, int pageSize = 40)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var userName = User.Identity.Name;
                var model = _tkbdHistoryService.GetAllByUserName(userName);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.Id).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<TKBDHistory>, IEnumerable<TKBDHistoryViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.FullName = _applicationUserService.getByUserId(item.UserId).FullName;
                }

                var paginationSet = new PaginationSet<TKBDHistoryViewModel>
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("gettkbd1day")]
        [HttpGet]
        public HttpResponseMessage GetHistory1Day(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var userName = User.Identity.Name;
                var model = _tkbdHistoryService.GetAllByUserName(userName);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.TransactionDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<TKBDHistory>, IEnumerable<TKBDHistoryViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.FullName = _applicationUserService.getByUserId(item.UserId).FullName;
                }

                var paginationSet = new PaginationSet<TKBDHistoryViewModel>
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("gettkbd30day")]
        [HttpGet]
        public HttpResponseMessage GetHistory30Day(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var userName = User.Identity.Name;
                var model = _tkbdHistoryService.GetAllByUserName30Day(userName);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.TransactionDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<TKBDHistory>, IEnumerable<TKBDHistoryViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.FullName = _applicationUserService.getByUserId(item.UserId).FullName;
                }

                var paginationSet = new PaginationSet<TKBDHistoryViewModel>
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("gettkbd7day")]
        [HttpGet]
        public HttpResponseMessage GetHistory7Day(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var userName = User.Identity.Name;
                var model = _tkbdHistoryService.GetAllByUserName7Day(userName);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.TransactionDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<TKBDHistory>, IEnumerable<TKBDHistoryViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.FullName = _applicationUserService.getByUserId(item.UserId).FullName;
                }

                var paginationSet = new PaginationSet<TKBDHistoryViewModel>
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("import")]
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
                    List<TKBDHistory> listItem = new List<TKBDHistory>();
                    listItem = this.ReadTKBDFromExcel(fullPath);
                    if (listItem.Count > 0)
                    {
                        foreach (var product in listItem)
                        {
                            var _savingTypeId = product.ServiceId.Substring(0, 2);
                            var _periodId = product.ServiceId.Substring(2, 2);
                            var _interestTypeId = product.ServiceId.Substring(4, 2);
                            if (_savingTypeId == "V0")
                            {
                                product.Rate = (decimal)0.01;
                            }
                            else
                            {
                                if (_savingTypeId == "VT")
                                {
                                    var money = product.Money;
                                    const int S1 = 100000000;
                                    const int S2 = 300000000;
                                    const int S3 = 500000000;
                                    const int S4 = 1000000000;
                                    const int S5 = 2000000000;
                                    if (S1 <= money && money < S2)
                                    {
                                        _savingTypeId = _savingTypeId + "1";
                                        product.Rate = _interestRateService.GetByCondition(_savingTypeId, _periodId, _interestTypeId).Percent;
                                    }
                                    else
                                    {
                                        if (S2 <= money && money < S3)
                                        {
                                            _savingTypeId = _savingTypeId + "2";
                                            product.Rate = _interestRateService.GetByCondition(_savingTypeId, _periodId, _interestTypeId).Percent;
                                        }
                                        else
                                        {
                                            if (S3 <= money && money < S4)
                                            {
                                                _savingTypeId = _savingTypeId + "3";
                                                product.Rate = _interestRateService.GetByCondition(_savingTypeId, _periodId, _interestTypeId).Percent;
                                            }
                                            else
                                            {
                                                if (S4 <= money && money < S5)
                                                {
                                                    _savingTypeId = _savingTypeId + "4";
                                                    product.Rate = _interestRateService.GetByCondition(_savingTypeId, _periodId, _interestTypeId).Percent;
                                                }
                                                else
                                                {
                                                    if (S5 <= money)
                                                    {
                                                        _savingTypeId = _savingTypeId + "5";
                                                        product.Rate = _interestRateService.GetByCondition(_savingTypeId, _periodId, _interestTypeId).Percent;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //InterestRate t = _interestRateService.GetByCondition(_savingTypeId, _periodId, _interestTypeId);
                                    
                                    product.Rate = _interestRateService.GetByCondition(_savingTypeId, _periodId, _interestTypeId).Percent;
                                }
                            }
                            _tkbdHistoryService.Add(product);
                            addedCount++;
                        }
                        _tkbdHistoryService.Save();
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Đã nhập thành công " + addedCount + " sản phẩm thành công.");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        private List<TKBDHistory> ReadTKBDFromExcel(string fullPath)
        {
            using (var package = new ExcelPackage(new FileInfo(fullPath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault();
                List<TKBDHistory> listTKBD = new List<TKBDHistory>();
                TKBDHistoryViewModel tkbdViewModel;
                TKBDHistory tkbdHistory;
                DateTime transactionDate;
                DateTime tranDate;
                decimal money;
                int test = workSheet.Dimension.Start.Row + 1;
                int end = workSheet.Dimension.End.Row;
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    tkbdViewModel = new TKBDHistoryViewModel();
                    tkbdHistory = new TKBDHistory();
                    //1
                    tkbdViewModel.CustomerName = workSheet.Cells[i, 2].Value.ToString();
                    //2
                    tkbdViewModel.CustomerId = workSheet.Cells[i, 3].Value.ToString();
                    //3
                    tkbdViewModel.Account = workSheet.Cells[i, 4].Value.ToString();
                    //4
                    tkbdViewModel.ServiceId = workSheet.Cells[i, 5].Value.ToString();
                    //5
                    if (DateTime.TryParseExact(workSheet.Cells[i, 6].Value.ToString(), "dd/MM/yyyy hh:mm:ss", null, DateTimeStyles.None, out transactionDate))
                    {
                        string temp = transactionDate.ToString("yyyy-MM-dd");
                        DateTime.TryParse(temp, out tranDate);
                        tkbdViewModel.TransactionDate = tranDate;
                    }
                    else
                    {
                        if (DateTime.TryParseExact(workSheet.Cells[i, 6].Value.ToString(), "MM/dd/yyyy hh:mm:ss", null, DateTimeStyles.None, out transactionDate))
                        {
                            string temp = transactionDate.ToString("yyyy-MM-dd");
                            DateTime.TryParse(temp, out tranDate);
                            tkbdViewModel.TransactionDate = tranDate;
                        }
                        else
                        {
                            transactionDate = DateTime.FromOADate(double.Parse(workSheet.Cells[i, 6].Value.ToString()));
                            if (transactionDate != null)
                            {
                                string temp = transactionDate.ToString("yyyy-MM-dd");
                                DateTime.TryParse(temp, out tranDate);
                                tkbdViewModel.TransactionDate = tranDate;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    //6
                    decimal.TryParse(workSheet.Cells[i, 8].Value.ToString().Replace(",", ""), out money);
                    tkbdViewModel.Money = money;
                    //7
                    if (_applicationUserService.getByUserName(workSheet.Cells[i, 9].Value.ToString()) != null)
                    {
                        tkbdViewModel.UserId = _applicationUserService.getByUserName(workSheet.Cells[i, 9].Value.ToString()).Id;
                    }
                    else
                    {
                        tkbdViewModel.UserId = "Người dùng không tồn tại";
                    }
                    //8 month
                    if (tkbdViewModel.TransactionDate != null)
                    {
                        tkbdViewModel.Month = tkbdViewModel.TransactionDate.Value.Month;
                        tkbdViewModel.Year = tkbdViewModel.TransactionDate.Value.Year;
                        var code = tkbdViewModel.Year.ToString() + tkbdViewModel.Month.ToString();
                        tkbdViewModel.TimeCode = int.Parse(code);
                    }
                    tkbdViewModel.CreatedBy = User.Identity.Name;
                    tkbdViewModel.CreatedDate = DateTime.Now;
                    tkbdViewModel.Status = true;
                    
                    tkbdHistory.UpdateTKBDHistory(tkbdViewModel);
                    listTKBD.Add(tkbdHistory);
                }
                return listTKBD;
            }
        }
        
        [Route("update")]
        [HttpGet]
        public HttpResponseMessage Update(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    int days = 0;
                    // previous month
                    var _now = int.Parse(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString()) -1;
                    var tkbdHistories = new List<TKBDHistory>();
                    // list account of previous month
                    tkbdHistories = _tkbdHistoryService.GetAll().Where(x=>x.Status==true && x.TimeCode==_now).ToList();
                    int c = tkbdHistories.Count();
                    foreach (var item in tkbdHistories.ToList())
                    {
                        decimal money = _tkbdHistoryService.GetByAccount(item.Account).Where(x => x.Status == true && (x.TransactionDate.Value.Month <= DateTime.Now.Month - 1 || x.TransactionDate.Value.Year <DateTime.Now.Year)).Sum(x => x.Money) ?? 0;

                        if (money <= 0)
                        {
                            var oldTransaction = _tkbdHistoryService.GetByAccount(item.Account);
                            foreach (var item1 in oldTransaction)
                            {
                                item1.Status = false;
                            }
                        }
                        else
                        {
                            TimeSpan s = new TimeSpan();
                            DateTimeOffset lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                            DateTimeOffset firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
                            s = lastDay.Subtract(item.TransactionDate ?? DateTimeOffset.UtcNow);
                            days = (int)s.TotalDays;
                            if (days > 31)
                            {
                                days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month - 1);
                            }

                            //if (firstDayOfMonth > item.TransactionDate)
                            //{
                            //    s = lastDay.Subtract(firstDayOfMonth);
                            //}
                            //else
                            //{
                            //    s = lastDay.Subtract(item.TransactionDate ?? DateTimeOffset.UtcNow);
                            //}

                            //days = (int)s.TotalDays;
                            TKBDAmountViewModel vm = new TKBDAmountViewModel();
                            vm.Status = true;
                            vm.Account = item.Account;
                            vm.CreatedBy = _applicationUserService.getByUserId(item.UserId).UserName;
                            vm.UserId = item.UserId;
                            vm.Month = DateTime.Now.Month - 1;
                            vm.Year = DateTime.Now.Year;
                            vm.TotalMoney = money;
                            vm.Amount = money * item.Rate * 20 * days / 120000 / 30 ?? 0;
                            TKBDAmount tkbd = new TKBDAmount();
                            tkbd.UpdateTKBD(vm);
                            if (_tkbdService.CheckExist(vm.Account, vm.Month, vm.Year))
                            {
                                continue;
                            }
                            _tkbdService.Add(tkbd);
                        }
                        _tkbdService.Save();
                    }

                    response = request.CreateResponse(HttpStatusCode.Created, tkbdHistories.Count());
                }
                return response;
            });
        }
        [Route("rank")]
        [HttpGet]
        public HttpResponseMessage Rank(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var m1 = DateTime.Now.Month - 1;
                    var m2 = DateTime.Now.Month - 2;
                    var query = _tkbdService.Rank(m2, m1);
                    var result = query.ToList();
                    var c = result.Count();
                    var responseData = Mapper.Map<IEnumerable<Rank>, IEnumerable<RankAfter>>(result);
                    foreach (var item in responseData)
                    {
                        item.FullName = _applicationUserService.getByUserName(item.CreatedBy).FullName;
                    }
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }
    }
}