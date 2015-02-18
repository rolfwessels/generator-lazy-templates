
'use strict';

  angular
      .module('webapp',[
      	'ngMaterial',
        'webapp.routes',
      	'webapp.controllers',
      	'webapp.directives',
      	'webapp.filters',
      	'webapp.services'
      	])
      .value('tokenUrl','http://localhost:8081/token')
      .value('apiUrlBase','http://localhost:8081/api')
      .config(function($mdThemingProvider) {
        $mdThemingProvider.theme('default')
          .primaryPalette('light-green')
          .accentPalette('amber');

      });

  

