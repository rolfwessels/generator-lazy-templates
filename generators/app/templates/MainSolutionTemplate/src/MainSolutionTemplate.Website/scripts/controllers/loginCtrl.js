'use strict';

/* dashboardCtrl */

angular.module('webapp.controllers')
  .controller('loginCtrl', ['$scope','$log','$mdToast',	function ($scope, $log, $mdToast) {
	$scope.model = {email:"admin", password:"admin!"}
	$scope.toastPosition = {	    bottom: false,	    top: true,	    left: false,	    right: true	  };
	$scope.login = login;
	$scope.forgotPassword = forgotPassword;
  	
    function login() {
    	console.log($scope.model);
		$mdToast.show(
	      $mdToast.simple()
	        .content('Login!')
	        .position($scope.getToastPosition())
	        .hideDelay(3000)
	    );
    }
	
	function forgotPassword() {
    	$mdToast.show(
	      $mdToast.simple()
	        .content('Password!')
	        .position($scope.getToastPosition())
	        .hideDelay(3000)
	    );
    }

  }]);

