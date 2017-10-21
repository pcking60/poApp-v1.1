(function (app) {
    'use strict';

    app.controller('applicationUserAddController', applicationUserAddController);

    applicationUserAddController.$inject = ['$scope', 'apiService', 'notificationService', '$location', 'commonService'];

    function applicationUserAddController($scope, apiService, notificationService, $location, commonService) {
        $scope.account = {
            Groups: [],
            BirthDay: new Date()
        }
        //$scope.test = {};
        //$scope.test.BirthDay = { startDate: null, endDate: null };        
        
        $scope.addAccount = addAccount;

        function addAccount() {
            apiService.post('/api/applicationUser/add', $scope.account, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.account.UserName + ' đã được thêm mới.');
            console.log($scope.test.BirthDay.startDate.format('YYYY-MM-DD'));
            $location.url('application_users');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            //notificationService.displayErrorValidation(response);
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

    }
})(angular.module('postoffice.application_users'));