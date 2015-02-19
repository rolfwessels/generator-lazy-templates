'use strict';

/* dashboardCtrl */

angular.module('webapp.controllers')
    .controller('loginCtrl', ['$scope', '$log', '$mdToast', 'authorizationService','$location',
        function($scope, $log, $mdToast, authorizationService,$location) {

            /*
             * Scope
             */
            var  currentUser = authorizationService.currentSession();
            $scope.model = {
                email: currentUser.email,
                password: "admin!"
            };
            $scope.toastPosition = "top left right";
            $scope.login = login;
            $scope.forgotPassword = forgotPassword;

            /*
             * Functions
             */

            function login() {
                
                var authenticate = authorizationService.authenticate($scope.model.email, $scope.model.password);
                authenticate.then(function() {
                    authorizationService.continueToPage();
                }, function(message) {
                	 $scope.model.password = "";
                    $mdToast.show(
                        $mdToast.simple()
                        .content(message || 'Invalid username or password.')
                        .position($scope.toastPosition)
                        .hideDelay(3000)
                    );
                });

            }

            function forgotPassword() {
                $location.path('forgotPassword')
            }

        }
    ]);
