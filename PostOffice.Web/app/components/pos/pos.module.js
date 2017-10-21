/// <reference path="~/Assets/admin/libs/angular/angular.js" />

angular.module('postoffice.pos', ['postoffice.common'])

    .config(['$stateProvider', '$urlRouterProvider',function( $stateProvider, $urlRouterProvider) {

        $urlRouterProvider.otherwise('/pos');

        $stateProvider
            .state('pos', {
                url: "/danh-sach-buu-cuc.html",
                templateUrl: "/app/components/pos/poView.html",
                parent: 'base',
                controller: "poController"
            }).state('add_po', {
                url: "/them-moi-buu-cuc.html",
                parent: 'base',
                templateUrl: "/app/components/pos/poAddView.html",
                controller: "poAddController"
            }).state('edit_po', {
                url: "/cap-nhat-buu-cuc/:id.html",
                templateUrl: "/app/components/pos/poEditView.html",
                parent: 'base',
                controller: "poEditController"
            });
    }])
