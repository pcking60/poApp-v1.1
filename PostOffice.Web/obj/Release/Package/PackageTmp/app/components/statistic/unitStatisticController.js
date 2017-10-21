(function (app) {
    app.controller('unitStatisticController', unitStatisticController);

    unitStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function unitStatisticController($scope, apiService, notificationService, $filter) {
        $scope.tabledata = [];
        $scope.labels = [];
        $scope.series = ['des_me'];

        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var currentMonth = firstDay.getMonth();
        var strFirstDay = firstDay.getMonth() + 1 + '/' + firstDay.getDate() + '/' + date.getFullYear();
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        var strLastDay = firstDay.getMonth() + 1 + '/' + lastDay.getDate() + '/' + date.getFullYear();

        $scope.chartdata = [];
        function getStatistic() {
            var config = {
                param: {
                    //mm/dd/yyyy
                    fromDate: strFirstDay,
                    toDate: strLastDay
                }
            }
            apiService.get('api/statistic/getunit?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null,
                function (response) {
                    $scope.tabledata = response.data;
                    var labels = [];
                    var chartData = [];
                    var totalMoney = [];

                    $.each(response.data, function (i, item) {
                        //labels.push($filter('date')(item.CreatedDate, 'dd/MM/yyyy'));
                        labels.push(item.Unit);
                        totalMoney.push(item.TotalMoney);

                    });
                    chartData.push(totalMoney);


                    $scope.chartdata = chartData;
                    $scope.labels = labels;
                },

                function (response) {
                    if (response.status == 500) {
                        notificationService.displayError('Không có dữ liệu');
                    }
                    else {
                        notificationService.displayError('Không thể tải dữ liệu');
                    }
                });
        }

        getStatistic();
    }

})(angular.module('postoffice.statistics'));