/* authorizationService */

angular.module('webapp.services')
	.service('dataService', ['$log', 'signalrBase', 'authorizationService', '$q', '$rootScope', 'endPointService',
		function ($log, signalrBase, authorizationService, $q, $rootScope, endPointService) {

			var currentConnectionString = null;
			var currentConnectionDefer = null;
			var connection = null;
			var userHub = {};

			

			/* 
             * Service
             */
			var returnService =  {
				whenConnected: function() {
					if (currentConnectionString != authorizationService.currentSession().accessToken) {
						if (connection) {							
							connection.stop();
							connection = null;
						}
						currentConnectionDefer = createConnection();
					}
					return currentConnectionDefer;
				},
				users: endPointService('user', userHub),
				projects: endPointService('project', userHub)
			};

		    /*
             * Private methods
             */

			function createConnection() {
			    currentConnectionString = authorizationService.currentSession().accessToken;
			    var connectionDefer = $q.defer();
			    console.log("Authenticated:" + authorizationService.isAuthenticate());
			    $.connection.hub.url = signalrBase;

			    connection = $.hubConnection();
			    connection.qs = { "bearer": authorizationService.currentSession().accessToken };
			    userHub = connection.createHubProxy('UserHub');

			    /*
                 * Register events
                 */
			    userHub.on('OnUpdate', function (data) {
			        $rootScope.$emit("userHub.OnUpdate", data);
			    });


			    var start = connection.start();


			    start.done(function () {
			        connectionDefer.resolve(connection);
			    });
			    start.fail(function (result) {
			        $log.error("signalRFail:" + result.message);
			        connectionDefer.reject(result.message);
			    });
			    return connectionDefer.promise;
			}

			

		    return returnService;

		}
	]);