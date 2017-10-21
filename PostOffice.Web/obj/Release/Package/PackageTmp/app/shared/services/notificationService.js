(function (app) {
    app.factory('notificationService', notificationService);

    function notificationService() {
        toastr.options = {
            "debug": false,
            "positionClass": "toast-top-center",
            "onclick": null,
            "showDuration": 300,
            "hideDuration": 1000,
            "timeOut": 3000,
            "extendedTimeOut": 1000
        };

        function displaySuccess(message) {
            toastr.success(message);
        }

        function displayError(error) {
            if (Array.isArray(error)) {
                error.each(function (err) {
                    toastr.error(err);
                });
            }
            else {
                toastr.error(error);
            }
        }

        function displayWarning(message) {
            toastr.warning(message);
        }
        function displayInfo(message) {
            toastr.info(message);
        }

        return {
            displaySuccess: displaySuccess,
            displayError: displayError,
            displayWarning: displayWarning,
            displayInfo: displayInfo
        }
    }
})(angular.module('postoffice.common'));