'use strict';
angular.module('postoffice.property_services')
    .controller('propertyServiceEditController', 
        ['$scope', 'apiService', 'notificationService', '$state', '$stateParams', 'commonService', 
            function($scope, apiService, notificationService, $state, $stateParams, commonService) {
                $scope.propertyService = {
                    Status: true,
                }
                $scope.EditPropertyService = EditPropertyService;
                //get alias & toTitleCase each word
                $scope.changeSomething = changeSomething;
                function changeSomething() {;
                    //$scope.district.Name = commonService.toTitleCase($scope.district.Name);
                }

                function loadPropertyServiceDetail() {
                    apiService.get('/api/property_services/getbyid/' + $stateParams.id, null, function (result) {
                        $scope.propertyService = result.data
                    }, function (error) {
                        notificationService.displayError(error.data)
                    });
                }
                function EditPropertyService() {
                    apiService.put('/api/property_services/update', $scope.propertyService,
                        function (result) {
                            notificationService.displaySuccess('Cập nhật thành công');
                            $state.go('property_services');
                        }, function (error) {
                            notificationService.displayError('cập nhật thất bại');
                        });
                }

                function loadListServices() {
                    apiService.get('/api/service/getallparents', null, function (result) {
                        $scope.listServices = result.data
                    }, function (error) {
                        notificationService.displayError(error.data)
                    });
                }
                loadListServices();
                loadPropertyServiceDetail();
    }]);
   