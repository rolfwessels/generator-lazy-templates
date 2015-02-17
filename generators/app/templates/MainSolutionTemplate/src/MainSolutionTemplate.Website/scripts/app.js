
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
      
      .config(function($mdThemingProvider) {
        $mdThemingProvider.theme('default')
          .primaryPalette('light-green')
          .accentPalette('amber');

      });

  

