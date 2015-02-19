'use strict';

angular
    .module('webapp', [
        'ngMaterial',
				'angular-loading-bar',
        'webapp.routes',
        'webapp.controllers',
        'webapp.directives',
        'webapp.filters',
        'webapp.services'
    ])
    .value('tokenUrl', 'http://localhost:8081/token')
    .value('apiUrlBase', 'http://localhost:8081/api')
    .config(function($mdThemingProvider) {
        $mdThemingProvider.theme('default')
            .primaryPalette('light-green')
            .accentPalette('amber');

    })
    .config(['cfpLoadingBarProvider', function(cfpLoadingBarProvider) {
      cfpLoadingBarProvider.includeSpinner = false;
    }])
    .run(['authorizationService', function(authorizationService) {
      authorizationService.isAuthenticatedOrRedirect();
    }]);
