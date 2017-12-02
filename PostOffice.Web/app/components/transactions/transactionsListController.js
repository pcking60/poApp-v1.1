(function (app) {
    app.controller('transactionsListController', transactionsListController);
    transactionsListController.$inject = ['$scope', 'apiService', 'notificationService',
            '$ngBootbox', '$filter', '$state', 'authService', '$stateParams'];
    function transactionsListController($scope, apiService, notificationService, $ngBootbox, $filter, $state, authService, $stateParams) {
        $scope.loading = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.users = [];
        $scope.transactions = [];
        $scope.transaction = {
            totalQuantity: 0,
            totalCash: 0,
            totalSent: 0,
            totalCollect: 0,
            totalFee: 0,
            totalSales: 0,
            totalDebt: 0,
            totalEarn: 0,
            totalVat: 0,
            totalCurrency: 0
        };
        //check role 
        $scope.isManager = authService.haveRole('Manager');
        $scope.isAdmin = authService.haveRole('Administrator');
        $scope.isSupport = authService.haveRole('Support');
        if (!$scope.isAdmin && !$scope.isManager && !$scope.isSupport) //basic user
        {
            $stateParams.id = authService.authentication.userName;
            apiService.get('/api/applicationUser/userinfo',
               null,
               function (response) {
                   $stateParams.id = response.data.Id;
                   //lay danh sach giao dich cua basic user
                   getTransactionsByUserId();
               },
               function (response) {
                   notificationService.displayError('Không tải được danh sách giao dịch.');
               });
        }
        else {
            if ($scope.isManager && !$scope.isAdmin && !$scope.isSupport) // is manager
            {
                $stateParams.id = authService.authentication.userName;
                apiService.get('/api/applicationUser/userinfo',
                    null,
                    function (response) {
                        $stateParams.id = response.data.POID;
                        getListUser();
                    },
                    function (response) {
                        notificationService.displayError('Không tải được danh sách người dùng.');
                    });
            }
            else {
                if (!$scope.isAdmin && $scope.isSupport) // is supporter
                {
                    $stateParams.id = authService.authentication.userName;
                    apiService.get('/api/applicationUser/userinfo',
                        null,
                        function (response) {
                            $stateParams.id = response.data.POID;
                            getListUser();
                        },
                        function (response) {
                            notificationService.displayError('Không tải được danh sách người dùng.');
                        });
                }
            }
        }
        $scope.getListUser = getListUser;
        function getListUser() {
            apiService.get('/api/applicationUser/getuserbypoid/' + $stateParams.id,
                null,
                function (response) {
                    $scope.users = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách nhân viên.');
                });
        }
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
                        notificationService.displayWarning("Chưa có giao dịch phát sinh trong 30 ngày gần đây!");
                    }
                    else {
                        $scope.transactions = [];
                        $scope.transactions = result.data.Items;
                        $scope.page = result.data.Page;
                        $scope.pagesCount = result.data.TotalPages;
                        $scope.totalCount = result.data.TotalCount;
                        $scope.transaction.totalQuantity = 0;
                        $scope.transaction.totalCash = 0;
                        $scope.transaction.totalSent = 0;
                        $scope.transaction.totalDebt = 0;
                        $scope.transaction.totalEarn = 0;
                        $scope.transaction.totalVat = 0;
                        $scope.transaction.totalCurrency = 0;
                        angular.forEach($scope.transactions, function (item) {
                            if (item.Status === true) {
                                $scope.transaction.totalQuantity += item.Quantity;
                                $scope.transaction.totalCash += item.TotalCash;
                                $scope.transaction.totalSent += item.TotalMoneySent;
                                $scope.transaction.totalDebt += item.TotalDebt;
                                $scope.transaction.totalEarn += item.EarnMoney;
                                $scope.transaction.totalCurrency += item.TotalCurrency;
                                $scope.transaction.totalVat += item.TotalCurrency + item.TotalCash + item.TotalDebt + item.TotalMoneySent - (item.TotalCurrency + item.TotalCash + item.TotalDebt + item.TotalMoneySent) / item.VAT;
                            }
                        });
                    }
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
                else {
                    $scope.transactions = [];
                    $scope.transactions = result.data.Items;
                    $scope.page = result.data.Page;
                    $scope.pagesCount = result.data.TotalPages;
                    $scope.totalCount = result.data.TotalCount;
                    $scope.transaction.totalQuantity = 0;
                    $scope.transaction.totalCash = 0;
                    $scope.transaction.totalSent = 0;
                    $scope.transaction.totalDebt = 0;
                    $scope.transaction.totalEarn = 0;
                    $scope.transaction.totalVat = 0;
                    $scope.transaction.totalCurrency = 0;
                    angular.forEach($scope.transactions, function (item) {
                        if (item.Status === true) {
                            $scope.transaction.totalQuantity += item.Quantity;
                            $scope.transaction.totalCash += item.TotalCash;
                            $scope.transaction.totalSent += item.TotalMoneySent;
                            $scope.transaction.totalDebt += item.TotalDebt;
                            $scope.transaction.totalEarn += item.EarnMoney;
                            $scope.transaction.totalCurrency += item.TotalCurrency;
                            $scope.transaction.totalVat += item.TotalCurrency + item.TotalCash + item.TotalDebt + item.TotalMoneySent - (item.TotalCurrency + item.TotalCash + item.TotalDebt + item.TotalMoneySent) / item.VAT;
                        }
                    });
                }
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
                else {
                    $scope.transactions = result.data.Items;
                    $scope.page = result.data.Page;
                    $scope.pagesCount = result.data.TotalPages;
                    $scope.totalCount = result.data.TotalCount;
                    $scope.transaction.totalQuantity = 0;
                    $scope.transaction.totalCash = 0;
                    $scope.transaction.totalSent = 0;
                    $scope.transaction.totalDebt = 0;
                    $scope.transaction.totalEarn = 0;
                    $scope.transaction.totalVat = 0;
                    $scope.transaction.totalCurrency = 0;
                    angular.forEach($scope.transactions, function (item) {
                        if (item.Status === true) {
                            $scope.transaction.totalQuantity += item.Quantity;
                            $scope.transaction.totalCash += item.TotalCash;
                            $scope.transaction.totalSent += item.TotalMoneySent;
                            $scope.transaction.totalDebt += item.TotalDebt;
                            $scope.transaction.totalEarn += item.EarnMoney;
                            $scope.transaction.totalCurrency += item.TotalCurrency;
                            $scope.transaction.totalVat += item.TotalCurrency + item.TotalCash + item.TotalDebt + item.TotalMoneySent - (item.TotalCurrency + item.TotalCash + item.TotalDebt + item.TotalMoneySent) / item.VAT;
                        }
                    });
                }
                
                
                $scope.loading = false;
            },
            function () {
                $scope.loading = false;
                console.log('Load transactions failed');
            });

        }
        $scope.getTransactionsByUserId = getTransactionsByUserId;
        function getTransactionsByUserId(page) {
            page = page || 0;
            var config = {
                params: {                    
                    page: page,
                    pageSize: 20,
                    userId: $stateParams.id
                }
            };
            $scope.loading = true;
            apiService.get('/api/transactions/getallbyuserid', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning("Chưa có giao dịch phát sinh trong ngày!");
                }
                else {
                    $scope.transactions = result.data.Items;
                    $scope.page = result.data.Page;
                    $scope.pagesCount = result.data.TotalPages;
                    $scope.totalCount = result.data.TotalCount;
                    $scope.transaction.totalQuantity = 0;
                    $scope.transaction.totalCash = 0;
                    $scope.transaction.totalSent = 0;
                    $scope.transaction.totalDebt = 0;
                    $scope.transaction.totalCollect = 0;
                    $scope.transaction.totalFee = 0;
                    $scope.transaction.totalEarn = 0;
                    $scope.transaction.totalSales = 0;
                    $scope.transaction.totalVat = 0;
                    $scope.transaction.totalCurrency = 0;
                    angular.forEach($scope.transactions, function (item) {
                        if (item.Status === true) {
                            $scope.transaction.totalQuantity += item.Quantity;
                            $scope.transaction.totalCash += item.TotalCash;
                            $scope.transaction.totalSent += item.TotalMoneySent;
                            $scope.transaction.totalDebt += item.TotalDebt;
                            $scope.transaction.totalEarn += item.EarnMoney;
                            $scope.transaction.totalCollect += item.TotalColection;
                            $scope.transaction.totalFee += item.TotalFee;
                            $scope.transaction.totalSales += item.Sales;
                            $scope.transaction.totalCurrency += item.TotalCurrency;
                            $scope.transaction.totalVat += item.TotalVat;
                        }
                    });
                }
                $scope.loading = false;
            },
            function () {
                $scope.loading = false;
                console.log('Load transactions failed');
            });

        }
        $scope.updateTransactionByUserId = updateTransactionByUserId;
        function updateTransactionByUserId(userId) {
            reFresh();
            $stateParams.id = userId;
            getTransactionsByUserId();
        }
        function reFresh() {
            $scope.transactions = null;
            $scope.transaction.totalQuantity = 0;
            $scope.transaction.totalCash = 0;
            $scope.transaction.totalSent = 0;
            $scope.transaction.totalDebt = 0;
            $scope.transaction.totalCollect = 0;
            $scope.transaction.totalFee = 0;
            $scope.transaction.totalEarn = 0;
            $scope.transaction.totalSales = 0;
            $scope.transaction.totalVat = 0;
            $scope.transaction.totalCurrency = 0;
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