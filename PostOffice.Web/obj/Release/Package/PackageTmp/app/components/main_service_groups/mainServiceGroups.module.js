/// <reference path="~/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('postoffice.main_service_groups', ['postoffice.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $urlRouterProvider.otherwise('/main_service_groups');

        $stateProvider
            
            .state('main_service_groups', {
                url: "/danh-sach-nhom-chinh.html",
                parent: 'base',
                templateUrl: "/app/components/main_service_groups/mainServiceGroupsListView.html",
                controller: "mainServiceGroupsListController"
            }).state('add_main_service_groups', {
                url: "/them-nhom-chinh.html",
                parent: 'base',
                templateUrl: "/app/components/main_service_groups/mainServiceGroupAddView.html",
                controller:"mainServiceGroupAddController"
            }).state('edit_main_service_groups', {
                url: "/thay-doi-thong-tin-nhom-chinh/:id.html",
                parent: 'base',
                templateUrl: "/app/components/main_service_groups/mainServiceGroupEditView.html",
                controller: "mainServiceGroupEditController"
            });
    };
})();