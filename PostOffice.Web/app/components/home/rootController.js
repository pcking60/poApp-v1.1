angular.module('postoffice')

    .controller('rootController', ['$state', '$scope', 'authService', 'apiService', '$rootScope', 'notificationService', '$http', '$base64',

        function ($state, $scope, authService, apiService, $rootScope, notificationService, $http, $base64) {

            $scope.logOut = function () {
                authService.logOut();
                $state.go('login');
            }
            $scope.authentication = authService.authentication;
            var userName = $scope.authentication.userName;

            if ($scope.authentication.isAuth) {
                $scope.isAdmin = authService.haveRole('Administrator');
                $scope.isManager = authService.haveRole('Manager');
                $scope.isCounter = authService.haveRole('Counter');

                getUserInfo();
                getAllServices();
            }

            function getUserInfo() {

                apiService.get('/api/applicationUser/getuserinfo/' + $base64.encode(userName), null,
                    function (result) {
                        $scope.userInfo = result.data;
                    },
                    function (error) {
                        //notificationService.displayError(error.data);
                    });
            }

            $scope.sideBarBaseView = 'app/shared/views/sideBarBaseView.html';

            function getAllServices() {
                apiService.get('/api/service/getallparents', null, function (result) {
                    $rootScope.Services = result.data;
                }, function (error) {
                    notificationService.displayError(error.data);
                });
            }

            //authenticationService.validateRequest();
        }
    ]);