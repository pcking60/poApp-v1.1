(function (app) {
    app.controller('mainServiceGroupEditController', mainServiceGroupEditController);
    mainServiceGroupEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams', 'commonService'];
    function mainServiceGroupEditController($scope, apiService, notificationService, $state, $stateParams, commonService) {
        $scope.mainServiceGroup = {
            Status: true
        }
        $scope.EditMainServiceGroup = EditMainServiceGroup;
        $scope.changeSomething = changeSomething;
        function changeSomething()
        {
            $scope.mainServiceGroup.Name = commonService.toTitleCase($scope.mainServiceGroup.Name);
        }

        function loadMainServiceGroupDetail() {
            apiService.get('/api/mainservicegroup/getbyid/' + $stateParams.id, null, function (result) {
                $scope.mainServiceGroup = result.data
            }, function (error) {
                notificationService.displayError(error.data)
            });
        }
        function EditMainServiceGroup() {
            apiService.put('/api/mainservicegroup/update', $scope.mainServiceGroup,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' cập nhật thành công');
                    $state.go('main_service_groups');
                }, function (error) {
                    notificationService.displayError('cập nhật thất bại');
                });
        }       
        loadMainServiceGroupDetail();
    }
})(angular.module('postoffice.main_service_groups'));