'use strict';

/* routes */

angular
	.module('webapp.routes', ['ngRoute'])
	.config(['$routeProvider', function($routeProvider) {
      console.log("Adding routing");

        $routeProvider
        .when('/', {
              templateUrl: 'views/dashboard.html',
              controller: 'dashboardCtrl'
            })
        .when('/login', {
              templateUrl: 'views/login.html',
              controller: 'loginCtrl'
            })
        .otherwise({ redirectTo: '/' });

      }
    ]);
