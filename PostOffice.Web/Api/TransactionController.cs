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
    [RoutePrefix("api/transactions")]
    [Authorize]
    public class TransactionController : ApiControllerBase
    {
        private ITransactionService _transactionService;
        private ITransactionDetailService _transactionDetailService;
        private IApplicationUserService _userService;
        private IServiceService _serviceService;
        private IErrorService _errorService;
        private IServiceGroupService _serviceGr;

        public TransactionController(IServiceGroupService serviceGr, IErrorService errorService, IServiceService serviceService, ITransactionDetailService transactionDetailService, ITransactionService transactionService, IApplicationUserService userService) : base(errorService)
        {
            this._transactionService = transactionService;
            _transactionDetailService = transactionDetailService;
            this._errorService = errorService;
            _serviceService = serviceService;
            _userService = userService;
            _serviceGr = serviceGr;
        }

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAllParentID(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _transactionService.GetAll();
                var responseData = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(model);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("stattistic")]
        [HttpGet]
        public HttpResponseMessage GetAllByTime(HttpRequestMessage request, DateTime fromDate, DateTime toDate, int districtId, int posId, string userId, int serviceId)
        {
            return CreateHttpResponse(request, () =>
            {
                try
                {
                    var model = _transactionService.General_statistic(fromDate, toDate, districtId, posId, User.Identity.Name, userId, serviceId);
                    var responseData = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(model);
                  
                    foreach (var item in responseData)
                    {
                        item.groupId = _serviceGr.GetGroupIdByServiceId(item.ServiceId);
                        item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                        item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                        item.ServiceName = _serviceService.GetById(item.ServiceId).Name;  

                        if (!item.IsCash && item.groupId != 94)
                        {
                            item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                        }
                        else
                        {
                            
                            if (item.groupId == 94)
                            {
                                if (item.IsCurrency && item.ServiceId == 1769) 
                                {
                                    item.TotalCurrency = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                }
                                else
                                {
                                    item.TotalMoneySent = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                }                                
                            }
                            else
                            {
                                item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                            }
                        }
                        item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                    }
                        var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                        return response;
                }
                catch (Exception e)
                {
                    return request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
                }
            });
        }

        [Route("getEarnMoneyByUserName")]
        [HttpGet]
        public decimal? GetEarnMoneyByUserName(string userName)
        {
            decimal? totalEarn = _transactionDetailService.GetTotalEarnMoneyByUsername(userName);
            return totalEarn;
        }

        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize = 40)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var userName = User.Identity.Name;
                var model = _transactionService.GetAllBy_UserName_Now(userName);
                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.Status).ThenBy(x=>x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.groupId = _serviceGr.GetGroupIdByServiceId(item.ServiceId);
                    item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                    item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;

                    if (!item.IsCash && item.groupId != 94)
                    {
                        item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                    }
                    else
                    {

                        if (item.groupId == 94)
                        {
                            if (item.IsCurrency && item.ServiceId == 1769)
                            {
                                item.TotalCurrency = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                            }
                            else
                            {
                                item.TotalMoneySent = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                            }
                        }
                        else
                        {
                            item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                        }
                    }
                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                }

                var paginationSet = new PaginationSet<TransactionViewModel>
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

        [Route("getallbyuserid")]
        public HttpResponseMessage GetAllByUserId(HttpRequestMessage request, string userId, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _transactionService.GetAllBy_UserId(userId);
                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.TransactionDate).ThenBy(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.groupId = _serviceGr.GetGroupIdByServiceId(item.ServiceId);
                    item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                    item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;

                    if (item.groupId == 94)     // chi ho
                    {
                        if (item.IsCurrency && item.ServiceId == 1769) // ngoai te
                        {
                            item.TotalCurrency = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                        }
                        else
                        {
                            item.TotalMoneySent = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                            item.TotalFee = _transactionDetailService.GetTotalFeeByTransactionId(item.ID);
                        }
                    }
                    else
                    {
                        if(item.groupId==93)    // thu ho
                        {
                            item.TotalColection = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                            item.TotalFee = _transactionDetailService.GetTotalFeeByTransactionId(item.ID);
                        }
                        else
                        {
                            if (item.IsCash)    // tien mat
                            {
                                item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                            }
                            else
                            {
                                item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                            }
                        }
                    }                   
                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);   // DTTL
                    item.Sales = item.EarnMoney + item.EarnMoney * 10 / 100;
                    item.TotalVat = item.EarnMoney * 10 / 100;
                }

                var paginationSet = new PaginationSet<TransactionViewModel>
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
        [Route("getall7days")]
        public HttpResponseMessage GetAll7Days(HttpRequestMessage request, int page, int pageSize = 40)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var userName = User.Identity.Name;
                var model = _transactionService.GetAllBy_UserName_7_Days(userName);
                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.Status).ThenBy(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.groupId = _serviceGr.GetGroupIdByServiceId(item.ServiceId);
                    item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                    item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;

                    if (!item.IsCash && item.groupId != 94)
                    {
                        item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                    }
                    else
                    {

                        if (item.groupId == 94)
                        {
                            if (item.IsCurrency && item.ServiceId == 1769)
                            {
                                item.TotalCurrency = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                            }
                            else
                            {
                                item.TotalMoneySent = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                            }
                        }
                        else
                        {
                            item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                        }
                    }
                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                }

                var paginationSet = new PaginationSet<TransactionViewModel>
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
       

        [Route("getall30days")]
        public HttpResponseMessage GetAll30Days(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var userName = User.Identity.Name;
                var model = _transactionService.GetAllBy_UserName_30_Days(userName);
                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.TransactionDate).ThenBy(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.groupId = _serviceGr.GetGroupIdByServiceId(item.ServiceId);
                    item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                    item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;

                    if (!item.IsCash && item.groupId != 94)
                    {
                        item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                    }
                    else
                    {

                        if (item.groupId == 94)
                        {
                            if (item.IsCurrency && item.ServiceId == 1769)
                            {
                                item.TotalCurrency = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                            }
                            else
                            {
                                item.TotalMoneySent = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                            }
                        }
                        else
                        {
                            item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                        }
                    }
                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                }

                var paginationSet = new PaginationSet<TransactionViewModel>
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

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public IHttpActionResult Delete(HttpRequestMessage request, int id)
        {
            if (!ModelState.IsValid)
            {
                return Json("302");
            }
            else
            {
                try
                {
                    var oldTransaction = _transactionService.GetById(id);
                    oldTransaction.Status = false;
                    var transactionDetails = _transactionDetailService.GetAllByTransactionId(oldTransaction.ID);
                    _transactionService.Update(oldTransaction);
                    foreach (var item in transactionDetails)
                    {
                        item.Status = false;
                    }
                    _transactionService.Save();
                    return Json(oldTransaction.ID);
                }
                catch(Exception e)
                {
                    return Json(e.InnerException.Message);
                }
                
            }
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, TransactionViewModel transactionVM)
        {
            if (!ModelState.IsValid)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                //return Json("301");
            }
            else
            {
                const double dateDelete = 2.5 * 24 * 60 * 60 * 1000;
                var dbTransaction = _transactionService.GetById(transactionVM.ID);

                var currentDate = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                if (transactionVM.TransactionDate == null)
                {
                    transactionVM.TransactionDate = dbTransaction.TransactionDate;
                }

                var transactionDate = transactionVM.TransactionDate.Ticks / TimeSpan.TicksPerMillisecond;

                bool isValid = (currentDate - transactionDate) > dateDelete;

                if (isValid)
                {
                    string message = "Chỉ cho phép chỉnh sửa trong thời hạn 2 ngày";
                    return request.CreateResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {
                    
                    ICollection<TransactionDetail> transactionDetails = transactionVM.TransactionDetails;
                    var responseTransactionDetail = Mapper.Map<IEnumerable<TransactionDetail>, IEnumerable<TransactionDetailViewModel>>(transactionDetails);
                    transactionVM.UpdatedBy = User.Identity.Name;
                    Transaction ts = new Transaction();
                    dbTransaction.UpdateTransaction(transactionVM);
                    _transactionService.Update(dbTransaction);
                    _transactionService.Save();
                    foreach (var item in responseTransactionDetail)
                    {
                        item.UpdatedDate = DateTime.Now;
                        item.UpdatedBy = User.Identity.Name;
                        var transactionDetail = new TransactionDetail();
                        transactionDetail.UpdateTransactionDetail(item);
                        _transactionDetailService.Update(transactionDetail);
                        _transactionDetailService.Save();
                    }
                    //var responseData = Mapper.Map<Transaction, TransactionViewModel>(dbTransaction);
                    //response = request.CreateResponse(HttpStatusCode.OK, responseData);

                    //update total earn money by username

                    //ApplicationUser user = _userService.getByUserName(User.Identity.Name);
                    //var responseData = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
                    //responseData.TotalEarn = _transactionDetailService.GetTotalEarnMoneyByUsername(user.UserName);
                    return request.CreateResponse(HttpStatusCode.OK, responseTransactionDetail);
                    // return Json(dbTransaction.ID);
                }
            }
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _transactionService.GetById(id);

                var responseData = Mapper.Map<Transaction, TransactionViewModel>(model);

                responseData.ServiceName = _serviceService.GetById(model.ServiceId).Name;

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, TransactionViewModel transactionVM)
        {
            return CreateHttpResponse(request, () =>
            {
                if (!ModelState.IsValid)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var transaction = new Transaction();
                    //var transactionDetails = transactionVM.TransactionDetails;
                    ICollection<TransactionDetail> transactionDetails = transactionVM.TransactionDetails;
                    var responseData = Mapper.Map<IEnumerable<TransactionDetail>, IEnumerable<TransactionDetailViewModel>>(transactionDetails);
                    //transactionVM.TransactionDetails = new List<TransactionDetail>();
                    if(transactionVM.UserId==null)
                    {
                        transactionVM.UserId = _userService.getByUserName(User.Identity.Name).Id;
                    }                    
                    transactionVM.CreatedBy = User.Identity.Name;
                    transaction.UpdateTransaction(transactionVM);
                    _transactionService.Add(transaction);
                    _transactionService.Save();
                    foreach (var item in responseData)
                    {
                        item.TransactionID = transaction.ID;
                        item.CreatedBy = User.Identity.Name;
                        item.CreatedDate = DateTime.Now;
                        var dbTransactionDetail = new TransactionDetail();
                        dbTransactionDetail.UpdateTransactionDetail(item);
                        _transactionDetailService.Add(dbTransactionDetail);
                        _transactionDetailService.Save();
                    }

                    //foreach (var item in transactionDetails)
                    //{
                    //    item.TransactionId = transactionVM.ID;
                    //    transactionVM.TransactionDetails.Add(item);
                    //}

                    //foreach (var item in responseData)
                    //{
                    //    var dbTransactionDetail = new TransactionDetail();
                    //    dbTransactionDetail.UpdateTransactionDetail(item);
                    //    dbTransactionDetail.TransactionId = item.ID;
                    //    //_transactionDetailService.Add(dbTransactionDetail);
                    //}

                    return request.CreateErrorResponse(HttpStatusCode.OK, transaction.ID.ToString());
                }
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedTransactions)
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
                    var listTransactions = new JavaScriptSerializer().Deserialize<List<int>>(checkedTransactions);
                    foreach (var item in listTransactions)
                    {
                        _transactionService.Delete(item);
                    }

                    _transactionService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listTransactions.Count);
                }

                return response;
            });
        }
    }
}