/// <reference path="/Assets/admin/libs/angular/angular.js" />
'use strict';
angular.module('postoffice.application_roles', ['postoffice.common'])
        .config(['$stateProvider', '$urlRouterProvider', function($stateProvider, $urlRouterProvider){
            $stateProvider.state('application_roles', {
                url: "/danh-sach-quyen-truy-cap.html",
                templateUrl: "/app/components/application_roles/applicationRoleListView.html",
                parent: 'base',
                controller: "applicationRoleListController"
            })
               .state('add_application_role', {
                   url: "/them-quyen-nguoi-dung.html",
                   parent: 'base',
                   templateUrl: "/app/components/application_roles/applicationRoleAddView.html",
                   controller: "applicationRoleAddController"
               })
               .state('edit_application_role', {
                   url: "/thay-doi-quyen-nguoi-dung/:id.html",
                   templateUrl: "/app/components/application_roles/applicationRoleEditView.html",
                   controller: "applicationRoleEditController",
                   parent: 'base',
               });
        }]);
