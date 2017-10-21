using AutoMapper;
using PostOffice.Common;
using PostOffice.Common.ViewModels;
using PostOffice.Model.Models;
using PostOffice.Service;
using PostOffice.Web.Infrastructure.Core;
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
                DateTime f;
                DateTime h;          
                // Thời gian để xuất dữ liệu
                if (DateTime.TryParseExact(fromDate, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out f))
                {
                    DateTime.TryParse(f.ToString("dd/MM/yyyy" ), out h);
                    vm.FromDate = h;             
                }
                else
                {
                    vm.FromDate = DateTime.ParseExact(fromDate, "MM/dd/yyyy", null);
                }

                if (DateTime.TryParseExact(toDate, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out f))
                {
                    DateTime.TryParse(f.ToString("dd/MM/yyyy"), out h);
                    vm.ToDate = h;
                }
                else
                {
                    vm.FromDate = DateTime.ParseExact(fromDate, "MM/dd/yyyy", null);
                }


                vm.CreatedBy = User.Identity.Name;

                //rp1Advance = _statisticService.RP1Advance();

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

                //var listMainGroup = _mainGroupService.GetAll();

                const int bccpId = 1; //BCCP
                const int ppttId = 2; //PPTT
                const int tcbcId = 3; //TCBC
                const int otherId = 4; //OTHER
                string currentUser = User.Identity.Name;
                switch (functionId)
                {
                    #region case 1 Bảng kê thu tiền tại bưu cục - tổng hợp

                    case 1:
                        vm.FunctionName = "Bảng kê thu tiền tại bưu cục - tổng hợp";

                        var responseBCCP = _statisticService.Export_By_Service_Group_And_Time_District_Po_BCCP(fromDate, toDate, districtId, poId, currentUser, userId);
                        var responsePPTT = _statisticService.Export_By_Service_Group_And_Time_District_Po_PPTT(fromDate, toDate, districtId, poId, currentUser, userId);
                        var responseTCBC = _statisticService.Export_By_Service_Group_TCBC(fromDate, toDate, districtId, poId, currentUser, userId);
                        //var responseOther = _statisticService.Export_By_Service_Group_And_Time(fromDate, toDate, otherId, districtId, poId, userId);
                        await ReportHelper.Export_By_Service_Group_And_Time(responseBCCP.ToList(), responsePPTT.ToList(), responseTCBC.ToList(), fullPath, vm);

                        break;

                    #endregion case 1 Bảng kê thu tiền tại bưu cục - tổng hợp

                    #region case 2 Bảng kê thu tiền tại bưu cục - chi tiết

                    case 2:
                        vm.FunctionName = "Bảng kê thu tiền tại bưu cục - chi tiết";
                        if (!isAdmin && !isManager)
                        {
                            break;
                        }
                        if (isAdmin)
                        {
                            if (districtId == 0)
                            {
                                var modelGg1 = _trasactionService.GetAllByMainGroupId(DateTime.ParseExact(fromDate, "MM/dd/yyyy", null), DateTime.Parse(toDate), bccpId);
                                var modelGg2 = _trasactionService.GetAllByMainGroupId(DateTime.Parse(fromDate), DateTime.Parse(toDate), ppttId);
                                var modelGg3 = _trasactionService.GetAllByMainGroupId(DateTime.Parse(fromDate), DateTime.Parse(toDate), tcbcId);
                                var modelGg4 = _trasactionService.GetAllByMainGroupId(DateTime.Parse(fromDate), DateTime.Parse(toDate), otherId);
                                var responseGg1 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg1);
                                var responseGg2 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg2);
                                var responseGg3 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg3);
                                var responseGg4 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg4);

                                // main group 1 - BCCP

                                #region BCCP

                                foreach (var item in responseGg1)
                                {
                                    item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                    item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                    if (!item.IsCash)
                                    {
                                        item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    else
                                    {
                                        item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                }
                                var responseDBGg1 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup1>>(responseGg1);
                                foreach (var item in responseDBGg1)
                                {
                                    if (item.TotalDebt > 0 && item.VAT > 0)
                                    {
                                        item.VatOfTotalDebt = item.TotalDebt - item.TotalDebt / Convert.ToDecimal(item.VAT);
                                    }
                                    if (item.TotalCash > 0 && item.VAT > 0)
                                    {
                                        item.VatOfTotalCash = item.TotalCash - item.TotalCash / Convert.ToDecimal(item.VAT);
                                    }
                                }

                                #endregion BCCP

                                // main group 2 - PPTT

                                #region PPTT

                                foreach (var item in responseGg2)
                                {
                                    item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                    item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                    if (!item.IsCash)
                                    {
                                        item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    else
                                    {
                                        item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                }
                                var responseDBGg2 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup1>>(responseGg2);
                                foreach (var item in responseDBGg2)
                                {
                                    if (item.TotalDebt > 0 && item.VAT > 0)
                                    {
                                        item.VatOfTotalDebt = item.TotalDebt - item.TotalDebt / Convert.ToDecimal(item.VAT);
                                    }
                                    if (item.TotalCash > 0 && item.VAT > 0)
                                    {
                                        item.VatOfTotalCash = item.TotalCash - item.TotalCash / Convert.ToDecimal(item.VAT);
                                    }
                                }

                                #endregion PPTT

                                // main group 3 - TCBC

                                #region TCBC

                                foreach (var item in responseGg3)
                                {
                                    item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                    item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                    var groupId = _serviceGroupService.GetSigleByServiceId(item.ID);
                                    if (groupId != null && (groupId.ID == 93 || groupId.ID == 75))
                                    {
                                        item.IsReceive = true;
                                        item.TotalMoneyReceive = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    else
                                    {
                                        item.IsReceive = false;
                                        item.TotalMoneySent = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                }
                                var responseDBGg3 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup3>>(responseGg3);

                                #endregion TCBC

                                await ReportHelper.RP2_1(responseDBGg1.ToList(), responseDBGg2.ToList(), responseDBGg3.ToList(), fullPath, vm);
                            }//end if districtId=0
                            else
                            {
                                if (poId == 0)
                                {
                                    var modelGg1 = _trasactionService.GetAllBy_Time_DistrictID_MainGroupId(DateTime.ParseExact(fromDate, "MM/dd/yyyy", null), DateTime.Parse(toDate), districtId, bccpId);
                                    var modelGg2 = _trasactionService.GetAllBy_Time_DistrictID_MainGroupId(DateTime.Parse(fromDate), DateTime.Parse(toDate), districtId, ppttId);
                                    var modelGg3 = _trasactionService.GetAllBy_Time_DistrictID_MainGroupId(DateTime.Parse(fromDate), DateTime.Parse(toDate), districtId, tcbcId);
                                    var modelGg4 = _trasactionService.GetAllBy_Time_DistrictID_MainGroupId(DateTime.Parse(fromDate), DateTime.Parse(toDate), districtId, otherId);
                                    var responseGg1 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg1);
                                    var responseGg2 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg2);
                                    var responseGg3 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg3);
                                    var responseGg4 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg4);

                                    // main group 1 - BCCP

                                    #region BCCP

                                    foreach (var item in responseGg1)
                                    {
                                        item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                        item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                        item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                        if (!item.IsCash)
                                        {
                                            item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                        }
                                        else
                                        {
                                            item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                        }
                                        item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                    }
                                    var responseDBGg1 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup1>>(responseGg1);
                                    foreach (var item in responseDBGg1)
                                    {
                                        if (item.TotalDebt > 0 && item.VAT > 0)
                                        {
                                            item.VatOfTotalDebt = item.TotalDebt - item.TotalDebt / Convert.ToDecimal(item.VAT);
                                        }
                                        if (item.TotalCash > 0 && item.VAT > 0)
                                        {
                                            item.VatOfTotalCash = item.TotalCash - item.TotalCash / Convert.ToDecimal(item.VAT);
                                        }
                                    }

                                    #endregion BCCP

                                    // main group 2 - PPTT

                                    #region PPTT

                                    foreach (var item in responseGg2)
                                    {
                                        item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                        item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                        item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                        if (!item.IsCash)
                                        {
                                            item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                        }
                                        else
                                        {
                                            item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                        }
                                        item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                    }
                                    var responseDBGg2 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup1>>(responseGg2);
                                    foreach (var item in responseDBGg2)
                                    {
                                        if (item.TotalDebt > 0 && item.VAT > 0)
                                        {
                                            item.VatOfTotalDebt = item.TotalDebt - item.TotalDebt / Convert.ToDecimal(item.VAT);
                                        }
                                        if (item.TotalCash > 0 && item.VAT > 0)
                                        {
                                            item.VatOfTotalCash = item.TotalCash - item.TotalCash / Convert.ToDecimal(item.VAT);
                                        }
                                    }

                                    #endregion PPTT

                                    // main group 3 - TCBC

                                    #region TCBC

                                    foreach (var item in responseGg3)
                                    {
                                        item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                        item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                        item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                        item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                        var groupId = _serviceGroupService.GetSigleByServiceId(item.ID);
                                        if (groupId != null && groupId.ID == 93)
                                        {
                                            item.IsReceive = true;
                                            item.TotalMoneyReceive = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                        }
                                        else
                                        {
                                            item.IsReceive = false;
                                            item.TotalMoneySent = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                        }
                                    }
                                    var responseDBGg3 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup3>>(responseGg3);

                                    #endregion TCBC

                                    await ReportHelper.RP2_1(responseDBGg1.ToList(), responseDBGg2.ToList(), responseDBGg3.ToList(), fullPath, vm);
                                }
                                else
                                {
                                    var modelGg1 = _trasactionService.GetAllBy_Time_DistrictID_POID_MainGroupId(DateTime.Parse(fromDate), DateTime.Parse(toDate), districtId, poId, bccpId);
                                    var modelGg2 = _trasactionService.GetAllBy_Time_DistrictID_POID_MainGroupId(DateTime.Parse(fromDate), DateTime.Parse(toDate), districtId, poId, ppttId);
                                    var modelGg3 = _trasactionService.GetAllBy_Time_DistrictID_POID_MainGroupId(DateTime.Parse(fromDate), DateTime.Parse(toDate), districtId, poId, tcbcId);
                                    var modelGg4 = _trasactionService.GetAllBy_Time_DistrictID_POID_MainGroupId(DateTime.Parse(fromDate), DateTime.Parse(toDate), districtId, poId, otherId);
                                    var responseGg1 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg1);
                                    var responseGg2 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg2);
                                    var responseGg3 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg3);
                                    var responseGg4 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg4);

                                    // main group 1 - BCCP

                                    #region BCCP

                                    foreach (var item in responseGg1)
                                    {
                                        item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                        item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                        item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                        if (!item.IsCash)
                                        {
                                            item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                        }
                                        else
                                        {
                                            item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                        }
                                        item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                    }
                                    var responseDBGg1 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup1>>(responseGg1);
                                    foreach (var item in responseDBGg1)
                                    {
                                        if (item.TotalDebt > 0 && item.VAT > 0)
                                        {
                                            item.VatOfTotalDebt = item.TotalDebt - item.TotalDebt / Convert.ToDecimal(item.VAT);
                                        }
                                        if (item.TotalCash > 0 && item.VAT > 0)
                                        {
                                            item.VatOfTotalCash = item.TotalCash - item.TotalCash / Convert.ToDecimal(item.VAT);
                                        }
                                    }

                                    #endregion BCCP

                                    // main group 2 - PPTT

                                    #region PPTT

                                    foreach (var item in responseGg2)
                                    {
                                        item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                        item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                        item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                        if (!item.IsCash)
                                        {
                                            item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                        }
                                        else
                                        {
                                            item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                        }
                                        item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                    }
                                    var responseDBGg2 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup1>>(responseGg2);
                                    foreach (var item in responseDBGg2)
                                    {
                                        if (item.TotalDebt > 0 && item.VAT > 0)
                                        {
                                            item.VatOfTotalDebt = item.TotalDebt - item.TotalDebt / Convert.ToDecimal(item.VAT);
                                        }
                                        if (item.TotalCash > 0 && item.VAT > 0)
                                        {
                                            item.VatOfTotalCash = item.TotalCash - item.TotalCash / Convert.ToDecimal(item.VAT);
                                        }
                                    }

                                    #endregion PPTT

                                    // main group 3 - TCBC

                                    #region TCBC

                                    foreach (var item in responseGg3)
                                    {
                                        item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                        item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                        item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                        item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                        var groupId = _serviceGroupService.GetSigleByServiceId(item.ID);
                                        if (groupId != null && groupId.ID == 93)
                                        {
                                            item.IsReceive = true;
                                            item.TotalMoneyReceive = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                        }
                                        else
                                        {
                                            item.IsReceive = false;
                                            item.TotalMoneySent = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                        }
                                    }
                                    var responseDBGg3 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup3>>(responseGg3);

                                    #endregion TCBC

                                    await ReportHelper.RP2_1(responseDBGg1.ToList(), responseDBGg2.ToList(), responseDBGg3.ToList(), fullPath, vm);
                                }
                            }
                        }
                        else
                        {
                            if (poId == 0)
                            {
                                districtId = _districtService.GetDistrictByUserName(currentUser).ID;
                                var modelGg1 = _trasactionService.GetAllBy_Time_DistrictID_MainGroupId(DateTime.ParseExact(fromDate, "MM/dd/yyyy", null), DateTime.ParseExact(toDate, "MM/dd/yyyy", null), districtId, bccpId);
                                var modelGg2 = _trasactionService.GetAllBy_Time_DistrictID_MainGroupId(DateTime.ParseExact(fromDate, "MM/dd/yyyy", null), DateTime.ParseExact(toDate, "MM/dd/yyyy", null), districtId, ppttId);
                                var modelGg3 = _trasactionService.GetAllBy_Time_DistrictID_MainGroupId(DateTime.ParseExact(fromDate, "MM/dd/yyyy", null), DateTime.ParseExact(toDate, "MM/dd/yyyy", null), districtId, tcbcId);
                                var modelGg4 = _trasactionService.GetAllBy_Time_DistrictID_MainGroupId(DateTime.ParseExact(fromDate, "MM/dd/yyyy", null), DateTime.ParseExact(toDate, "MM/dd/yyyy", null), districtId, otherId);
                                var responseGg1 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg1);
                                var responseGg2 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg2);
                                var responseGg3 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg3);
                                var responseGg4 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg4);

                                // main group 1 - BCCP

                                #region BCCP

                                foreach (var item in responseGg1)
                                {
                                    item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                    item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                    if (!item.IsCash)
                                    {
                                        item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    else
                                    {
                                        item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                }
                                var responseDBGg1 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup1>>(responseGg1);
                                foreach (var item in responseDBGg1)
                                {
                                    if (item.TotalDebt > 0 && item.VAT > 0)
                                    {
                                        item.VatOfTotalDebt = item.TotalDebt - item.TotalDebt / Convert.ToDecimal(item.VAT);
                                    }
                                    if (item.TotalCash > 0 && item.VAT > 0)
                                    {
                                        item.VatOfTotalCash = item.TotalCash - item.TotalCash / Convert.ToDecimal(item.VAT);
                                    }
                                }

                                #endregion BCCP

                                // main group 2 - PPTT

                                #region PPTT

                                foreach (var item in responseGg2)
                                {
                                    item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                    item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                    if (!item.IsCash)
                                    {
                                        item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    else
                                    {
                                        item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                }
                                var responseDBGg2 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup1>>(responseGg2);
                                foreach (var item in responseDBGg2)
                                {
                                    if (item.TotalDebt > 0 && item.VAT > 0)
                                    {
                                        item.VatOfTotalDebt = item.TotalDebt - item.TotalDebt / Convert.ToDecimal(item.VAT);
                                    }
                                    if (item.TotalCash > 0 && item.VAT > 0)
                                    {
                                        item.VatOfTotalCash = item.TotalCash - item.TotalCash / Convert.ToDecimal(item.VAT);
                                    }
                                }

                                #endregion PPTT

                                // main group 3 - TCBC

                                #region TCBC

                                foreach (var item in responseGg3)
                                {
                                    item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                    item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                    var groupId = _serviceGroupService.GetSigleByServiceId(item.ID);
                                    if (groupId != null && groupId.ID == 93)
                                    {
                                        item.IsReceive = true;
                                        item.TotalMoneyReceive = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    else
                                    {
                                        item.IsReceive = false;
                                        item.TotalMoneySent = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                }
                                var responseDBGg3 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup3>>(responseGg3);

                                #endregion TCBC

                                await ReportHelper.RP2_1(responseDBGg1.ToList(), responseDBGg2.ToList(), responseDBGg3.ToList(), fullPath, vm);
                            }
                            else
                            {
                                districtId = _districtService.GetDistrictByUserName(currentUser).ID;
                                poId = _poService.GetPOByCurrentUser(currentUser).ID;
                                
                                var modelGg1 = _trasactionService.GetAllBy_Time_DistrictID_POID_MainGroupId(DateTime.ParseExact(fromDate, "MM/dd/yyyy", null), DateTime.ParseExact(fromDate, "MM/dd/yyyy", null), districtId, poId, bccpId);
                                var modelGg2 = _trasactionService.GetAllBy_Time_DistrictID_POID_MainGroupId(DateTime.ParseExact(fromDate, "MM/dd/yyyy", null), DateTime.ParseExact(toDate, "MM/dd/yyyy", null), districtId, poId, ppttId);
                                var modelGg3 = _trasactionService.GetAllBy_Time_DistrictID_POID_MainGroupId(DateTime.ParseExact(fromDate, "MM/dd/yyyy", null), DateTime.ParseExact(toDate, "MM/dd/yyyy", null), districtId, poId, tcbcId);
                                var modelGg4 = _trasactionService.GetAllBy_Time_DistrictID_POID_MainGroupId(DateTime.ParseExact(fromDate, "MM/dd/yyyy", null), DateTime.ParseExact(toDate, "MM/dd/yyyy", null), districtId, poId, otherId);
                                var responseGg1 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg1);
                                var responseGg2 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg2);
                                var responseGg3 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg3);
                                var responseGg4 = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(modelGg4);

                                // main group 1 - BCCP

                                #region BCCP

                                foreach (var item in responseGg1)
                                {
                                    item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                    item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                    if (!item.IsCash)
                                    {
                                        item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    else
                                    {
                                        item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                }
                                var responseDBGg1 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup1>>(responseGg1);
                                foreach (var item in responseDBGg1)
                                {
                                    if (item.TotalDebt > 0 && item.VAT > 0)
                                    {
                                        item.VatOfTotalDebt = item.TotalDebt - item.TotalDebt / Convert.ToDecimal(item.VAT);
                                    }
                                    if (item.TotalCash > 0 && item.VAT > 0)
                                    {
                                        item.VatOfTotalCash = item.TotalCash - item.TotalCash / Convert.ToDecimal(item.VAT);
                                    }
                                }

                                #endregion BCCP

                                // main group 2 - PPTT

                                #region PPTT

                                foreach (var item in responseGg2)
                                {
                                    item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                    item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                    if (!item.IsCash)
                                    {
                                        item.TotalDebt = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    else
                                    {
                                        item.TotalCash = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                }
                                var responseDBGg2 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup1>>(responseGg2);
                                foreach (var item in responseDBGg2)
                                {
                                    if (item.TotalDebt > 0 && item.VAT > 0)
                                    {
                                        item.VatOfTotalDebt = item.TotalDebt - item.TotalDebt / Convert.ToDecimal(item.VAT);
                                    }
                                    if (item.TotalCash > 0 && item.VAT > 0)
                                    {
                                        item.VatOfTotalCash = item.TotalCash - item.TotalCash / Convert.ToDecimal(item.VAT);
                                    }
                                }

                                #endregion PPTT

                                // main group 3 - TCBC

                                #region TCBC

                                foreach (var item in responseGg3)
                                {
                                    item.VAT = _serviceService.GetById(item.ServiceId).VAT;
                                    item.Quantity = Convert.ToInt32(_transactionDetailService.GetAllByCondition("Sản lượng", item.ID).Money);
                                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                                    var groupId = _serviceGroupService.GetSigleByServiceId(item.ID);
                                    if (groupId != null && groupId.ID == 93)
                                    {
                                        item.IsReceive = true;
                                        item.TotalMoneyReceive = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                    else
                                    {
                                        item.IsReceive = false;
                                        item.TotalMoneySent = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                                    }
                                }
                                var responseDBGg3 = Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<MainGroup3>>(responseGg3);

                                #endregion TCBC

                                await ReportHelper.RP2_1(responseDBGg1.ToList(), responseDBGg2.ToList(), responseDBGg3.ToList(), fullPath, vm);
                            }
                        }

                        break;

                    #endregion case 2 Bảng kê thu tiền tại bưu cục - chi tiết

                    case 3:
                        vm.FunctionName = "Bảng kê thu tiền theo nhân viên";
                        var responseBCCP3 = _statisticService.Export_By_Service_Group_And_Time_District_Po_BCCP(fromDate, toDate, districtId, poId, currentUser, userId);
                        var responsePPTT3 = _statisticService.Export_By_Service_Group_And_Time_District_Po_PPTT(fromDate, toDate, districtId, poId, currentUser, userId);
                        var responseTCBC3 = _statisticService.Export_By_Service_Group_TCBC(fromDate, toDate, districtId, poId, currentUser, userId);
                        //var responseOther3 = _statisticService.Export_By_Service_Group_And_Time(fromDate, toDate, otherId, districtId, poId, userId);
                        await ReportHelper.Export_By_Service_Group_And_Time(responseBCCP3.ToList(), responsePPTT3.ToList(), responseTCBC3.ToList(), fullPath, vm);

                        break;

                    case 4:
                        vm.FunctionName = "Bảng kê thu tiền theo dịch vụ";
                        break;

                    case 5:
                        vm.FunctionName = "Bảng kê thu tiền theo nhân viên và dịch vụ";
                        break;

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
    }
}