(function (app) {
    app.controller('tkbdImportListController', tkbdImportListController);
    tkbdImportListController.$inject = ['$scope', 'apiService', 'notificationService', '$state', 'commonService', '$stateParams', '$ngBootbox', '$timeout', '$http', 'authInterceptorService','localStorageService', 'authService'];
    function tkbdImportListController($scope, apiService, notificationService, $state, commonService, $stateParams, $ngBootbox, $timeout, $http, authInterceptorService, localStorageService, authService) {
       
        $scope.files = [];
        $scope.$on("fileSelected", function (event, args) {
            $scope.$apply(function () {
                $scope.files.push(args.file);
            });
        });
        $scope.TKBDImport = TKBDImport;
        function TKBDImport(config) {
            //delete $http.defaults.headers.common['X-Requested-With'];
            //console.log(authService.authentication);
            //if (authService.authentication != undefined) {
            //    $http.defaults.headers.common['Authorization'] = 'Bearer ' + authService.authentication.accessToken;
            //    $http.defaults.headers.common['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
            //};
            $http({
                method: 'POST',
                url: "/api/tkbd/import",
                headers: { 'Content-Type': undefined },                
                transformRequest: function (data) {
                    var formData = new FormData();
                    for (var i = 0; i < data.files.length; i++)
                    {
                        formData.append("file" + i, data.files[i]);
                    }
                    
                    return formData;
                },
                data: {files: $scope.files}
            }).then(function (result, status, headers, config) {
                notificationService.displaySuccess(result.data);
                $state.go('tkbdhistory');
            }, function (data, status, headers, config) {
                notificationService.displayError(data);
            })

        }

    }
})(angular.module('postoffice.tkbd'));