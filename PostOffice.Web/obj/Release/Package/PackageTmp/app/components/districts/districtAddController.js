'use strict';
angular.module('postoffice.districts')

    .controller('districtAddController', 
        ['$scope', 'apiService', 'notificationService', '$state', 'commonService', 
            function($scope, apiService, notificationService, $state, commonService){
                $scope.district = {
                    Status: true
                }
                //get alias & toTitleCase each word
                $scope.changeSomething = changeSomething;
                function changeSomething() {
                    $scope.district.Name = commonService.toTitleCase($scope.district.Name);
                }

                $scope.AddDistrict = AddDistrict;
                function AddDistrict() {
                    apiService.post('/api/district/create', $scope.district,
                        function (result) {
                            notificationService.displaySuccess(result.data.Name + ' Thêm mới thành công!');
                            $state.go('districts');

                        }, function (error) {
                            notificationService.displayError('Thêm mới thất bại');
                        });
                }
    }]);
   