using AutoMapper;
using PostOffice.Model.Models;
using PostOffice.Service;
using PostOffice.Web.Infrastructure.Core;
using PostOffice.Web.Infrastructure.Extensions;
using PostOffice.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace PostOffice.Web.Api
{
    [RoutePrefix("api/property_services")]
    [Authorize]
    public class PropertyServiceController : ApiControllerBase
    {
        private IPropertyServiceService _propertyServiceSerivce;
        private IServiceService _serviceService;
        private IErrorService _errorService;

        public PropertyServiceController(IErrorService errorService, IPropertyServiceService propertyServiceSerivce, IServiceService serviceService) : base(errorService)
        {
            this._propertyServiceSerivce = propertyServiceSerivce;
            this._errorService = errorService;
            this._serviceService = serviceService;          
        }

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAllParentID(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _propertyServiceSerivce.GetAll();
                var responseData = Mapper.Map<IEnumerable<PropertyService>, IEnumerable<PropertyServiceViewModel>>(model);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize = 100)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _propertyServiceSerivce.GetAll();
                totalRow = model.Count();
                var query = model.OrderBy(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<PropertyService>, IEnumerable<PropertyServiceViewModel>>(query);

                foreach(var item in responseData)
                {
                    item.ServiceName = _serviceService.GetById(item.ServiceID).Name;
                }
               
                var paginationSet = new PaginationSet<PropertyServiceViewModel>//sai ne ban.
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
        public HttpResponseMessage Update(HttpRequestMessage request, PropertyServiceViewModel propVM)
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
                    var dbPropertyService = _propertyServiceSerivce.GetById(propVM.ID);
                    dbPropertyService.UpdatePropertyService(propVM);
                    _propertyServiceSerivce.Update(dbPropertyService);
                    _propertyServiceSerivce.Save();
                    var responseData = Mapper.Map<PropertyService, PropertyServiceViewModel>(dbPropertyService);
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
                var model = _propertyServiceSerivce.GetById(id);

                var responseData = Mapper.Map<PropertyService, PropertyServiceViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getbyserviceid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetByServiceId(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                try
                {
                    var model = _propertyServiceSerivce.GetListPropertyByServiceId(id).OrderBy(x => x.Name);
                    var responseData = Mapper.Map<IEnumerable<PropertyService>, IEnumerable<PropertyServiceViewModel>>(model);

                    var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                    return response;
                }
                catch(Exception e)
                {
                    return request.CreateErrorResponse(HttpStatusCode.NotFound, e.InnerException.Message);
                }    
                
            });
        }

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, PropertyServiceViewModel properVM)
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
                    var newProperty = new PropertyService();
                    newProperty.UpdatePropertyService(properVM);
                    _propertyServiceSerivce.Add(newProperty);
                    _propertyServiceSerivce.Save();
                    var responseData = Mapper.Map<PropertyService, PropertyServiceViewModel>(newProperty);
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
                    var oldPropertyService = _propertyServiceSerivce.Delete(id);
                    _propertyServiceSerivce.Save();
                    var responseData = Mapper.Map<PropertyService, PropertyServiceViewModel>(oldPropertyService);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedPropertyServices)
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
                    var listPropertyServices = new JavaScriptSerializer().Deserialize<List<int>>(checkedPropertyServices);
                    foreach (var item in listPropertyServices)
                    {
                        _propertyServiceSerivce.Delete(item);
                    }

                    _propertyServiceSerivce.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listPropertyServices.Count);
                }

                return response;
            });
        }
    }
}