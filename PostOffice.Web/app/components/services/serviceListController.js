(function (app) {
    app.controller('serviceListController', serviceListController);
    serviceListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox'];
    function serviceListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.services = [];
        $scope.getServices = getServices;
        $scope.AllServices = [];
        $scope.getAllServices = getAllServices;
        $scope.keyword = '';
        $scope.search = search;
        $scope.deleteService = deleteService;
        $scope.loading = true;
        $scope.exportExcel = exportExcel;

        function exportExcel() {
            var config = {
                params: {
                    filter: $scope.keyword
                }
            };
            apiService.get('/api/service/ExportXls', config, function (response) {
                if (response.status = 200) {
                    window.location.href = response.data.Message;
                }
            }, function (error) {
                notificationService.displayError(error);

            });
        }

        function deleteService(id) {
            $ngBootbox.confirm('Bạn có chắc xóa không?')
                .then(
                    function () {
                        config =
                            {
                                params: {
                                    id: id
                                }
                            };
                apiService.del(
                    '/api/service/delete',
                    config,
                    function () {
                        notificationService.displaySuccess('Xóa dữ liệu thành công');
                        search();
                    },
                    function () {
                        notificationService.displaySuccess('Xóa dữ liệu thất bại');
                    }

                );

            }, function () {
                console.log('Command was cancel!');
            });
        }

        function search() {
            getServices();
        }
        function getServices(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 100
                }
            };
            apiService.get('/api/service/getall', config, function (result) {
                if (result.data.TotalCount === 0) {
                    notificationService.displayWarning("Chưa có dữ liệu");
                }                
                $scope.services = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.loading = false;
            },
            function () {
                console.log('Load service failed');
                $scope.loading = false;
            });
        };

        function getAllServices() {
            apiService.get('api/service/getallparents', null,
                function (result) {
                    $scope.AllServices = result.data;
                },
                function () {
                    console.log('Load service failed');
                });
        };

        //$scope.getAllServices();
        //$scope.getServices();
    }
})(angular.module('postoffice.services'));
