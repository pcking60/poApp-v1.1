(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function revenueStatisticController($scope, apiService, notificationService, $filter) {
        $scope.tabledata = [];
        $scope.labels = [];
        $scope.series = ['asdfds'];

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
            apiService.get('api/statistic/getrevenue?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null,
                function (response)
                {
                    $scope.tabledata = response.data;
                    var labels = [];
                    var chartData = [];
                    var totalMoney = [];
                
                    $.each(response.data, function (i, item) {
                        labels.push($filter('date')(item.TransactionDate, 'dd/MM/yyyy'));
                        totalMoney.push(item.totalMoney);
                    
                    });
                    chartData.push(totalMoney);
               

                    $scope.chartdata = chartData;
                    $scope.labels = labels;
                },

                function (response)
                {
                    if (response.status == 500) {
                        notificationService.displayError('Không có dữ liệu');
                    }
                    else
                    {
                        notificationService.displayError('Không thể tải dữ liệu');
                    }                
            });
        }

        getStatistic();
    }

})(angular.module('postoffice.statistics'));