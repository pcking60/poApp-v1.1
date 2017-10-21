(function (app) {
    app.controller('homeController', homeController);
    homeController.$inject = ['$scope', 'authService'];
    function homeController($scope, authService) {
        
    }
})(angular.module('postoffice'));