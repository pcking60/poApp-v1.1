/// <reference path="/Assets/admin/libs/angular/angular.js" />
'use strict';
angular.module('postoffice.districts', ['postoffice.common'])
    .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) { 
    
        $stateProvider
            .state('districts', {
                url: "/danh-sach-huyen.html",
                templateUrl: "/app/components/districts/districtListView.html",
                parent: 'base',
                controller: "districtListController"
            })
            .state('add_district', {
                url: "/them-moi-huyen.html",
                parent: 'base',
                templateUrl: "/app/components/districts/districtAddView.html",
                controller: "districtAddController"
            })
            .state('edit_district', {
                url: "/thay-doi-thong-tin-huyen/:id.html",
                templateUrl: "/app/components/districts/districtEditView.html",
                controller: "districtEditController",
                parent: 'base',
            });
    }])

 

