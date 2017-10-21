/// <reference path="/Assets/admin/libs/angular/angular.js" />

angular.module('postoffice',
            [
                'postoffice.application_groups',
                'postoffice.application_roles',
                'postoffice.application_users',
                'postoffice.districts',
                'postoffice.services',
                'postoffice.service_groups',
                'postoffice.main_service_groups',
                'postoffice.pos',
                'postoffice.tkbd',
               'postoffice.property_services',
               'postoffice.transactions',
               'postoffice.statistics',
                'angular-loading-bar',
                'LocalStorageModule',                
                'ngCookies',
                'ngBootstrap',
                'postoffice.common'
            ])

    .config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
        //$locationProvider.html5Mode(true);
        $stateProvider
            .state('base', {
                url: '',
                templateUrl: '/app/shared/views/baseView.html',
                abstract: true
            })
            .state('userbase', {
                url: '',
                templateUrl: '/app/shared/views/userBaseView.html',
                abstract: true
            })
            .state('login', {
                url: "/dang-nhap.html",
                templateUrl: "/app/components/login/loginView.min.html",
                controller: "loginController"
            })
            .state('user_dashboard', {
                url: "/user_dashboard",
                parent: 'base',
                templateUrl: "/app/components/user_dashboard/userDashboardView.html",
                controller: "userDashboardController"
            })
            .state('home', {
                url: "/trang-chinh.html",
                parent: 'base',
                templateUrl: "/app/components/home/homeView.html",
                controller: "homeController"
            });
        $urlRouterProvider.otherwise('/dang-nhap.html');
    })

    .config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
    })

    .run(['authService', function (authService) {
        authService.fillAuthData();
    }])

    .config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeBar = true;
        cfpLoadingBarProvider.includeSpinner = true;
    }]);

  

