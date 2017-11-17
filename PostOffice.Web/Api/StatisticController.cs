using AutoMapper;
using PostOffice.Common;
using PostOffice.Common.ViewModels;
using PostOffice.Common.ViewModels.ExportModel;
using PostOffice.Model.Models;
using PostOffice.Service;
using PostOffice.Web.Infrastructure.Core;
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
    [RoutePrefix("api/statistic")]
    public class StatisticController : ApiControllerBase
    {
        private IStatisticService _statisticService;
        private IDistrictService _districtService;
        private IPOService _poService;
        private IApplicationUserService _userService;
        private IServiceService _serviceService;
        private ITransactionService _trasactionService;
        private ITransactionDetailService _transactionDetailService;
        private IMainServiceGroupService _mainGroupService;
        private IServiceGroupService _serviceGroupService;

        public StatisticController(IServiceGroupService serviceGroupService, IMainServiceGroupService mainGroupService, ITransactionDetailService transactionDetailService, ITransactionService trasactionService, IServiceService serviceService, IApplicationUserService userService, IErrorService errorService, IStatisticService statisticService, IDistrictService districtService, IPOService poService) : base(errorService)
        {
            _serviceGroupService = serviceGroupService;
            _mainGroupService = mainGroupService;
            _transactionDetailService = transactionDetailService;
            _trasactionService = trasactionService;
            _serviceService = serviceService;
            _userService = userService;
            _statisticService = statisticService;
            _districtService = districtService;
            _poService = poService;
        }

        [Route("getrevenue")]
        [HttpGet]
        public HttpResponseMessage GetRevenueStatistic(HttpRequestMessage request, string fromDate, string toDate)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _statisticService.GetRevenueStatistic(fromDate, toDate);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        [Route("getunit")]
        [HttpGet]
        public HttpResponseMessage GetUnitStatistic(HttpRequestMessage request, string fromDate, string toDate)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _statisticService.GetUnitStatistic(fromDate, toDate);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        [HttpGet]
        [Route("rp1")]
        public async Task<HttpResponseMessage> RP1(HttpRequestMessage request, string fromDate, string toDate, int districtId, int functionId, int poId, string userId, int serviceId)
        {
            //check role
            bool isAdmin = false;
            bool isManager = false;
            isAdmin = _userService.CheckRole(User.Identity.Name, "Administrator");
            isManager = _userService.CheckRole(User.Identity.Name, "Manager");
            bool isSupport = _userService.CheckRole(User.Identity.Name, "Support");

            #region Config Export file

            string fileName = string.Concat("Money_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);

            #endregion Config Export file

            ReportTemplate vm = new ReportTemplate();
            //IEnumerable<RP1Advance> rp1Advance;

            try
            {
                #region customFill Test

                District district = new District();
                PO po = new PO();
                ApplicationUser user = new ApplicationUser();
                Model.Models.Service sv = new Model.Models.Service();
                // current user
                string currentUser = User.Identity.Name;
                // Thời gian để xuất dữ liệu
                DateTime fd;
                DateTime td;
                try
                {
                    fd = DateTime.ParseExact(fromDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    vm.FromDate = fd;
                    td = DateTime.ParseExact(toDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    vm.ToDate = td;
                }
                catch (Exception)
                {
                    throw;
                }

                vm.CreatedBy = currentUser;
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
                if (serviceId != 0)
                {
                    sv = _serviceService.GetById(serviceId);
                    vm.Service = sv.Name;
                }

                #endregion data input
                
                switch (functionId)
                {
                    #region case 1 Bảng kê thu tiền tại bưu cục - tổng hợp

                    case 1:
                        vm.FunctionName = "Bảng kê thu tiền tại bưu cục - tổng hợp";
                        // BCCP
                        var query = _statisticService.Export_By_Service_Group_And_Time_District_Po_BCCP(fromDate, toDate, districtId, poId, currentUser, userId);
                        var c = query.Count();
                        var responseBCCP = Mapper.Map<IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP>, IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP_VM>>(query);
                        foreach (var item in responseBCCP)
                        {
                            item.TotalMoneyBeforeVat = (item.TotalCash + item.TotalDebt) / (decimal)item.VAT;
                            item.TotalVat = (item.TotalCash + item.TotalDebt) - ((item.TotalCash + item.TotalDebt) / (decimal)item.VAT);
                        }
                        var responseBCCP1 = Mapper.Map<IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP_VM>, IEnumerable<Export_Template_VM>>(responseBCCP);
                        //TCBC
                        var query2 = _statisticService.Get_General_TCBC(fromDate, toDate, districtId, poId, currentUser, userId);
                        var c2 = query2.Count();
                        var responseTCBC = Mapper.Map<IEnumerable<Get_General_TCBC>, IEnumerable<Get_General_TCBC_VM>>(query2);
                        foreach (var item in responseTCBC)
                        {
                            item.Tax =item.Sales - item.Sales / (decimal)item.VAT;
                        }
                        var responseTCBC1 = Mapper.Map<IEnumerable<Get_General_TCBC_VM>, IEnumerable<Export_Template_TCBC>>(responseTCBC);
                        //PPTT
                        var query3 = _statisticService.Export_By_Service_Group_And_Time_District_Po_PPTT(fromDate, toDate, districtId, poId, currentUser, userId);
                        var responsePPTT = Mapper.Map<IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP>, IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP_VM>>(query3);
                        foreach (var item in responsePPTT)
                        {
                            item.TotalMoneyBeforeVat = (item.TotalCash + item.TotalDebt) / (decimal)item.VAT;
                            item.TotalVat = (item.TotalCash + item.TotalDebt) - ((item.TotalCash + item.TotalDebt) / (decimal)item.VAT);
                        }
                        var responsePPTT1 = Mapper.Map<IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP_VM>, IEnumerable<Export_Template_VM>>(responsePPTT);
                        await ReportHelper.Export_By_Service_Group_And_Time(responseBCCP1.ToList(), responsePPTT1.ToList(), responseTCBC1.ToList(), fullPath, vm);

                        break;

                    #endregion case 1 Bảng kê thu tiền tại bưu cục - tổng hợp

                    #region case 2 Bảng kê thu tiền tại bưu cục - chi tiết

                    case 2:
                        vm.FunctionName = "Bảng kê thu tiền tại bưu cục - chi tiết";
                        // check if basic user
                        if (!isAdmin && !isManager && !isSupport)
                        {
                            break;
                        }
                        #region BCCP
                        var q1 = _trasactionService.GetByCondition_BCCP(fd, td, districtId, poId, currentUser);
                        var c4 = q1.Count();
                        var BCCP = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(q1);
                        foreach (var item in BCCP)
                        {
                            item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                            item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                            item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                            if (!item.IsCash)
                            {
                                item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                item.TotalCash = 0;
                            }
                            else
                            {
                                item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                item.TotalDebt = 0;
                            }
                            item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                            item.TotalMoneyBeforeVat = (item.TotalCash + item.TotalDebt) / (decimal)item.VAT;
                            item.TotalVat = (item.TotalCash + item.TotalDebt) - ((item.TotalCash + item.TotalDebt) / (decimal)item.VAT);
                        }
                        var resBCCP = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<Export_Template_VM>>(BCCP);
                        foreach (var item in resBCCP)
                        {
                            var stt = item.STT;
                            var name = item.ServiceName;
                            var qty = item.Quantity;
                            var cash = item.TotalCash;
                            var debt = item.TotalDebt;
                            var m1 = item.TotalMoneyBeforeVat;
                            var vat = item.TotalVat;
                            var e = item.EarnMoney;
                        }
                        #endregion
                        #region TCBC
                        var q2 = _trasactionService.GetByCondition_TCBC(fd, td, districtId, poId, currentUser);
                        var c5 = q2.Count();
                        var TCBC = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(q2);
                        foreach (var item in TCBC)
                        {
                            item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                            item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);                            
                            var Fe =  _transactionDetailService.GetFeeById("Số tiền cước", item.ID);
                            if (Fe == null)
                            {
                                item.Fee = 0;
                            }
                            else
                            {
                                item.Fee = Fe.Money;
                            }
                            item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                            item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                            var groupId = _serviceGroupService.GetSigleByServiceId(item.ID);
                            if (groupId != null && (groupId.ID == 93 || groupId.ID == 75))
                            {
                                item.IsReceive = true;
                                item.TotalColection = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                item.TotalPay = 0;
                            }
                            else
                            {
                                item.IsReceive = false;
                                item.TotalPay = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                item.TotalColection = 0;
                            }
                        }
                        var resTCBC_temp = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<Get_General_TCBC>>(TCBC);
                        var resTCBC_temp2 = Mapper.Map<IEnumerable<Get_General_TCBC>, IEnumerable<Get_General_TCBC_VM>>(resTCBC_temp);
                        foreach (var item in resTCBC_temp2)
                        {
                            item.Sales = item.EarnMoney * (decimal)item.VAT;
                            item.Tax = item.Sales - item.Sales / (decimal)item.VAT;
                            var name = item.ServiceName;
                            var qty = item.Quantity;
                            var co = item.TotalColection;
                            var p = item.TotalPay;
                            var m1 = item.Sales;
                            var f = item.Fee;
                            var e = item.EarnMoney;
                        }
                        var resTCBC = Mapper.Map<IEnumerable<Get_General_TCBC_VM>, IEnumerable<Export_Template_TCBC>>(resTCBC_temp2);
                        #endregion
                        #region PPTT
                        var q3 = _trasactionService.GetByCondition_PPTT(fd, td, districtId, poId, currentUser);
                        var c6 = q3.Count();
                        var PPTT = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(q3);
                        foreach (var item in PPTT)
                        {
                            item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                            item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                            item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                            if (!item.IsCash)
                            {
                                item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                item.TotalCash = 0;
                            }
                            else
                            {
                                item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                item.TotalDebt = 0;
                            }
                            item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                            item.TotalMoneyBeforeVat = (item.TotalCash + item.TotalDebt) / (decimal)item.VAT;
                            item.TotalVat = (item.TotalCash + item.TotalDebt) - ((item.TotalCash + item.TotalDebt) / (decimal)item.VAT);
                        }
                        var resPPTT = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<Export_Template_VM>>(PPTT);
                        #endregion
                        await ReportHelper.RP2_1(resBCCP.ToList(), resPPTT.ToList(), resTCBC.ToList(), fullPath, vm);        
                        break;

                    #endregion customFill Test

                    #region case 3 Bảng kê thu tiền theo nhân viên

                    case 3:
                        vm.FunctionName = "Bảng kê thu tiền theo nhân viên";
                        var c3_bccp = _statisticService.Export_By_Service_Group_And_Time_District_Po_BCCP(fromDate, toDate, districtId, poId, currentUser, userId);
                        var responseBCCP3 = Mapper.Map<IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP>, IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP_VM>>(c3_bccp);
                        foreach (var item in responseBCCP3)
                        {
                            item.TotalMoneyBeforeVat = (item.TotalCash + item.TotalDebt) / (decimal)item.VAT;
                        }
                        var c3_pptt = _statisticService.Export_By_Service_Group_And_Time_District_Po_PPTT(fromDate, toDate, districtId, poId, currentUser, userId);
                        var responsePPTT3 = Mapper.Map<IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP>, IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP_VM>>(c3_pptt);
                        foreach (var item in responsePPTT3)
                        {
                            item.TotalMoneyBeforeVat = (item.TotalCash + item.TotalDebt) / (decimal)item.VAT;
                        }
                        var c3_tcbc = _statisticService.Export_By_Service_Group_TCBC(fromDate, toDate, districtId, poId, currentUser, userId);
                        var responseTCBC3 = Mapper.Map<IEnumerable<Export_By_Service_Group_TCBC>, IEnumerable<Export_By_Service_Group_TCBC_Vm>>(c3_tcbc);
                        foreach (var item in responseTCBC3)
                        {
                            item.TotalMoney = (item.TotalColection + item.TotalPay) / (decimal)item.VAT;
                        }
                        //var responseOther3 = _statisticService.Export_By_Service_Group_And_Time(fromDate, toDate, otherId, districtId, poId, userId);
                        await ReportHelper.Export_By_Service_Group_And_Time(responseBCCP3.ToList(), responsePPTT3.ToList(), responseTCBC3.ToList(), fullPath, vm);

                        break;
                    #endregion case 3 Bảng kê thu tiền theo nhân viên

                    #region feature function
                    case 4:
                        vm.FunctionName = "Bảng kê thu tiền theo dịch vụ";
                        break;

                    case 5:
                        vm.FunctionName = "Bảng kê thu tiền theo nhân viên và dịch vụ";
                        break;

                    default:
                        vm.FunctionName = "Chức năng khác";
                        break;
                        #endregion feature function
                }

                #endregion case 3 Bảng kê thu tiền theo nhân viên

                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}