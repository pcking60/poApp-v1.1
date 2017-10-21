/// <reference path="~/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('postoffice.transactions', ['postoffice.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $urlRouterProvider.otherwise('/transactions');

        $stateProvider
            
            .state('transactions', {
                url: "/danh-sach-giao-dich.html",
                parent: 'base',
                templateUrl: "/app/components/transactions/transactionsListView.html",
                controller: "transactionsListController"
            }).state('add_transaction', {
                url: "/them-moi-giao-dich/:id.html",
                parent: 'base',
                templateUrl: "/app/components/transactions/transactionAddView.html",
                controller: "transactionAddController"
            }).state('edit_transaction', {
                url: "/cap-nhat-giao-dich/:id.html",
                parent: 'base',
                templateUrl: "/app/components/transactions/transactionEditView.html",
                controller: "transactionEditController"
            });
    };
})();