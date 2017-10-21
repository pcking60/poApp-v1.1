'use strict';
angular.module('postoffice.pos')
    .controller('poAddController',
        ['$scope', 'apiService', 'notificationService', '$state',
            function($scope, apiService, notificationService, $state){       
   
                $scope.po = {
                    Status: true
                }

                $scope.AddPO = AddPO;
                function AddPO() {
                    apiService.post('/api/po/create', $scope.po,
                        function (result) {
                            notificationService.displaySuccess(result.data.Name + ' Thêm mới thành công!');
                            $state.go('pos');

                        }, function (error) {
                            notificationService.displayError('Thêm mới thất bại');
                        });
                }
                function loaddistricts() {
                    apiService.get('/api/district/getallparents', null, function (result) {
                        $scope.districts = result.data;
                    }, function () {
                        console.log('Can not load district list!');
                    });
                }
                loaddistricts();
            }
        ])

