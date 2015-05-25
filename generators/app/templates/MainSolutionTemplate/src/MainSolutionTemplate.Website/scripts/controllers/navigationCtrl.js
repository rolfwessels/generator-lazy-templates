/* dashboardCtrl */

angular.module('webapp.controllers')
  .controller('navigationCtrl', ['$scope','$rootScope', 'authorizationService', '$timeout', '$location',
		function ($scope,$rootScope, authorizationService, $timeout, $location) {
    
    
    $scope.logout = logout;
    $scope.navigateHome = navigateHome;
    $scope.login = login;
    $rootScope.$watch("isAuthenticated", function(newValue) {
      if (!newValue) {
          $timeout(function() {

          },500);           
        }
      }
    );
      
    function navigateHome() {
      $location.path("/");
    }

    function login() {
      $location.path("/login");
    }

    function logout() {
      authorizationService.logout();
    }
  }]);
