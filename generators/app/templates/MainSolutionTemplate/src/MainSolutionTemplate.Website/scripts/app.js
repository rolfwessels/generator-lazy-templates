angular
    .module('webapp', [
		'angular-loading-bar',
        'ui.materialize',
        'webapp.routes',
        'webapp.controllers',
        'webapp.directives',
        'webapp.filters',
        'webapp.services'
    ])
    .value('tokenUrl', 'http://localhost:8081/token')
    .value('apiUrlBase', 'http://localhost:8081/api')
    .value('signalrBase', 'http://localhost:8081/signalr')    
    .config(function() {
        $(".button-collapse").sideNav({
          menuWidth: 300, // Default is 240
          edge: 'left', // Choose the horizontal origin
          closeOnClick: true // Closes side-nav on <a> clicks, useful for Angular/Meteor
        });
    })
    .config(['cfpLoadingBarProvider', function(cfpLoadingBarProvider) {
      cfpLoadingBarProvider.includeSpinner = false;
    }])
    .run(['service.authorization`', function(authorizationService) {
      authorizationService.isAuthenticatedOrRedirect();
    }]);
