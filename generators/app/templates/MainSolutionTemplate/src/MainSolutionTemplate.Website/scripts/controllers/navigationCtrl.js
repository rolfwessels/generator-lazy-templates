'use strict';

/* dashboardCtrl */

angular.module('webapp.controllers')
  .controller('navigationCtrl', ['$scope','$rootScope', '$mdSidenav', 'authorizationService', '$timeout', '$location',
		function ($scope,$rootScope, $mdSidenav, authorizationService, $timeout, $location) {
    
    $scope.toggleSidenav = toggleSideNav;
    $scope.logout = logout;
    $rootScope.$watch("isAuthenticated", function(newValue) {
      if (!newValue) {
          $timeout(function() {
          	$mdSidenav('left').close();
          },500);           
        }
      }
    );
    

  	/**
		 * Hide or Show the sideNav area
		 * @param menuId
		 */

  	function toggleSideNav(name) {
  		$mdSidenav(name).toggle();
  	}


    function navigateHome() {
      $location.path("/");
    }


    function logout(name) {
      authorizationService.logout();
      $mdSidenav(name).toggle();
    }


  }]);
