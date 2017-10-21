'use strict';
angular.module('postoffice.pos')
    .controller('poEditController', ['$scope', 'apiService', 'notificationService', '$state', '$stateParams',
        function poEditController($scope, apiService, notificationService, $state, $stateParams) {
            $scope.po = {
                Status: true,
            }
            $scope.EditPO = EditPO;

            function loadPODetail() {
                apiService.get('/api/po/getbyid/' + $stateParams.id, null, function (result) {
                    $scope.po = result.data
                }, function (error) {
                    notificationService.displayError(error.data)
                });
            }
            function EditPO() {
                apiService.put('/api/po/update', $scope.po,
                    function (result) {
                        notificationService.displaySuccess('Cập nhật thành công');
                        $state.go('pos');
                    }, function (error) {
                        notificationService.displayError('Cập nhật thất bại');
                    });
            }

            function loaddistricts() {
                apiService.get('/api/district/getallparents', null, function (result) {
                    $scope.districts = result.data;
                }, function () {
                    console.log('Can not load district list!');
                });
            }
             
            loadPODetail();
            loaddistricts();
        }
    ])

    
