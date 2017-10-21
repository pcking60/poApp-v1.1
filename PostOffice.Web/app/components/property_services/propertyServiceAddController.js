'use strict';
angular.module('postoffice.property_services')
    .controller('propertyServiceAddController', 
        ['$scope', 'apiService', 'notificationService', '$state', 'commonService', '$rootScope',
            function ($scope, apiService, notificationService, $state, commonService, $rootScope)
            {
                $scope.propertyService = {
                    Status: true
                }
                //get alias & toTitleCase each word
                $scope.changeSomething = changeSomething;
                function changeSomething() {
                    //$scope.district.Name = commonService.toTitleCase($scope.district.Name);
                }

                $scope.AddPropertyService = AddPropertyService;
                function AddPropertyService() {
                    apiService.post('/api/property_services/create', $scope.propertyService,
                        function (result) {
                            notificationService.displaySuccess(result.data.Name + ' Thêm mới thành công!');
                            $state.go('property_services');

                        }, function (error) {
                            notificationService.displayError('Thêm mới thất bại');
                        });
                }

                function loadServices() {
                    apiService.get('/api/service/getallparents', null, function (result) {
                        $scope.listServices = result.data;
                    }, function () {
                        console.log('Can not load services!');
                    });
                }
                loadServices();
            }
        ]);
   