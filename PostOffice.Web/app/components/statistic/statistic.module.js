
(function () {
    angular.module('postoffice.statistics', ['postoffice.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('statistic_revenue', {
                url: "/thong-ke-doanh-thu.html",
                parent: 'base',
                templateUrl: "/app/components/statistic/revenueStatisticView.html",
                controller: "revenueStatisticController"
            })
            .state('timeStatistic', {
                url: "/thong-ke-doanh-thu-theo-thoi-gian.html",
                parent: 'base',
                templateUrl: "/app/components/statistic/timeStatisticView.html",
                controller: "timeStatisticController"
            })
            .state('reports', {
                url: "/bao-cao-doanh-thu.html",
                parent: "base",
                templateUrl: "app/components/statistic/reportsView.html",
                controller: "reportsController"
            })
        ;
            
    }
})();