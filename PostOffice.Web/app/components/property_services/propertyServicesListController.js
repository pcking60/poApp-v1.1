'use strict';
angular.module('postoffice.property_services')
    .controller('propertyServicesListController',
        ['$scope', 'apiService', 'notificationService', '$ngBootbox','$filter', 
            function($scope, apiService, notificationService, $ngBootbox, $filter) {            
                $scope.page = 0;
                $scope.pagesCount = 0;
                $scope.propertyServices = [];
                $scope.getPropertyServices = getPropertyServices;
                $scope.keyword = '';
                $scope.search = search;
                $scope.selectAll = selectAll;
                $scope.deleteMulti = deleteMulti;
                $scope.loading = true;

                function deleteMulti() {
                    var listId = [];
                    $.each($scope.selected, function (i, item) {
                        listId.push(item.ID);
                    });
                    var config = {
                        params: {
                            checkedPropertyServices: JSON.stringify(listId)
                        }
                    }
                    $ngBootbox.confirm('Bạn có chắc xóa không?').then(
                        function () {
                            apiService.del('/api/property_services/deletemulti', config, function (result) {
                                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                                search();
                            }, function (error) {
                                notificationService.displayError('Xóa không thành công');
                            });
                        }, function () {
                            console.log('Command was cancel');
                        });

                }
                $scope.isAll = false;
                function selectAll() {
                    if ($scope.isAll === false) {
                        angular.forEach($scope.propertyServices, function (item) {
                            item.checked = true;
                        });
                        $scope.isAll = true;
                    } else {
                        angular.forEach($scope.propertyServices, function (item) {
                            item.checked = false;
                        });
                        $scope.isAll = false;
                    }
                }

                $scope.$watch("propertyServices", function (n, o) {
                    var checked = $filter("filter")(n, { checked: true });
                    if (checked.length) {
                        $scope.selected = checked;
                        $('#btnDelete').removeAttr('disabled');
                    } else {
                        $('#btnDelete').attr('disabled', 'disabled');
                    }
                }, true);
                function search() {
                    getPropertyServices();
                }
                $scope.deletePropertyService = deletePropertyService;
                function deletePropertyService(id) {
                    $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                        var config = {
                            params: {
                                id: id
                            }
                        }
                        apiService.del('/api/property_services/delete', config,
                        function () {
                            notificationService.displaySuccess('Xóa dữ liệu thành công');
                            search();
                        }
                        , function () {
                            notificationService.displayError('Xóa dữ liệu thất bại');
                        });
                    },
                    function () {
                        console.log('Command was cancel');
                    });
                }

                function getPropertyServices(page) {
                    page = page || 0;
                    var config = {
                        params: {

                            page: page,
                            pageSize: 100
                        }
                    }
                    apiService.get('/api/property_services/getall', config, function (result) {
                        if (result.data.TotalCount == 0) {
                            notificationService.displayWarning("Không tìm thấy bản ghi nào!");
                        }
                        $scope.loading = false;
                        $scope.propertyServices = result.data.Items;
                        $scope.page = result.data.Page;
                        $scope.pagesCount = result.data.TotalPages;
                        $scope.totalCount = result.data.TotalCount;
                    },
                    function () {
                        $scope.loading = false;
                        console.log('Load property services failed');
                    });
                } $scope.getPropertyServices();

                //function loadServices() {
                //    apiService.get('/api/service/getallparents', null, function (result) {
                //        $scope.listServices = result.data;
                //    }, function () {
                //        console.log('Can not load services!');
                //    });
                //}
                //loadServices();
            }]);

   
        
