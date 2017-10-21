(function (app) {
    'use strict';

    app.controller('applicationUserEditController', applicationUserEditController);

    applicationUserEditController.$inject = ['$scope', 'apiService', 'notificationService', '$location', '$stateParams', '$filter'];

    function applicationUserEditController($scope, apiService, notificationService, $location, $stateParams, $filter) {

        $scope.account = {
        };

        $scope.updateAccount = updateAccount;

        function updateAccount() {
            apiService.put('/api/applicationUser/update', $scope.account, addSuccessed, addFailed);
        }
        function loadDetail() {
            apiService.get('/api/applicationUser/detail/' + $stateParams.id, null,
            function (result) {
                $scope.account = result.data;
                $scope.account.BirthDay = new Date($scope.account.BirthDay);
                //$scope.account.BirthDay = $filter('date')($scope.account.BirthDay, "dd/MM/yyyy");
                console.log($scope.account.BirthDay);
            },
            function (result) {
                notificationService.displayError(result.data);
            });
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.account.FullName + ' đã được cập nhật thành công.');

            $location.url('application_users');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }
        function loadGroups() {
            apiService.get('/api/applicationGroup/getlistall',
                null,
                function (response) {
                    $scope.groups = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách nhóm.');
                });

        }
        function loadPOs() {
            apiService.get('/api/po/getallparents', null, function (result) {
                $scope.pos = result.data;
            }, function () {
                console.log('Can not load list POs!');
            });
        }

        loadPOs();
        loadGroups();
        loadDetail();

       


    }
})(angular.module('postoffice.application_users'));