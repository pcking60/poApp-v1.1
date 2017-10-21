(function (app) {
   

    
    app.controller('serviceGroupAddController', serviceGroupAddController);
    serviceGroupAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state', 'commonService', '$rootScope'];
    function serviceGroupAddController($scope, apiService, notificationService, $state, commonService, $rootScope) {
        $scope.serviceGroup = {
            Status: true
        }
        $scope.flatFolders = [];
        $scope.parentGroups = [];
        $scope.changeSomething = changeSomething;
        function changeSomething() {
            $scope.serviceGroup.Alias = commonService.getSeoTitle($scope.serviceGroup.Name);
            //$scope.serviceGroup.Name = commonService.toTitleCase($scope.serviceGroup.Name);
        }
        $scope.AddServiceGroup = AddServiceGroup;
        function AddServiceGroup() {
            apiService.post('/api/servicegroup/create', $scope.serviceGroup,
                function (result) {
                    notificationService.displaySuccess('Thêm mới thành công!');
                    $state.go('service_groups');
                }, function (error) {
                    notificationService.displayError('Thêm mới thất bại');
            });
        }       
        
        function loadparentGroup() {
            apiService.get('/api/mainservicegroup/getallparents', null, function (result) {
                //console.log(result);
                $scope.parentGroups = result.data;
                //$scope.parentGroups = commonService.getTree(result.data, 'ID', 'MainServiceGroupId');
                //$scope.parentGroups.forEach(function (item) {
                //    recur(item, 0, $scope.flatFolders);
                //});
            }, function () {
                console.log('Can not load Parent Group');
            }); 
        }
        loadparentGroup();

        function times(n, str) {
            var result = '';
            for (var i = 0; i < n; i++) {
                result += str;
            }
            return result;
        };
        function recur(item, level, arr) {
            arr.push({
                Name: times(level, '–') + ' ' + item.Name,
                ID: item.ID,
                Level: level,
                Indent: times(level, '–')
            });
            if (item.children) {
                item.children.forEach(function (item) {
                    recur(item, level + 1, arr);
                });
            }
        };
    }
})(angular.module('postoffice.service_groups'));