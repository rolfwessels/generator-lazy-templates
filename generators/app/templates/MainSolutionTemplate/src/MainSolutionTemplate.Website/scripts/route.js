/* routes */

angular
	.module('webapp.routes', ['ngRoute'])
	.config(['$routeProvider', function($routeProvider) {

        $routeProvider
        .when('/', {
              templateUrl: 'views/dashboard.html',
              controller: 'dashboardCtrl'
        })
        .when('/user/:id?', {
            templateUrl: 'views/user.html',
            controller: 'userCtrl'
        })
        .when('/project/:id?', {
            templateUrl: 'views/project.html',
            controller: 'projectCtrl'
        })
        .when('/login/', {
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