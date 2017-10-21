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

namespace PostOffice.Web.Api
{
    [RoutePrefix("api/po")]
    [Authorize]
    public class POController : ApiControllerBase
    {
        private IPOService _poService;
        private IApplicationUserService _userService;
        private IDistrictService _districtService;

        public POController(IErrorService errorService, IPOService poService, IDistrictService districtService, IApplicationUserService userService) : base(errorService)
        {
            this._poService = poService;
            this._districtService = districtService;
            this._userService = userService;
        }

        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _poService.Getall();
                totalRow = model.Count();

                var query = model.OrderBy(x => x.ID).Skip(page * pageSize).Take(pageSize);
                var responseData = Mapper.Map<IEnumerable<PO>, IEnumerable<POViewModel>>(query);
                //
                foreach (var item in responseData)
                {
                    var district = _districtService.GetById(item.DistrictID);
                    int No = _userService.getNoUserByPoID(item.ID);
                    item.NoUser = No;
                    item.DistrictName = district.Name;
                }

                var paginationSet = new PaginationSet<POViewModel>
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
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _poService.Getall();

                var responseData = Mapper.Map<IEnumerable<PO>, IEnumerable<POViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, POViewModel poVM)
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
                    var dbPO = _poService.GetByID(poVM.ID);
                    dbPO.UpdatePO(poVM);
                    _poService.Update(dbPO);
                    _poService.Save();
                    var responseData = Mapper.Map<PO, POViewModel>(dbPO);
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
                var model = _poService.GetByID(id);

                var responseData = Mapper.Map<PO, POViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getbycurrentuser")]
        [HttpGet]
        public HttpResponseMessage GetByCurrentUser(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _poService.GetPOByCurrentUser(User.Identity.Name);

                var responseData = Mapper.Map<PO, POViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getbydistrictid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetByDistrictId(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _poService.GetAllPOByDistrictId(id);

                var responseData = Mapper.Map<IEnumerable<PO>,IEnumerable<POViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, POViewModel poVM)
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
                    var newPO = new PO();
                    newPO.UpdatePO(poVM);
                    _poService.Add(newPO);
                    _poService.Save();
                    var responseData = Mapper.Map<PO, POViewModel>(newPO);
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
                    var oldPO = _poService.Delete(id);
                    _poService.Save();
                    var responseData = Mapper.Map<PO, POViewModel>(oldPO);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("getallnopaging")]
        public HttpResponseMessage GetAllNoPaging(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var responseData = Mapper.Map<IEnumerable<PO>, IEnumerable<POViewModel>>(_poService.Getall());
                return request.CreateResponse(HttpStatusCode.OK, responseData);
            });
        }
    }
}