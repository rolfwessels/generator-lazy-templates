'use strict';

/* dashboardCtrl */

angular.module('webapp.controllers')
  .controller('loginCtrl', ['$scope','$log','$mdToast',	function ($scope, $log, $mdToast) {
  	/*
  	 * Scope
  	 */
	$scope.model = {email:"admin", password:"admin!"};
	$scope.toastPosition = "top left right";
	$scope.login = login;
	$scope.forgotPassword = forgotPassword;
  	
  	/*
  	 * Functions
  	 */

    function login() {
    	console.log($scope.model);
		$mdToast.show(
	      $mdToast.simple()
	        .content('Login!')
	        .position($scope.toastPosition)
	        .hideDelay(3000)
	    );
    }
	
	function forgotPassword() {
    	$mdToast.show(
	      $mdToast.simple()
	        .content('Password!')
	        .position($scope.toastPosition)
	        .hideDelay(3000)
	    );
    }

  }]);

