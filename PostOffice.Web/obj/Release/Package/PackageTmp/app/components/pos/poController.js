'use strict';
angular.module('postoffice.pos')
    .controller('poController',['$scope', 'apiService', 'notificationService', '$ngBootbox',
        function poController($scope, apiService, notificationService, $ngBootbox) {
            $scope.page = 0;
            $scope.pagesCount = 0;
            $scope.pos = [];
            $scope.getPOs = getPOs;
            $scope.keyword = '';
            $scope.search = search;

            $scope.deletePO = deletePO;
            function deletePO(id) {
                $ngBootbox.confirm('Bạn có chắc muốn xóa?')
                    .then(
                        function () {

                            var config = {
                                params: {
                                    id: id
                                }
                            }

                            apiService.del('/api/po/delete', config, function () {
                                notificationService.displaySuccess('Xóa dữ liệu thành công');
                                search();
                            }, function () {
                                notificationService.displaySuccess('Xóa dữ liệu thất bại');
                            })

                        }, 
                        function () {
                            console.log('Command was cancel');
                        }
                 );
            }
            function search() {
                getPOs();
            }
            function getPOs(page) {
                page = page || 0;
                var config = {
                    params: {
                        keyword: $scope.keyword,
                        page: page,
                        pageSize: 20
                    }
                }
                apiService.get('/api/po/getall', config, function (result) {
                    if (result.data.TotalCount == 0) {
                        notificationService.displayWarning("Không tìm thấy bản ghi nào!");
                    }
                
                    $scope.pos = result.data.Items;
                    $scope.page = result.data.Page;
                    $scope.pagesCount = result.data.TotalPages;
                    $scope.totalCount = result.data.TotalCount;
                },
                function () {
                    console.log('Load pos failed');
                });
            } 
            $scope.getPOs();    
        }
    ])
   