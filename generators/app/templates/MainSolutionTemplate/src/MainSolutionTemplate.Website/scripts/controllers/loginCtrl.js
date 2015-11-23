/* dashboardCtrl */

angular.module('webapp.controllers')
    .controller('loginCtrl', ['$scope', '$log', 'service.message', 'service.authorization`','$location',
        function($scope, $log, messageService, authorizationService,$location) {

            /*
             * Scope
             */
            var  currentUser = authorizationService.currentSession();
            $scope.model = {
                email: currentUser.email||'asdf',
                password: ''
            };
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
                    messageService.error(message||'Invalid username or password.');
                },messageService.info);

            }

            function forgotPassword() {
	            $location.path('forgotPassword');
            }

        }
    ]);
