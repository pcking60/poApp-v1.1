using AutoMapper;
using PostOffice.Common.ViewModels;
using PostOffice.Common.ViewModels.ExportModel;
using PostOffice.Common.ViewModels.RankModel;
using PostOffice.Common.ViewModels.StatisticModel;
using PostOffice.Model.Models;
using PostOffice.Web.Models;

namespace PostOffice.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
            Mapper.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
            Mapper.CreateMap<ApplicationUser, ApplicationUserViewModel>();
            Mapper.CreateMap<District, DistrictViewModel>();
            Mapper.CreateMap<PO, POViewModel>();
            Mapper.CreateMap<ServiceGroup, ServiceGroupViewModel>();
            Mapper.CreateMap<PostOffice.Model.Models.Service, ServiceViewModel>();
            Mapper.CreateMap<MainServiceGroup, MainServiceGroupViewModel>();
            Mapper.CreateMap<PropertyService, PropertyServiceViewModel>();
            Mapper.CreateMap<Transaction, TransactionViewModel>();
            Mapper.CreateMap<TransactionDetail, TransactionDetailViewModel>();
            Mapper.CreateMap<Model.Models.Service, ReportServiceViewModel>();
            Mapper.CreateMap<ServiceViewModel, ReportServiceViewModel>();
            Mapper.CreateMap<TKBDAmount, TKBDAmountViewModel>();
            Mapper.CreateMap<TKBDHistory, TKBDHistoryViewModel>();
            Mapper.CreateMap<TransactionViewModel, RP2_1>();
            Mapper.CreateMap<TransactionViewModel, Export_Template_VM>();
            Mapper.CreateMap<TransactionViewModel, MainGroup3>();
            Mapper.CreateMap<TKBD_Export_Template, TKBD_Export_Template_ViewModel>();
            Mapper.CreateMap<TKBD_Export_Detail_Template, TKBD_Export_Detail_Template_ViewModel>();
            Mapper.CreateMap<TKBD_History_Statistic, TKBD_History_Statistic_ViewModel>();
            Mapper.CreateMap<Export_By_Service_Group_And_Time_District_Po_BCCP, Export_By_Service_Group_And_Time_District_Po_BCCP_VM>();
            Mapper.CreateMap<Export_By_Service_Group_And_Time_District_Po_BCCP_VM, Export_Template_VM>();
            Mapper.CreateMap<Export_By_Service_Group_TCBC, Export_By_Service_Group_TCBC_Vm>();
            Mapper.CreateMap<Rank, RankAfter>();
            Mapper.CreateMap<Get_General_TCBC, Get_General_TCBC_VM>();
            Mapper.CreateMap<Get_General_TCBC_VM, Export_Template_TCBC>();
            Mapper.CreateMap<TransactionViewModel, Get_General_TCBC>();
        }
    }
}