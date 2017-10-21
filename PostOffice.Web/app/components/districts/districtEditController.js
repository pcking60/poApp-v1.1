'use strict';
angular.module('postoffice.districts')
    .controller('districtEditController', 
        ['$scope', 'apiService', 'notificationService', '$state', '$stateParams', 'commonService', 
            function($scope, apiService, notificationService, $state, $stateParams, commonService) {
                $scope.district = {
                    Status: true,
                }
                $scope.EditDistrict = EditDistrict;
                //get alias & toTitleCase each word
                $scope.changeSomething = changeSomething;
                function changeSomething() {;
                    $scope.district.Name = commonService.toTitleCase($scope.district.Name);
                }

                function loadDistrictDetail() {
                    apiService.get('/api/district/getbyid/' + $stateParams.id, null, function (result) {
                        $scope.district = result.data
                    }, function (error) {
                        notificationService.displayError(error.data)
                    });
                }
                function EditDistrict() {
                    apiService.put('/api/district/update', $scope.district,
                        function (result) {
                            notificationService.displaySuccess('Cập nhật thành công');
                            $state.go('districts');
                        }, function (error) {
                            notificationService.displayError('cập nhật thất bại');
                        });
                }

                loadDistrictDetail();
    }]);
   