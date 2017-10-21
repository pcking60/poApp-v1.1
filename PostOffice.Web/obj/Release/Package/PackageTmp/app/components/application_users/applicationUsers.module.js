/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('postoffice.application_users', ['postoffice.common', 'ngMaterial', 'ngMessages']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('application_users', {
            url: "/danh-sach-nguoi-dung.html",
            templateUrl: "/app/components/application_users/applicationUserListView.html",
            parent: 'base',
            controller: "applicationUserListController"
        })
        .state('add_application_user', {
            url: "/them-moi-nhan-vien.html",
            parent: 'base',
            templateUrl: "/app/components/application_users/applicationUserAddView.html",
            controller: "applicationUserAddController"
        })
        .state('user_profile', {
            url: "/thong-tin-ca-nhan.html",
            parent: 'base',
            templateUrl: "/app/components/application_users/userProfileView.html",
            controller: "userProfileViewController"
        })
        .state('changePassword', {
            url: "/thay-doi-mat-khau.html",
            parent: 'base',
            templateUrl: "/app/components/application_users/changePassword.html",
            controller: "changePasswordController"
        })
        .state('edit_application_user', {
            url: "/cap-nhat-nguoi-dung/:id.html",
            templateUrl: "/app/components/application_users/applicationUserEditView.html",
            controller: "applicationUserEditController",
            parent: 'base'
        });
    }
})();