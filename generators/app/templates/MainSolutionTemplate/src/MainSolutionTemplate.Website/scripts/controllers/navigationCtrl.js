'use strict';

/* dashboardCtrl */

angular.module('webapp.controllers')
  .controller('navigationCtrl', ['$scope', '$mdSidenav',
		function ($scope, $mdSidenav) {
    
    $scope.toggleSidenav = toggleSideNav;
 
  	/**
		 * Hide or Show the sideNav area
		 * @param menuId
		 */

  	function toggleSideNav(name) {
  		$mdSidenav(name).toggle();
  	}

  }]);
