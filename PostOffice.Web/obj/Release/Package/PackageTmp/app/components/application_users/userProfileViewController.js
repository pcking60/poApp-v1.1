(function (app) {
    'use strict';

    app.controller('userProfileViewController', userProfileViewController);

    userProfileViewController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox'];

    function userProfileViewController($scope, apiService, notificationService, $ngBootbox) {
        var vm = this;
        vm.tab = 1;
        vm.selectTab = function (setTab) {
            vm.tab = setTab;
        };
        vm.isTab = function (checkTab) {
            return vm.tab === checkTab;
        };

        $scope.info = {};
        $scope.updateInfo = updateInfo;
        $scope.loadUserInfo = loadUserInfo;
        function updateInfo() {
            apiService.put("/api/applicationUser/updateInfo", $scope.info, updateSuccess, updateFailed);
        }

        function loadUserInfo() {
            apiService.get("/api/applicationUser/userinfo", null,
                function (result) {
                    $scope.info = result.data;
                }
                , function (result) {
                    notificationService.displayError(result.data);
                })
        };
        
        function updateSuccess() {
            notificationService.displaySuccess("Cập nhật thông tin người dùng thành công");
        };
        function updateFailed() {
            notificationService.displayError("Có lỗi xảy ra, cập nhật thông tin thất bại");
        }

        loadUserInfo();
    }
})(angular.module('postoffice.application_users'));