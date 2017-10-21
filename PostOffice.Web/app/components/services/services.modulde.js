/// <reference path="~/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('postoffice.services', ['postoffice.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('services', {
                url: "/danh-sach-dich-vu.html",
                templateUrl: "/app/components/services/serviceListView.html",
                parent: 'base',
                controller: "serviceListController"
            }).state('service_add', {
                url: "/them-moi-dich-vu.html",
                templateUrl: "/app/components/services/serviceAddView.html",
                parent: 'base',
                controller: "serviceAddController"
            }).state('service_edit', {
                url: "/cap-nhat-thong-tin-dich-vu/:id.html",
                templateUrl: "/app/components/services/serviceEditView.html",
                parent: 'base',
                controller: "serviceEditController"
            });
    }
})();