/* authorizationService */

angular.module('webapp.services')
    .service('messageService', ['$log', '$mdToast',
        function($log, $mdToast) {

            var toastPosition = "top left right";

            /*
             * Private methods
             */

            /* 
             * Service
             */
            return {
                info: function(message) {
                    $log.info(message);
                    $mdToast.show(
                        $mdToast.simple()
                        .content(message)
                        .position(toastPosition)
                        .hideDelay(3000)
                    );
                },
                warn: function(message) {
                    $log.warn(message);
                    $mdToast.show(
                        $mdToast.simple()
                        .content(message)
                        .position(toastPosition)
                        .hideDelay(3000)
                    );
                },
                error: function(message) {
                    $log.error(message);
                    $mdToast.show(
                        $mdToast.simple()
                        .content(message)
                        .position(toastPosition)
                        .hideDelay(3000)
                    );
                },
                debug: function(message) {
                    $log.debug(message);

                }

            };

        }
    ]);
