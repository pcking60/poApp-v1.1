using AutoMapper;
using PostOffice.Model.Models;
using PostOffice.Service;
using PostOffice.Web.Infrastructure.Core;
using PostOffice.Web.Infrastructure.Extensions;
using PostOffice.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace PostOffice.Web.Api
{
    [RoutePrefix("api/servicegroup")]
    [Authorize]
    public class ServiceGroupController : ApiControllerBase
    {
        private IServiceGroupService _serviceGroupService;
        private IServiceService _serviceService;

        public ServiceGroupController(IErrorService errorService, IServiceService serviceService, IServiceGroupService serviceGroupService)
           : base(errorService)
        {
            this._serviceGroupService = serviceGroupService;
            this._serviceService = serviceService;
        }

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _serviceGroupService.GetAll();

                var responseData = Mapper.Map<IEnumerable<ServiceGroup>, IEnumerable<ServiceGroupViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _serviceGroupService.GetById(id);

                var responseData = Mapper.Map<ServiceGroup, ServiceGroupViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;

                var model = _serviceGroupService.GetAll(keyword);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<ServiceGroup>, IEnumerable<ServiceGroupViewModel>>(query);

                foreach (var item in responseData)
                {
                    var no = _serviceGroupService.GetAllByServiceGroupId(item.ID);
                    item.NoService = no.Count();
                }

                var paginationSet = new PaginationSet<ServiceGroupViewModel>
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

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, ServiceGroupViewModel serviceGroupVM)
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
                    var dbServiceGroup = _serviceGroupService.GetById(serviceGroupVM.ID);
                    dbServiceGroup.UpdateServiceGroup(serviceGroupVM);
                    _serviceGroupService.update(dbServiceGroup);
                    _serviceGroupService.Save();
                    var responseData = Mapper.Map<ServiceGroup, ServiceGroupViewModel>(dbServiceGroup);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, ServiceGroupViewModel serviceGroupVM)
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
                    var newServiceGroup = new ServiceGroup();
                    newServiceGroup.UpdateServiceGroup(serviceGroupVM);
                    _serviceGroupService.Add(newServiceGroup);
                    _serviceGroupService.Save();
                    var responseData = Mapper.Map<ServiceGroup, ServiceGroupViewModel>(newServiceGroup);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
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
                    var oldServiceGroup = _serviceGroupService.Delete(id);
                    _serviceGroupService.Save();
                    var responseData = Mapper.Map<ServiceGroup, ServiceGroupViewModel>(oldServiceGroup);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }
                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedServiceGroups)
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
                    var listServiceGroup = new JavaScriptSerializer().Deserialize<List<int>>(checkedServiceGroups);
                    foreach (var item in listServiceGroup)
                    {
                        _serviceGroupService.Delete(item);
                    }

                    _serviceGroupService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listServiceGroup.Count);
                }

                return response;
            });
        }
    }
}