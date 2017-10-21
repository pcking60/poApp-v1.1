(function (app) {
    app.controller('serviceGroupEditController', serviceGroupEditController);
    serviceGroupEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams', 'commonService'];
    function serviceGroupEditController($scope, apiService, notificationService, $state, $stateParams, commonService) {
        $scope.serviceGroup = {
            Status: true
        }
        $scope.EditServiceGroup = EditServiceGroup;
        $scope.changeSomething = changeSomething;
        function changeSomething()
        {
            $scope.serviceGroup.Alias = commonService.getSeoTitle($scope.serviceGroup.Name);
            //$scope.serviceGroup.Name = commonService.toTitleCase($scope.serviceGroup.Name);
        }

        function loadServiceGroupDetail() {
            apiService.get('/api/servicegroup/getbyid/' + $stateParams.id, null, function (result) {
                $scope.serviceGroup = result.data
            }, function (error) {
                notificationService.displayError(error.data)
            });
        }
        function EditServiceGroup() {
            apiService.put('/api/servicegroup/update', $scope.serviceGroup,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' cập nhật thành công');
                    $state.go('service_groups');
                }, function (error) {
                    notificationService.displayError('cập nhật thất bại');
                });
        }
        function loadparentGroup() {
            apiService.get('/api/mainservicegroup/getallparents', null, function (result) {
                $scope.parentGroups = result.data;
            }, function () {
                console.log('Can not load Parent Group');
            });
        }
        loadparentGroup();
        loadServiceGroupDetail();
    }
})(angular.module('postoffice.service_groups'));