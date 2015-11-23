/* routes */

angular
	.module('webapp.routes', ['ngRoute'])
	.config(['$routeProvider', function($routeProvider) {

        $routeProvider
        .when('/', {
              templateUrl: 'views/dashboard.html',
              controller: 'controller.dashboard'
        })
        .when('/user/:id?', {
            templateUrl: 'views/user.html',
            controller: 'controller.userCrud'
        })
        .when('/project/:id?', {
            templateUrl: 'views/project.html',
            controller: 'controller.projectCrud'
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