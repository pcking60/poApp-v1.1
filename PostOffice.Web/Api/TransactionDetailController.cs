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
    [RoutePrefix("api/transactiondetails")]
    [Authorize]
    public class TransactionDetailController : ApiControllerBase
    {
        private ITransactionDetailService _transactionDetailService;
        private IErrorService _errorService;
        private IPropertyServiceService _propertyServiceService;

        public TransactionDetailController(IErrorService errorService, IPropertyServiceService propertyServiceService, ITransactionDetailService transactionDetailService) : base(errorService)
        {
            this._transactionDetailService = transactionDetailService;
            this._propertyServiceService = propertyServiceService;
            this._errorService = errorService;
        }

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAllParentID(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _transactionDetailService.GetAll();
                var responseData = Mapper.Map<IEnumerable<TransactionDetail>, IEnumerable<TransactionDetailViewModel>>(model);
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
                var model = _transactionDetailService.GetAll();
                totalRow = model.Count();
                var query = model.OrderBy(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<TransactionDetail>, IEnumerable<TransactionDetailViewModel>>(query);


                var paginationSet = new PaginationSet<TransactionDetailViewModel>
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
        public HttpResponseMessage Update(HttpRequestMessage request, TransactionDetailViewModel propVM)
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
                    var dbTransactionDetail = _transactionDetailService.GetById(propVM.ID);
                    dbTransactionDetail.UpdateTransactionDetail(propVM);
                    _transactionDetailService.Update(dbTransactionDetail);
                    _transactionDetailService.Save();
                    var responseData = Mapper.Map<TransactionDetail, TransactionDetailViewModel>(dbTransactionDetail);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("getbytransactionid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetByTransactionId(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _transactionDetailService.GetAllByTransactionId(id);

                var responseData = Mapper.Map<IEnumerable<TransactionDetail>, IEnumerable<TransactionDetailViewModel>>(model);

                foreach (var item in responseData)
                {
                    item.PropertyServiceName = _propertyServiceService.GetById(item.PropertyServiceId).Name;
                }

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, TransactionDetailViewModel transactionDetailVM)
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
                    var newTransactionDetail = new TransactionDetail();
                    newTransactionDetail.UpdateTransactionDetail(transactionDetailVM);
                    _transactionDetailService.Add(newTransactionDetail);
                    _transactionDetailService.Save();
                    var responseData = Mapper.Map<TransactionDetail, TransactionDetailViewModel>(newTransactionDetail);
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
                    var oldTransactionDetail = _transactionDetailService.Delete(id);
                    _transactionDetailService.Save();
                    var responseData = Mapper.Map<TransactionDetail, TransactionDetailViewModel>(oldTransactionDetail);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedTransactionDetails)
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
                    var listTransactionDetails = new JavaScriptSerializer().Deserialize<List<int>>(checkedTransactionDetails);
                    foreach (var item in listTransactionDetails)
                    {
                        _transactionDetailService.Delete(item);
                    }

                    _transactionDetailService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listTransactionDetails.Count);
                }

                return response;
            });
        }
    }
}