'use strict';

/* dashboardCtrl */

angular.module('webapp.controllers')
    .controller('forgotPasswordCtrl', ['$scope', '$log', '$mdToast', 'authorizationService','$location',
        function($scope, $log, $mdToast, authorizationService,$location) {

            /*
             * Scope
             */
			var  currentUser = authorizationService.currentSession();

            $scope.model = { email: currentUser.email };
            $scope.toastPosition = "top left right";
            $scope.forgotPassword = forgotPassword;

            /*
             * Functions
             */

            function forgotPassword() {
               var authenticate = authorizationService.forgotPassword($scope.model.email);
                authenticate.then(function() {
                	$mdToast.show(
                        $mdToast.simple()
                        .content('Your password has been sent to your email')
                        .position($scope.toastPosition)
                        .hideDelay(3000)
                    )
                    $location.path("/login");
                }, function(message) {
                    $mdToast.show(
                        $mdToast.simple()
                        .content(message || 'Ooops something has gone wrong.')
                        .position($scope.toastPosition)
                        .hideDelay(3000)
                    );
                });

            }

        }
    ]);
