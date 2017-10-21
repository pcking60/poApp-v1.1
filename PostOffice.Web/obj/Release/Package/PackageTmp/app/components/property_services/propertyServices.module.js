/// <reference path="/Assets/admin/libs/angular/angular.js" />
'use strict';
angular.module('postoffice.property_services', ['postoffice.common'])
    .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) { 
    
        $stateProvider
            .state('property_services', {
                url: "/danh-sach-thuoc-tinh.html",
                templateUrl: "/app/components/property_services/propertyServicesListView.html",
                parent: 'base',
                controller: "propertyServicesListController"
            })
            .state('add_property_service', {
                url: "/them-moi-thuoc-tinh.html",
                parent: 'base',
                templateUrl: "/app/components/property_services/propertyServiceAddView.html",
                controller: "propertyServiceAddController"
            })
            .state('edit_property_service', {
                url: "/cap-nhat-thong-tin-thuoc-tinh/:id.html",
                templateUrl: "/app/components/property_services/propertyServiceEditView.html",
                controller: "propertyServiceEditController",
                parent: 'base',
            });
    }])

 

