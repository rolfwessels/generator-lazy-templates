/* authorizationService */

angular.module('webapp.services')
	.service('dataService', ['$log', 'signalrBase', 'authorizationService', '$q', '$rootScope', 'endPointService',
		function ($log, signalrBase, authorizationService, $q, $rootScope, endPointService) {

			var currentConnectionString = null;
			var currentConnectionDefer = null;
			var connection = null;
			var userHub = {};
			var projectHub = {};
			

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
				projects: endPointService('project', projectHub)
			};

		    /*
             * Private methods
             */

			function createConnection() {
			    currentConnectionString = authorizationService.currentSession().accessToken;
			    var connectionDefer = $q.defer();
			    console.log("Authenticated:" + signalrBase);

			    connection = $.hubConnection(signalrBase);
			    connection.qs = { "bearer": authorizationService.currentSession().accessToken };

			    /*
                * Register events
                */
			    userHub = connection.createHubProxy('UserHub');
			    userHub.on('OnUpdate', function (data) {
			        $rootScope.$emit("userHub.OnUpdate", data);
			    });

			    projectHub = connection.createHubProxy('ProjectHub');
			    projectHub.on('OnUpdate', function (data) {
			        $rootScope.$emit("projectHub.OnUpdate", data);
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