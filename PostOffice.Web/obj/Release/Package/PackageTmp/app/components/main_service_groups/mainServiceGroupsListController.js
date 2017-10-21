(function (app) {
    app.controller('mainServiceGroupsListController', mainServiceGroupsListController);
    mainServiceGroupsListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];
    function mainServiceGroupsListController($scope, apiService, notificationService, $ngBootbox, $filter) {
             
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.mainServiceGroups = [];
        $scope.getMainServiceGroups = getMainServiceGroups;
        $scope.keyword = '';
        $scope.search = search;
        $scope.deleteMainServiceGroup = deleteMainServiceGroup;
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
                    checkedMainServiceGroups: JSON.stringify(listId)
                }
            }
            $ngBootbox.confirm('Bạn có chắc xóa không?').then(
                function () {
                    apiService.del('/api/mainservicegroup/deletemulti', config, function (result) {
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
                angular.forEach($scope.mainServiceGroups, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.mainServiceGroups, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("mainServiceGroups", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);
        function deleteMainServiceGroup(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/mainservicegroup/delete', config, function () {
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
            getMainServiceGroups();
        }
        function getMainServiceGroups(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword : $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('/api/mainservicegroup/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning("Chưa có dữ liệu");
                    
                }
                $scope.loading = false;
                $scope.mainServiceGroups = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            },
            function () {
                $scope.loading = false;
                console.log('Load mainservicegroups failed');
            });

        }
        $scope.getMainServiceGroups();
        
    }
})(angular.module('postoffice.main_service_groups'));