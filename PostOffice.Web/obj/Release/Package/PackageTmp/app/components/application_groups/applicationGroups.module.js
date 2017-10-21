/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('postoffice.application_groups', ['postoffice.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('application_groups', {
            url: "/danh-sach-nhom-nguoi-dung.html",
            templateUrl: "/app/components/application_groups/applicationGroupListView.html",
            parent: 'base',
            controller: "applicationGroupListController"
        })
            .state('add_application_group', {
                url: "/them-moi-nhom-nguoi-dung.html",
                parent: 'base',
                templateUrl: "/app/components/application_groups/applicationGroupAddView.html",
                controller: "applicationGroupAddController"
            })
            .state('edit_application_group', {
                url: "/thay-doi-nhom-nguoi-dung/:id.html",
                templateUrl: "/app/components/application_groups/applicationGroupEditView.html",
                controller: "applicationGroupEditController",
                parent: 'base',
            });
    }
})();