(function (app) {
    app.controller('serviceAddController', serviceAddController);
    serviceAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state', 'commonService', '$rootScope'];
    function serviceAddController($scope, apiService, notificationService, $state, commonService, $rootScope) {
        $scope.service = {
            CreatedDate: new Date(),
            Status: true
        }
        $scope.getSeoTitle = getSeoTitle;
        function getSeoTitle() {
            $scope.service.Alias = commonService.getSeoTitle($scope.service.Name);
        }
        $scope.AddService = AddService;
        function AddService() {
            apiService.post('/api/service/add', $scope.service,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' được thêm thành công');
                    $state.go('services');
                }, function (error) {
                    notificationService.displayError('Thêm mới thất bại');
                });
        }
        

        function loadparentGroup() {
            apiService.get('/api/servicegroup/getallparents', null, function (result) {
                $scope.parentGroups = result.data;
            }, function () {
                console.log('Can not load service group!');
            });
        }
        //function loadPaymentMethod() {
        //    apiService.get('/api/payment/getallname', null, function (result) {
        //        $scope.paymentMethods = result.data;
        //    }, function () {
        //        console.log('Can not load payment method !');
        //    });
        //}
        loadparentGroup();
        //loadPaymentMethod();
    }
})(angular.module('postoffice.services'));