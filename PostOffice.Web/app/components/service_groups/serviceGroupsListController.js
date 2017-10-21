(function (app) {
    app.controller('serviceGroupsListController', serviceGroupsListController);
    serviceGroupsListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];
    function serviceGroupsListController($scope, apiService, notificationService, $ngBootbox, $filter) {
             
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.serviceGroups = [];     
        $scope.getServiceGroups = getServiceGroups;
        $scope.keyword = '';
        $scope.search = search;
        $scope.deleteServiceGroup = deleteServiceGroup;
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
                    checkedServiceGroups: JSON.stringify(listId)
                }
            }
            $ngBootbox.confirm('Bạn có chắc xóa không?').then(
                function () {
                    apiService.del('/api/servicegroup/deletemulti', config, function (result) {
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
                angular.forEach($scope.serviceGroups, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.serviceGroups, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("serviceGroups", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);
        function deleteServiceGroup(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/servicegroup/delete', config, function () {
                    notificationService.displaySuccess('Xóa mẫu tin thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa mẫu tin thất bại!');
                });
            }, function () {
                console.log('Command was cancel');
            });
        }
        function search() {
            getServiceGroups();
        }
        function getServiceGroups(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword : $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('/api/servicegroup/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning("Chưa có dữ liệu");
                    
                }
                $scope.loading = false;
                $scope.serviceGroups = result.data.Items;                
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            },
            function () {
                $scope.loading = false;
                console.log('Load servicegroups failed');
            });

        }
        $scope.getServiceGroups();
        
    }
})(angular.module('postoffice.service_groups'));