/* routes */

angular
	.module('webapp.routes', ['ngRoute'])
	.config(['$routeProvider', function($routeProvider) {

        $routeProvider
        .when('/', {
              templateUrl: 'views/dashboard.html',
              controller: 'dashboardCtrl'
        })
        .when('/user', {
            templateUrl: 'views/user.html',
            controller: 'userCtrl'
        })
        .when('/login', {
              templateUrl: 'views/login.html',
              controller: 'loginCtrl'
            })
        .when('/forgotPassword', {
              templateUrl: 'views/forgotPassword.html',
              controller: 'forgotPasswordCtrl'
            })
        .otherwise({ redirectTo: '/' });

      }
    ]);