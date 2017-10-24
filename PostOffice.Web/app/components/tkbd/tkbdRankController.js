(function (app) {
    app.controller('tkbdRankController', tkbdRankController);

    tkbdRankController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function tkbdRankController($scope, apiService, notificationService, $filter) {
        $scope.tabledata = [];
        $scope.labels = [];
        $scope.series = ['asdfds'];
        $scope.chartdata = [];
        var m = new Date();
        $scope.month = m.getMonth();
        function getRank() {
            $scope.loading = true;
            apiService.get('api/tkbd/rank', null,
                function (response) {
                    $scope.tabledata = response.data;
                    var l = [];
                    var d = [];
                    var c = [];

                    $.each(response.data, function (i, item) {
                        l.push(item.CreatedBy);
                        d.push(item.cl);
                    })
                    c.push(d);
                    $scope.chartdata = c;
                    $scope.labels = l;
                    $scope.loading = false;
                },

                function (response) {
                    if (response.status == 500) {
                        notificationService.displayError('Không có dữ liệu');
                        $scope.loading = false;
                    }
                    else {
                        notificationService.displayError('Không thể tải dữ liệu');
                        $scope.loading = false;
                    }
                });
        }

        getRank();
    }

})(angular.module('postoffice.tkbd'));