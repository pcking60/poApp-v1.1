(function (app) {
    app.controller('transactionsListController', transactionsListController);
    transactionsListController.$inject = ['$scope', 'apiService', 'notificationService',
            '$ngBootbox', '$filter', '$state', 'authService'];
    function transactionsListController($scope, apiService, notificationService, $ngBootbox, $filter, $state, authService) {
        $scope.loading = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.transactions = [];
        $scope.keyword = '';
        $scope.search = search;
        $scope.deleteTransaction = deleteTransaction;
        $scope.function = 0;
        $scope.getTransactionsIn30Days =
        function getTransactionsIn30Days(page) {
            $scope.function = 30;
            page = page || 0;
            var config = {
                params: {
                    page: page,
                    pageSize: 20
                }
            };
            $scope.loading = true;
            apiService.get('/api/transactions/getall30days', config,
                function (result) {
                    if (result.data.TotalCount == 0) {
                        notificationService.displayWarning("Chưa có dữ liệu");
                    }
                    $scope.transactions = [];
                    $scope.transactions = result.data.Items;
                    $scope.page = result.data.Page;
                    $scope.pagesCount = result.data.TotalPages;
                    $scope.totalCount = result.data.TotalCount;
                    console.log(result.data.Count);
                    $scope.loading = false;
                },
                function () {
                    $scope.loading = false;
                    console.log('Load transactions failed');
                });
        }

        $scope.getTransactionsIn7Days = 
        function getTransactionsIn7Days(page) {
            $scope.function = 7;
            page = page || 0;
            var config = {
                params: {
                    page: page,
                    pageSize: 20
                }
            };
            $scope.loading = true;
            apiService.get('/api/transactions/getall7days', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning("Chưa có giao dịch phát sinh trong 7 ngày gần đây!");
                }
                $scope.transactions = [];
                $scope.transactions = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                console.log(result.data.Count);
                $scope.loading = false;
            },
            function () {
                $scope.loading = false;
                console.log('Load transactions failed');
            });
        }

        //test gettime()
        $scope.currentDate = new Date();     
        
        function deleteTransaction(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/transactions/delete', config,
                    function (result) {
                        notificationService.displaySuccess('Giao dịch đã được xóa');
                        $state.reload();
                        //$state.go('transactions');
                    }, function (error) {
                        notificationService.displayError('Xóa giao dịch thất bại, vui lòng liên hệ quản trị');
                    });
                }, function () {
                console.log('Command was cancel');
            });
         }

        function search() {
            getTransactionsIn30Days();
        }

        $scope.getTransactions =
        function getTransactions(page) {
            page = page || 0;
            var config = {
                params: {
                    page: page,
                    pageSize: 40
                }
            };
            $scope.loading = true;
            apiService.get('/api/transactions/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning("Chưa có giao dịch phát sinh trong ngày!");                    
                } 
                $scope.transactions = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                console.log(result.data.Count);                
                $scope.loading = false;
            },
            function () {
                $scope.loading = false;
                console.log('Load transactions failed');
            });

        }

        $scope.authentication = authService.authentication;
        var userName = $scope.authentication.userName;

        const ACCEPTABLE_OFFSET = 172800*1000;

        $scope.editEnabled = function(transaction)
        {
            return (new Date().getTime() - (new Date(transaction.TransactionDate)).getTime()) > ACCEPTABLE_OFFSET;
        }        
    }

    
})(angular.module('postoffice.transactions'));