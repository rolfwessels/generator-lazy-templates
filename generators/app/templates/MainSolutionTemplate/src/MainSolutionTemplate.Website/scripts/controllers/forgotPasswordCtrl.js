/* dashboardCtrl */

angular.module('webapp.controllers')
    .controller('forgotPasswordCtrl', ['$scope', '$log', 'messageService', 'authorizationService','$location',
        function($scope, $log, messageService, authorizationService,$location) {

            /*
             * Scope
             */
			var  currentUser = authorizationService.currentSession();

            $scope.model = { email: currentUser.email };
            $scope.forgotPassword = forgotPassword;

            /*
             * Functions
             */

            function forgotPassword() {
               var authenticate = authorizationService.forgotPassword($scope.model.email);
                authenticate.then(function() {
                	messageService.info('Your password has been sent to your email');
                    $location.path("/login");
                },messageService.error);

            }

        }
    ]);
