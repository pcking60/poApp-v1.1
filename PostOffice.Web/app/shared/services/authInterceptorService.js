'use strict';
angular.module('postoffice.common')
    .service('authInterceptorService', ['$q', '$location', 'localStorageService', 'notificationService', function ($q, $location, localStorageService, notificationService) {

        var authInterceptorServiceFactory = {};

        var _request = function (config) {

            config.headers = config.headers || {};

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }

            return config;
        }

        var _responseError = function (rejection) {
            if (rejection.status === 401) {
                $location.path('/login');
                notificationService.displayError('Yêu cầu đăng nhập');
            }
            return $q.reject(rejection);
        }

        authInterceptorServiceFactory.request = _request;
        authInterceptorServiceFactory.responseError = _responseError;

        return authInterceptorServiceFactory;
}]);