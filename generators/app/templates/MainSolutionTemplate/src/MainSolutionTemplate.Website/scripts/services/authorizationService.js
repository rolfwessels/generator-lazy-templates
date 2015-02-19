'use strict';

/* authorizationService */

angular.module('webapp.services')
	.service('authorizationService', ['$log', '$http', 'localStorageService', 'tokenUrl', 'apiUrlBase', '$q', '$location', '$rootScope',
		function($log, $http, localStorageService, tokenUrl, apiUrlBase, $q, $location,$rootScope) {
			
			var clientId = "MainSolutionTemplate.Api";
			var currentSession = localStorageService.get('token') || {};
			var directTo = "/";
			var pathsToIgnore = ["/login",'/forgotPassword'];
			$rootScope.isAuthenticated = isAuthenticate();
			
			/*
			 * Private methods
			 */
			function saveSession() {
				localStorageService.set('token', currentSession);
				$rootScope.isAuthenticated = isAuthenticate();
			}

			function isAuthenticate() {					
					return currentSession.accessToken != null ;//&&  currentSession.expires < new Date();
			}

			/* 
			 * Service
			 */
			return {
				currentSession : function() {
					return currentSession;
				},
				continueToPage : function() {
					$location.path(directTo);
				},
				isAuthenticatedOrRedirect: function() {
					if (pathsToIgnore.indexOf($location.path()) >= 0) {
						return;
					}
					if (!this.isAuthenticate()) {
						directTo = $location.path();
						console.log(directTo);
						$location.path("/login");
					}
				},
				
				isAuthenticate: isAuthenticate,


				logout: function() {
					currentSession.accessToken = null;
					saveSession();
					$location.path("/login");
				},
				authenticate: function(email, password) {
					var deferred = $q.defer();

					var config = {
						method: 'POST',
						url: tokenUrl,
						headers: {
							'Content-Type': 'application/x-www-form-urlencoded',
						},
						data: 'client_id=' + clientId + '&grant_type=password&username=' + email + '&password=' + password,
					};

					$http(config)
						.success(function(data) {
							currentSession.expires = Date.parse(data['.expires']);
							currentSession.issued =  Date.parse(data['.issued']);
							currentSession.accessToken = data.access_token;
							currentSession.displayName = data.displayName;
							currentSession.permissions = data.permissions;
							currentSession.email = data.userName;
							saveSession();							
							deferred.resolve(data);
						})
						.error(function(data) {
							$log.error("Auth ERROR: ", data);
							if (data && data.error_description) {
								deferred.reject(data.error_description);
							} else {
								deferred.reject('Unable to contact server; please, try again later.');
							}
						});
					return deferred.promise;
				},
				forgotPassword : function (email) {
					var deferred = $q.defer();

					$http.get(apiUrlBase+'/user/forgotPassword/'+email)
						.success(function(data) {		
							deferred.resolve(data);
						})
						.error(function(data) {
							if (data && data.error_description) {
								deferred.reject(data.error_description);
							} else {
								deferred.reject('Unable to contact server; please, try again later.');
							}
						});
					return deferred.promise;

				}
			};

		}
	]);