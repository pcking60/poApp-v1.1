using AutoMapper;
using PostOffice.Common;
using PostOffice.Common.ViewModels;
using PostOffice.Service;
using PostOffice.Web.Infrastructure.Core;
using PostOffice.Web.Infrastructure.Extensions;
using PostOffice.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PostOffice.Web.Api
{
    [RoutePrefix("api/service")]
    [Authorize]
    public class ServiceController : ApiControllerBase
    {
        #region
        private IServiceService _serviceService;
        private IServiceGroupService _serviceGroupService;

        public ServiceController(IErrorService errorService, IServiceService service, IServiceGroupService serviceGroupService) : base(errorService)
        {
            this._serviceService = service;
            this._serviceGroupService = serviceGroupService;
        }
        #endregion
        [Route("edit")]
        [HttpPut]
        [Authorize(Roles = "UpdateService")]
        public HttpResponseMessage Update(HttpRequestMessage request, ServiceViewModel serviceVm)
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
                    var dbService = _serviceService.GetById(serviceVm.ID);
                    serviceVm.UpdatedBy = User.Identity.Name;                    
                    dbService.UpdateService(serviceVm);
                    _serviceService.Update(dbService);
                    _serviceService.Save();
                    var responseData = Mapper.Map<PostOffice.Model.Models.Service, ServiceViewModel>(dbService);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [Authorize(Roles = "DeleteService")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
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
                    var oldServie = _serviceService.Delete(id);
                    _serviceService.Save();
                    var responseData = Mapper.Map<PostOffice.Model.Models.Service, ServiceViewModel>(oldServie);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _serviceService.GetById(id);

                var responseData = Mapper.Map<PostOffice.Model.Models.Service, ServiceViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }
        
        [Route("getallbyuserid/{id}")]
        [HttpGet]
        public HttpResponseMessage GetAllByUserId(HttpRequestMessage request, string id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _serviceService.GetAllByUserId(id);

                var responseData = Mapper.Map<IEnumerable<PostOffice.Model.Models.Service>,IEnumerable<ServiceViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("add")]
        [HttpPost]
        [Authorize(Roles = "AddService")]
        public HttpResponseMessage Add(HttpRequestMessage request, ServiceViewModel serviceVM)
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
                    var newService = new PostOffice.Model.Models.Service();
                    serviceVM.CreatedBy = User.Identity.Name;
                    newService.UpdateService(serviceVM);
                    _serviceService.Add(newService);
                    _serviceService.Save();
                    var responseData = Mapper.Map<PostOffice.Model.Models.Service, ServiceViewModel>(newService);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 100)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _serviceService.Getall(keyword);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);
                var responseData = Mapper.Map<IEnumerable<PostOffice.Model.Models.Service>, IEnumerable<ServiceViewModel>>(query);

                foreach (var item in responseData)
                {
                    var sv = _serviceGroupService.GetById(item.GroupID);
                    item.GroupName = sv.Name;
                }

                var paginationSet = new PaginationSet<ServiceViewModel>
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

        [Route("getallparents")]        
        [HttpGet]
        public HttpResponseMessage GetAllParentID(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {//ham nay la sao ban
                var model = _serviceService.Getall();
                var query = model.OrderBy(x => x.Name);
                var responseData = Mapper.Map<IEnumerable<Model.Models.Service>, IEnumerable<ServiceViewModel>>(query);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [HttpGet]
        [Route("ExportXls")]
        public async Task<HttpResponseMessage> ExportXls(HttpRequestMessage request, string filter = null)
        {
            string fileName = string.Concat("Product_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            statisticReportViewModel vm = new statisticReportViewModel();
            try
            {
                #region customFill Test
                vm.fromDate = new DateTime(2017, 5, 1);
                vm.toDate = new DateTime(2017, 5, 31);
                vm.ServiceName = "Tất cả";
                vm.UserName = User.Identity.Name;
                vm.PoName = "Mỹ Xuyên";
                #endregion

                var data = _serviceService.Getall(filter);
                var responseData = Mapper.Map<IEnumerable<Model.Models.Service>, IEnumerable<ServiceViewModel>>(data);
                foreach (var item in responseData)
                {
                    var sv = _serviceGroupService.GetById(item.GroupID);
                    item.GroupName = sv.Name;
                }
                var result = Mapper.Map<IEnumerable<ServiceViewModel>, IEnumerable<ReportServiceViewModel>>(responseData);

                //test medthod customFill
                await ReportHelper.StatisticXls(result.ToList(), fullPath, vm);

                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        
    }
}