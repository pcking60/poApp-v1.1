(function (app) {  
    app.controller('mainServiceGroupAddController', mainServiceGroupAddController);
    mainServiceGroupAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state', 'commonService'];
    function mainServiceGroupAddController($scope, apiService, notificationService, $state, commonService) {
        $scope.mainServiceGroup = {
            Status: true
        }
        $scope.changeSomething = changeSomething;
        function changeSomething() {            
            $scope.mainServiceGroup.Name = commonService.toTitleCase($scope.mainServiceGroup.Name);
        }
        $scope.AddMainServiceGroup = AddMainServiceGroup;
        function AddMainServiceGroup() {
            apiService.post('/api/mainservicegroup/create', $scope.mainServiceGroup,
                function (result) {
                    notificationService.displaySuccess('Thêm mới thành công!');
                    $state.go('main_service_groups');
                }, function (error) {
                    notificationService.displayError('Thêm mới thất bại');
                });
        }
    }
})(angular.module('postoffice.main_service_groups'));