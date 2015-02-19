'use strict';

/* authorizationService */

angular.module('webapp.services')
	.service('dataService', ['$log', 'signalrBase', 'authorizationService', '$q',
		function($log, signalrBase, authorizationService, $q) {
			
			$rootScope.isAuthenticated = authorizationService.isAuthenticate();
			
			$.connection.hub.url = "http://localhost:8081/signalr";
			var entryNotifications = $.connection.dataEntryNotifications;

			
      
		      //Getting the connection object
		      var connection = $.hubConnection();
		      //TODO: Map to global settings connection.url = apiBaseUri;
		      //console.log("hubConnection: ", connection);
		      //Creating proxy
		      this.proxy = connection.createHubProxy('dataEntryNotifications');
		      console.log("connecting to hub: ", apiBaseUri);
		      //Starting connection
		      var start = connection.start();
		      start.done(function () {
		        console.log("connection made");
		      });
		      start.fail(function (result) {
		        console.log('Could not Connect! > ', result.message);
		      });

		      //Publishing an event when server pushes a greeting message
		      this.proxy.on('entryChangeNotification', function (message) {
		        $rootScope.$emit("entryChangeNotification", message);
		      });
		   

			/*
			 * Private methods
			 */
			function saveSession() {
				
			}

			/* 
			 * Service
			 */
			return {
				currentSession : function() {
					return currentSession;
				}
			};

		}
	]);