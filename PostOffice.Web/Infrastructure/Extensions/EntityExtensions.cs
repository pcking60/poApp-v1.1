using PostOffice.Common.ViewModels;
using PostOffice.Model.Models;
using PostOffice.Web.Models;
using System;

namespace PostOffice.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateApplicationGroup(this ApplicationGroup appGroup, ApplicationGroupViewModel appGroupViewModel)
        {
            appGroup.ID = appGroupViewModel.ID;
            appGroup.Name = appGroupViewModel.Name;
        }

        public static void UpdateApplicationRole(this ApplicationRole appRole, ApplicationRoleViewModel appRoleViewModel, string action = "add")
        {
            if (action == "update")
                appRole.Id = appRoleViewModel.Id;
            else
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            appRole.Description = appRoleViewModel.Description;
        }

        public static void UpdateUser(this ApplicationUser appUser, ApplicationUserViewModel appUserViewModel, string action = "add")
        {
            appUser.Id = appUserViewModel.Id;
            appUser.FullName = appUserViewModel.FullName;
            appUser.BirthDay = appUserViewModel.BirthDay;
            appUser.Email = appUserViewModel.Email;
            appUser.UserName = appUserViewModel.UserName;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
            appUser.POID = appUserViewModel.POID;
            appUser.Status = appUserViewModel.Status;
            appUser.PasswordHash = appUserViewModel.PasswordHash;
            appUser.Address = appUserViewModel.Address;
        }

        public static void UpdateDistrict(this District dis, DistrictViewModel vm) {
            dis.Code = vm.Code;
            dis.CreatedBy = vm.CreatedBy;
            dis.CreatedDate = vm.CreatedDate;
            dis.ID = vm.ID;
            dis.MetaDescription = vm.MetaDescription;
            dis.MetaKeyWord = vm.MetaKeyWord;
            dis.Name = vm.Name;
            dis.Status = vm.Status;
            dis.UpdatedBy = vm.UpdatedBy;
            dis.UpdatedDate = vm.UpdatedDate;
        }

        public static void UpdateTKBD(this TKBDAmount tkbd, TKBDAmountViewModel vm)
        {
            tkbd.Account = vm.Account;
            tkbd.Amount = vm.Amount;
            tkbd.CreatedBy = vm.CreatedBy;
            tkbd.CreatedDate = vm.CreatedDate;
            tkbd.Id = vm.Id;
            tkbd.MetaDescription = vm.MetaDescription;
            tkbd.MetaKeyWord = vm.MetaKeyWord;
            tkbd.Month = vm.Month;
            tkbd.Status = vm.Status;
            tkbd.UpdatedBy = vm.UpdatedBy;
            tkbd.UpdatedDate = vm.UpdatedDate;
            tkbd.Year = vm.Year;
            tkbd.TotalMoney = vm.TotalMoney??0;
        }

        public static void UpdateTKBDHistory(this TKBDHistory tkbd, TKBDHistoryViewModel vm)
        {
            tkbd.Id = vm.Id;
            tkbd.Account = vm.Account;
            tkbd.CustomerId = vm.CustomerId;
            tkbd.CreatedBy = vm.CreatedBy;
            tkbd.CreatedDate = vm.CreatedDate;
            tkbd.Money = vm.Money;
            tkbd.CustomerName = vm.CustomerName;
            tkbd.Rate = vm.Rate;
            tkbd.Status = vm.Status;
            tkbd.TransactionDate = vm.TransactionDate;
            tkbd.UserId = vm.UserId;
            tkbd.ServiceId = vm.ServiceId;
        }

        public static void UpdatePO(this PO po, POViewModel vm)
        {
            po.Code = vm.Code;
            po.CreatedBy = vm.CreatedBy;
            po.CreatedDate = vm.CreatedDate;
            po.DistrictID = vm.DistrictID;
            po.ID = vm.ID;
            po.MetaDescription = vm.MetaDescription;
            po.MetaKeyWord = vm.MetaKeyWord;
            po.Name = vm.Name;
            po.POAddress = vm.POAddress;
            po.POMobile = vm.POMobile;
            po.POStyle = vm.POStyle;
            po.UpdatedBy = vm.UpdatedBy;
            po.UpdatedDate = vm.UpdatedDate;
            po.Status = vm.Status;
          
        }

        public static void UpdateServiceGroup(this ServiceGroup serviceGroup, ServiceGroupViewModel serviceGroupVM)
        {
            serviceGroup.ID = serviceGroupVM.ID;
            serviceGroup.Alias = serviceGroupVM.Alias;
            serviceGroup.CreatedBy = serviceGroupVM.CreatedBy;
            serviceGroup.CreatedDate = serviceGroupVM.CreatedDate;
            serviceGroup.Description = serviceGroupVM.Description;
            serviceGroup.DisplayOrder = serviceGroupVM.DisplayOrder;
            serviceGroup.Image = serviceGroupVM.Image;
            serviceGroup.MetaDescription = serviceGroupVM.MetaDescription;
            serviceGroup.MetaKeyWord = serviceGroupVM.MetaKeyWord;
            serviceGroup.Name = serviceGroupVM.Name;
            serviceGroup.MainServiceGroupId = serviceGroupVM.MainServiceGroupId;

            serviceGroup.Status = serviceGroupVM.Status;
            serviceGroup.UpdatedBy = serviceGroupVM.UpdatedBy;
            serviceGroup.UpdatedDate = serviceGroupVM.UpdatedDate;
        }

        public static void UpdateMainServiceGroup(this MainServiceGroup mainServiceGroup, MainServiceGroupViewModel mainServiceGroupVM)
        {
            mainServiceGroup.Id = mainServiceGroupVM.Id;

            mainServiceGroup.CreatedBy = mainServiceGroupVM.CreatedBy;
            mainServiceGroup.CreatedDate = mainServiceGroupVM.CreatedDate;
            mainServiceGroup.MetaDescription = mainServiceGroupVM.MetaDescription;
            mainServiceGroup.MetaKeyWord = mainServiceGroupVM.MetaKeyWord;
            mainServiceGroup.Name = mainServiceGroupVM.Name;

            mainServiceGroup.Status = mainServiceGroupVM.Status;
            mainServiceGroup.UpdatedBy = mainServiceGroupVM.UpdatedBy;
            mainServiceGroup.UpdatedDate = mainServiceGroupVM.UpdatedDate;
        }

        public static void UpdateService(this PostOffice.Model.Models.Service service, ServiceViewModel serviceVM)
        {
            service.ID = serviceVM.ID;
            service.Alias = serviceVM.Alias;
            service.CreatedBy = serviceVM.CreatedBy;
            service.CreatedDate = serviceVM.CreatedDate;
            service.Description = serviceVM.Description;
            service.BuyIn = serviceVM.BuyIn;
            service.SoldOut = serviceVM.SoldOut;
            service.VAT = serviceVM.VAT;
            service.CreatedBy = serviceVM.CreatedBy;
            service.CreatedDate = serviceVM.CreatedDate;
            service.Description = serviceVM.Description;
            service.GroupID = serviceVM.GroupID;
            service.PayMethodID = serviceVM.PayMethodID;
            service.MetaDescription = serviceVM.MetaDescription;
            service.MetaKeyWord = serviceVM.MetaKeyWord;
            service.Name = serviceVM.Name;
            service.Status = serviceVM.Status;
            service.UpdatedBy = serviceVM.UpdatedBy;
            service.UpdatedDate = serviceVM.UpdatedDate;
            service.Unit = serviceVM.Unit;
        }

        public static void UpdatePropertyService(this PropertyService prop, PropertyServiceViewModel vm)
        {
            prop.CreatedBy = vm.CreatedBy;
            prop.CreatedDate = vm.CreatedDate;
            prop.ID = vm.ID;
            prop.MetaDescription = vm.MetaDescription;
            prop.MetaKeyWord = vm.MetaKeyWord;
            prop.Name = vm.Name;
            prop.Percent = vm.Percent;
            prop.ServiceID = vm.ServiceID;
            prop.UpdatedBy = vm.UpdatedBy;
            prop.Status = vm.Status;
            prop.UpdatedDate = vm.UpdatedDate;
        }

        public static void UpdateTransaction(this Transaction transaction, TransactionViewModel vm)
        {
            transaction.CreatedBy = vm.CreatedBy;
            transaction.CreatedDate = vm.CreatedDate;
            transaction.ID = vm.ID;
            transaction.MetaDescription = vm.MetaDescription;
            transaction.MetaKeyWord = vm.MetaKeyWord;
            transaction.ServiceId = vm.ServiceId;
            transaction.Status = vm.Status;
            transaction.TransactionDate = vm.TransactionDate;
            transaction.UpdatedBy = vm.UpdatedBy;
            transaction.UpdatedDate = vm.UpdatedDate;
            transaction.UserId = vm.UserId;
            //transaction.TransactionDetails = vm.TransactionDetails;
            transaction.Quantity = vm.Quantity;
            transaction.IsCash = vm.IsCash;
            transaction.IsCurrency = vm.IsCurrency;
        }

        public static void UpdateMainGroup1(MainGroup1 RP, TransactionViewModel vm)
        {
            RP.STT = vm.ID;
            RP.ServiceName = vm.ServiceName;
            RP.Quantity = vm.Quantity;
            RP.TotalCash = vm.TotalCash;
            RP.VatOfTotalCash = vm.VatOfTotalCash;
            RP.TotalDebt = vm.TotalDebt;
            RP.VatOfTotalDebt = vm.VatOfTotalDebt;
        }

        public static void UpdateTransactionDetail(this TransactionDetail transactionDetail, TransactionDetailViewModel vm)
        {
            transactionDetail.CreatedBy = vm.CreatedBy;
            transactionDetail.CreatedDate = vm.CreatedDate;
            transactionDetail.ID = vm.ID;
            transactionDetail.MetaDescription = vm.MetaDescription;
            transactionDetail.MetaKeyWord = vm.MetaKeyWord;
            transactionDetail.Money = vm.Money;
            transactionDetail.PropertyServiceId = vm.PropertyServiceId;
            transactionDetail.Status = vm.Status;
            transactionDetail.TransactionId = vm.TransactionID;
            transactionDetail.UpdatedBy = vm.UpdatedBy;
            transactionDetail.UpdatedDate = vm.UpdatedDate;
        }

        public static void UpdatePaymentMethod(this PaymentMethod payment, PaymentMethodViewModel paymentVM)
        {
            payment.CreatedBy = paymentVM.CreatedBy;
            payment.CreatedDate = paymentVM.CreatedDate;
            payment.Description = paymentVM.Description;
            payment.ID = paymentVM.ID;
            payment.MetaDescription = paymentVM.MetaDescription;
            payment.MetaKeyWord = paymentVM.MetaKeyWord;
            payment.Name = paymentVM.Name;
            payment.Status = paymentVM.Status;
            payment.UpdatedBy = paymentVM.UpdatedBy;
            payment.UpdatedDate = paymentVM.UpdatedDate;
            payment.Value = paymentVM.Value;
        }

    }
}