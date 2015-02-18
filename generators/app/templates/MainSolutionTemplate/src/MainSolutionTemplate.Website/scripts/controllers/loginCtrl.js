'use strict';

/* dashboardCtrl */

angular.module('webapp.controllers')
  .controller('loginCtrl', ['$scope','$log','$mdToast','authorizationService',	
  	function ($scope, $log, $mdToast, authorizationService) {
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
    	var authorize = authorizationService.authorize($scope.model.email,$scope.model.password);
    	authorize.then(function() {

    	},function (message) {
    		$mdToast.show(
		      $mdToast.simple()
		        .content(message || 'Invalid username or password.')
		        .position($scope.toastPosition)
		        .hideDelay(3000)
		    );
    	});
		
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

