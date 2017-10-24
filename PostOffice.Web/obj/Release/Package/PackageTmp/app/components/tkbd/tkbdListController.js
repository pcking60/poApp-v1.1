'use strict';
angular.module('postoffice.tkbd')

    .controller('tkbdListController',
        ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter', '$state', '$stateParams', 'authService',
            function ($scope, apiService, notificationService, $ngBootbox, $filter, $state, $stateParams, authService) {
                $scope.page = 0;
                $scope.pagesCount = 0;
                $scope.tkbds = [];
                //$scope.getTkbds = getTkbds;
                $scope.keyword = '';
                $scope.search = search;
                $scope.update = update;
                $scope.loading = false;
                $scope.tkbd = {
                    functionId: null,
                    date: { startDate: null, endDate: null },
                    districts: [],
                    pos: [],
                    users: [],
                    districtId: 0,
                    posId: 0,
                    userId: '',
                    serviceId: 0
                };
                $scope.tkbd.dates = null;

                $stateParams.id = 0;

                $scope.functions =
                [
                    { Id: 1, Name: 'Thống kê tổng hợp giao dịch phát sinh' },
                    { Id: 2, Name: 'Thống kê chi tiết giao dịch phát sinh' }
                ];

                //check role 
                $scope.isManager = authService.haveRole('Manager');
                $scope.isAdmin = authService.haveRole('Administrator');
                $scope.isSupport = authService.haveRole('Support');

                if ($scope.isAdmin || $scope.isSupport) {
                    getDistricts();
                }
                else {
                    if ($scope.isManager) {
                        $stateParams.id = authService.authentication.userName;
                        apiService.get('/api/applicationUser/userinfo',
                            null,
                            function (response) {
                                $stateParams.id = response.data.POID;
                                $scope.tkbd.districtId = response.data.POID;
                                getPos();
                            },
                            function (response) {
                                notificationService.displayError('Không tải được danh sách huyện/ thành phố.');
                            });
                    }
                    else {

                    }
                }
                  

                //lấy danh sách huyện / đơn vị
                $scope.getDistricts = getDistricts;
                function getDistricts() {
                    apiService.get('/api/district/getallparents',
                        null,
                        function (response) {
                            $scope.tkbd.districts = response.data;
                        }, function (response) {
                            notificationService.displayError('Không tải được danh sách huyện.');
                        }
                    );
                }

                // câp nhật danh sách bưu cục của đơn vị được chọn
                $scope.updatePos = function (item) {
                    if (item !== 0 && item !== null) {
                        $stateParams.id = item;
                        getPos();
                    }
                    else {
                        $scope.tkbd.pos = [];
                        $scope.tkbd.posId = 0;
                    }
                };

                // lấy danh sách bưu cục 
                $scope.getPos = getPos;
                function getPos() {
                    apiService.get('/api/po/getbydistrictid/ ' + $stateParams.id,
                        null,
                        function (response) {
                            $scope.tkbd.pos = response.data;
                        }, function (response) {
                            notificationService.displayError('Không tải được danh sách đơn vị.');
                        }
                    );
                }

                // lấy danh sách users của bưu cục được chọn
                $scope.updateUser = function (item) {
                    if (item !== 0 && item !== null) {
                        $stateParams.id = item;
                        getListUser();
                    }
                    else {
                        $scope.tkbd.users = [];
                        $scope.tkbd.userId = 0;
                    }
                };

                // lấy danh sách người dùng
                $scope.getListUser = getListUser;
                function getListUser() {
                    apiService.get('/api/applicationUser/getuserbypoid/' + $stateParams.id,
                        null,
                        function (response) {
                            $scope.tkbd.users = response.data;
                        }, function (response) {
                            notificationService.displayError('Không tải được danh sách nhân viên.');
                        });
                }
            
                function search() {
                    getTkbds();
                }
                
                function update() {
                    $scope.loading = true;
                    apiService.get('/api/tkbd/update', null, function (result) {
                        //console.log(result.data.TotalCount);                                               
                        $scope.tkbd = result.data.Items;
                        $scope.page = result.data.Page;
                        $scope.pagesCount = result.data.TotalPages;
                        $scope.totalCount = result.data.TotalCount;
                        $state.reload();
                    },
                    function () {                        
                        console.log('Load tkbds failed');
                    });
                }

                $scope.Export = Export;
                function Export() {
                    var month = $scope.tkbd.dates.getMonth()+1;
                    var year = $scope.tkbd.dates.getFullYear();
                    console.log($scope.tkbd.dates);
                    var config = {
                        params: {
                            //mm/dd/yyyy
                            month: month,
                            year: year,
                            districtId: $scope.tkbd.districtId || 0,
                            functionId: $scope.tkbd.functionId || 0,
                            poId: $scope.tkbd.poId || 0,
                            userId: $scope.tkbd.userId || '',
                            serviceId: $scope.tkbd.serviceId || 0
                        }
                    };
                    apiService.get('/api/tkbd/export', config,
                        function (response) {
                            $scope.loading = true;
                            const st = 200;
                            if (response.status === st) {
                                window.location.href = response.data.Message;
                                $scope.loading = false;
                            }
                        },
                        function (response) {
                            if (response.status === 500) {
                                notificationService.displayError('Không có dữ liệu');
                                $scope.loading = false;
                            }
                            else {
                                notificationService.displayError('Không thể tải dữ liệu');
                                $scope.loading = false;
                            }
                        }
                    );
                }
                
            }]);