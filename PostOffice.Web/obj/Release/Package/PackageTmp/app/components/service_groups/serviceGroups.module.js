/// <reference path="~/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('postoffice.service_groups', ['postoffice.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $urlRouterProvider.otherwise('/service_groups');

        $stateProvider
            
            .state('service_groups', {
                url: "/danh-sach-nhom-dich-vu.html",
                parent: 'base',
                templateUrl: "/app/components/service_groups/serviceGroupsListView.html",
                controller: "serviceGroupsListController"
            }).state('add_service_groups', {
                url: "/them-moi-nhom-dich-vu.html",
                parent: 'base',
                templateUrl: "/app/components/service_groups/serviceGroupAddView.html",
                controller:"serviceGroupAddController"
            }).state('edit_service_groups', {
                url: "/cap-nhat-thong-tin-nhom-dich-vu/:id.html",
                parent: 'base',
                templateUrl: "/app/components/service_groups/serviceGroupEditView.html",
                controller: "serviceGroupEditController"
            });
    };
})();