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
    [RoutePrefix("api/mainservicegroup")]
    [Authorize]
    public class MainServiceGroupController : ApiControllerBase
    {
        private IMainServiceGroupService _mainServiceGroupService;
        private IServiceService _serviceService;
        private IServiceGroupService _serviceGroupService;

        public MainServiceGroupController(IErrorService errorService, 
                IServiceService serviceService, 
                IMainServiceGroupService mainServiceGroupService,
                IServiceGroupService serviceGroupService)
           : base(errorService)
        {
            this._mainServiceGroupService = mainServiceGroupService;
            this._serviceService = serviceService;
            _serviceGroupService = serviceGroupService;
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _mainServiceGroupService.GetById(id);

                var responseData = Mapper.Map<MainServiceGroup, MainServiceGroupViewModel>(model);

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

                var model = _mainServiceGroupService.GetAll(keyword);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<MainServiceGroup>, IEnumerable<MainServiceGroupViewModel>>(query);
                var paginationSet = new PaginationSet<MainServiceGroupViewModel>
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                foreach (var item in responseData)
                {
                    item.NoGroupService = _serviceGroupService.GetAllByParentId(item.Id).Count();
                }
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _mainServiceGroupService.GetAll();

                var responseData = Mapper.Map<IEnumerable<MainServiceGroup>, IEnumerable<MainServiceGroupViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, MainServiceGroupViewModel mainServiceGroupVM)
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
                    var dbMainServiceGroup = _mainServiceGroupService.GetById(mainServiceGroupVM.Id);
                    dbMainServiceGroup.UpdateMainServiceGroup(mainServiceGroupVM);
                    _mainServiceGroupService.update(dbMainServiceGroup);
                    _mainServiceGroupService.Save();
                    var responseData = Mapper.Map<MainServiceGroup, MainServiceGroupViewModel>(dbMainServiceGroup);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, MainServiceGroupViewModel mainServiceGroupVM)
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
                    var newMainServiceGroup = new MainServiceGroup();
                    newMainServiceGroup.UpdateMainServiceGroup(mainServiceGroupVM);
                    newMainServiceGroup.CreatedBy = User.Identity.Name;
                    _mainServiceGroupService.Add(newMainServiceGroup);
                    _mainServiceGroupService.Save();
                    var responseData = Mapper.Map<MainServiceGroup, MainServiceGroupViewModel>(newMainServiceGroup);
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
                    var oldMainServiceGroup = _mainServiceGroupService.Delete(id);
                    _mainServiceGroupService.Save();
                    var responseData = Mapper.Map<MainServiceGroup, MainServiceGroupViewModel>(oldMainServiceGroup);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }
                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedMainServiceGroups)
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
                    var listMainServiceGroup = new JavaScriptSerializer().Deserialize<List<int>>(checkedMainServiceGroups);
                    foreach (var item in listMainServiceGroup)
                    {
                        _mainServiceGroupService.Delete(item);
                    }

                    _mainServiceGroupService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listMainServiceGroup.Count);
                }

                return response;
            });
        }
    }
}