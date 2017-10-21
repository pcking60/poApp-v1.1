(function (app) {
    app.controller('loginController', ['$scope', 'authService', '$injector', 'notificationService',
        function ($scope, authService, $injector, notificationService) {

            $scope.loginData = {
                userName: "",
                password: ""
            };
           
            $scope.loginSubmit = function () {
                authService.login($scope.loginData).then(function (response) {
                    if (response != null && response.error != undefined) {
                        notificationService.displayError("Đăng nhập không đúng.");                        
                    }
                    else {
                        var stateService = $injector.get('$state');  
                        if ($scope.loginData.userName == 'admin') {
                            stateService.go('home');
                        }
                        else {
                            stateService.go('transactions');
                        }                        
                    }
                });
            }
        }
    ]);
})(angular.module('postoffice'));