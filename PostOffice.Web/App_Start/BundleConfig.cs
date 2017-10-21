using System.Web.Optimization;

namespace PostOffice.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Assets/admin/libs/toastr/toastr.css",
                "~/Assets/admin/libs/angular-loading-bar/build/loading-bar.css",
                "~/Assets/admin/css/custom.css",
                "~/Assets/admin/libs/iCheck/all.css",
                "~/Assets/admin/libs/ngprogress/ngProgress.css",
                "~/Assets/admin/libs/angular-ui-select/dist/select.css",
                "~/Assets/admin/libs/bootstrap-daterangepicker/daterangepicker.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Assets/admin/libs/jquery/dist/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                "~/Assets/admin/libs/angular/angular.js"));           

            bundles.Add(new ScriptBundle("~/bundles/ckfinder").Include(                
                "~/Assets/admin/libs/ckfinder/ckfinder.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Assets/admin/libs/bootstrap/dist/js/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(                
                "~/Assets/admin/libs/slimScroll/jquery.slimscroll.js",
                "~/Assets/admin/libs/fastclick/lib/fastclick.js",
                "~/Assets/admin/libs/ngprogress/build/ngprogress.js",
                "~/Assets/admin/libs/toastr/toastr.js",
                "~/Assets/admin/libs/bootbox/bootbox.js", 
                "~/Assets/admin/libs/ng-ckeditor/ng-ckeditor.js",
                "~/Assets/admin/libs/angular-local-storage/dist/angular-local-storage.js",
                "~/Assets/admin/libs/angular-loading-bar/build/loading-bar.js",
                "~/Assets/admin/libs/checklist-model/checklist-model.js",
                "~/Assets/admin/libs/iCheck/icheck.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/action").Include(
                    "~/app/shared/modules/postoffice.common.js",
                    "~/app/shared/services/authService.js",
                    "~/app/shared/services/authInterceptorService.js",
                    "~/app/shared/filters/statusFilter.js",
                    "~/app/shared/services/apiService.js",
                    "~/app/shared/directives/pagerDirective.js",
                    "~/app/shared/directives/asDateDirective.js",
                    "~/app/shared/services/notificationService.js",
                    "~/app/shared/directives/fileUpload.directive.js",
                    "~/app/shared/services/commonService.js",
                    "~/app/components/application_groups/applicationGroups.module.js",
                    "~/app/components/application_roles/applicationRoles.module.js",
                    "~/app/components/application_users/applicationUsers.module.js",
                    "~/app/components/districts/districts.module.js",
                    "~/app/components/pos/pos.module.js",
                    "~/app/components/service_groups/serviceGroups.module.js",
                    "~/app/components/services/services.modulde.js",
                    "~/app/components/main_service_groups/mainServiceGroups.module.js",
                    "~/app/components/property_services/propertyServices.module.js",
                    "~/app/components/transactions/transactions.module.js",
                    "~/app/components/statistic/statistic.module.js",
                    "~/app/app.js",
                    "~/app/components/home/rootController.js",
                    "~/app/components/home/homeController.js",
                    "~/app/components/login/loginController.js",
                    "~/app/components/application_groups/applicationGroupListController.js",
                    "~/app/components/application_groups/applicationGroupEditController.js",
                    "~/app/components/application_groups/applicationGroupAddController.js",
                    "~/app/components/application_roles/applicationRoleAddController.js",
                    "~/app/components/application_roles/applicationRoleEditController.js",
                    "~/app/components/application_roles/applicationRoleListController.js",
                    "~/app/components/application_users/applicationUserAddController.js",
                    "~/app/components/application_users/applicationUserEditController.js",
                    "~/app/components/application_users/applicationUserListController.js",
                    "~/app/components/application_users/userProfileViewController.js",
                    "~/app/components/districts/districtListController.js",
                    "~/app/components/districts/districtAddController.js",
                    "~/app/components/districts/districtEditController.js",
                    "~/app/components/pos/poController.js",
                    "~/app/components/pos/poAddController.js",
                    "~/app/components/pos/poEditController.js",
                    "~/app/components/services/serviceAddController.js",
                    "~/app/components/services/serviceEditController.js",
                    "~/app/components/services/serviceListController.js",
                    "~/app/components/service_groups/serviceGroupsListController.js",
                    "~/app/components/service_groups/serviceGroupAddController.js",
                    "~/app/components/service_groups/serviceGroupEditController.js",
                    "~/app/components/user_dashboard/userDashboardController.js",
                    "~/app/components/main_service_groups/mainServiceGroupAddController.js",
                    "~/app/components/main_service_groups/mainServiceGroupEditController.js",
                    "~/app/components/main_service_groups/mainServiceGroupsListController.js",
                    "~/app/components/property_services/propertyServicesListController.js",
                    "~/app/components/property_services/propertyServiceEditController.js",
                    "~/app/components/property_services/propertyServiceAddController.js",
                    "~/app/components/transactions/transactionsListController.js",
                    "~/app/components/transactions/transactionEditController.js",
                    "~/app/components/transactions/transactionAddController.js",
                    "~/app/components/statistic/revenueStatisticController.js",
                    "~/app/components/statistic/unitStatisticController.js",
                    "~/app/components/statistic/timeStatisticController.js",
                    "~/app/components/statistic/reportsController.js",
                    "~/app/components/tkbd/tkbd.module.js",
                    "~/app/components/tkbd/tkbdListController.js",
                    "~/app/components/tkbd/tkbdImportListController.js",
                    "~/app/components/tkbd/TKBDHistoryController.js",
                    "~/Assets/admin/js/app.js",
                    "~/Assets/admin/js/demo.js"
                ));
            BundleTable.EnableOptimizations = true;
        }
    }
}