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
    [RoutePrefix("api/district")]
    [Authorize]
    public class DistrictController : ApiControllerBase
    {
        private IDistrictService _districtService;
        private IPOService _poService;

        public DistrictController(IErrorService errorService, IDistrictService districtService, IPOService poService) : base(errorService)
        {
            this._districtService = districtService;
            this._poService = poService;
        }

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAllParentID(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {//ham nay la sao ban
                var model = _districtService.GetAll();
                var responseData = Mapper.Map<IEnumerable<District>, IEnumerable<DistrictViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _districtService.GetAll();
                totalRow = model.Count();
                var query = model.OrderBy(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<District>, IEnumerable<DistrictViewModel>>(query);

                foreach (var item in responseData)
                {
                    var listPO = _poService.GetAllPOByDistrictId(item.ID);
                    item.NoPO = listPO.Count();
                }
                //ban test lai thu
                var paginationSet = new PaginationSet<DistrictViewModel>//sai ne ban.
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
        public HttpResponseMessage Update(HttpRequestMessage request, DistrictViewModel districtVM)
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
                    var dbDistrict = _districtService.GetById(districtVM.ID);
                    dbDistrict.UpdateDistrict(districtVM);
                    _districtService.Update(dbDistrict);
                    _districtService.Save();
                    var responseData = Mapper.Map<District, DistrictViewModel>(dbDistrict);
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
                var model = _districtService.GetById(id);

                var responseData = Mapper.Map<District, DistrictViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, DistrictViewModel districtVm)
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
                    var newDistrict = new District();
                    newDistrict.UpdateDistrict(districtVm);
                    _districtService.Add(newDistrict);
                    _districtService.Save();
                    var responseData = Mapper.Map<District, DistrictViewModel>(newDistrict);
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
                    var oldDistrict = _districtService.Delete(id);
                    _districtService.Save();
                    var responseData = Mapper.Map<District, DistrictViewModel>(oldDistrict);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedDistricts)
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
                    var listDistricts = new JavaScriptSerializer().Deserialize<List<int>>(checkedDistricts);
                    foreach (var item in listDistricts)
                    {
                        _districtService.Delete(item);
                    }

                    _districtService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listDistricts.Count);
                }

                return response;
            });
        }
    }
}