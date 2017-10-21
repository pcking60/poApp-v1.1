(function (app) {
    app.controller('serviceEditController', serviceEditController);
    serviceEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams', 'commonService'];
    function serviceEditController($scope, apiService, notificationService, $state, $stateParams, commonService) {
        $scope.service = {     
        }
        $scope.EditService = EditService;

        $scope.getSeoTitle = getSeoTitle;
        function getSeoTitle() {
            $scope.service.Alias = commonService.getSeoTitle($scope.service.Name);
        }
        function loadServiceDetail() {
            apiService.get('/api/service/getbyid/' + $stateParams.id, null, function (result) {
                $scope.service = result.data
            }, function (error) {
                notificationService.displayError(error.data)
            });
        }
        function EditService() {
            apiService.put('/api/service/edit', $scope.service,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' được cập nhật thành công');
                    $state.go('services');
                }, function (error) {
                    notificationService.displayError('Cập nhật thất bại');
                });
        }
        function loadparentGroup() {
            apiService.get('/api/servicegroup/getallparents', null, function (result) {
                $scope.parentGroups = result.data;
            }, function () {
                console.log('Can not load service group!');
            });
        }
        function loadPaymentMethod() {
            apiService.get('/api/payment/getallname', null, function (result) {
                $scope.paymentMethods = result.data;
            }, function () {
                console.log('Can not load payment method !');
            });
        }
        loadparentGroup();
        loadPaymentMethod();
        loadServiceDetail();
    }
})(angular.module('postoffice.services'));