(function (app) {
    app.controller('changePasswordController', changePasswordController);
    changePasswordController.$inject = ['$scope', 'apiService', 'notificationService', '$state', 'commonService', '$stateParams', '$ngBootbox', '$timeout', '$location'];
    function changePasswordController($scope, apiService, notificationService, $state, commonService, $stateParams, $ngBootbox, $timeout, $location) {
       
        $scope.user = {
            oldPassword: '',
            newPassword: ''
        };

        function loadDetail() {
            apiService.get('/api/applicationUser/detail/' + $stateParams.id, null,
            function (result) {
                $scope.user = result.data;                
            },
            function (result) {
                notificationService.displayError(result.data);
            });
        }

        $scope.ChangePassword = function ChangePassword() {
            apiService.put('/api/applicationUser/ChangePassword', $scope.user, addSuccessed, addFailed
            );
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.user.FullName + ' đã được cập nhật thành công.');
            $location.url('transactions');
        }

        function addFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        loadDetail();
    }
})(angular.module('postoffice.application_users'));