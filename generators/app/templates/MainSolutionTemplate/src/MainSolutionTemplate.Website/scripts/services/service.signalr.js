/* service.signalr */

angular.module('webapp.services')
    .service('service.signalr', [
        '$log', 'signalrBase', 'authorizationService', '$q', '$rootScope',
        function($log, signalrBase, authorizationService, $q, $rootScope) {

            var currentConnectionString = null;
            var currentConnectionDefer = null;
            var connection = null;
            var notificationHub = {};


            /* 
             * Service
             */
            var returnService = {
                whenConnected: function() {
                    if (currentConnectionString != authorizationService.currentSession().accessToken) {
                        if (connection) {
                            connection.stop();
                            connection = null;
                        }
                        currentConnectionString = authorizationService.currentSession().accessToken;
                        currentConnectionDefer = createConnection();
                    }
                    return currentConnectionDefer;
                },
                getNotificationHub : function() {
                    return notificationHub;
                }
            };

            /*
             * Private methods
             */
            function createConnection() {
                
                var connectionDefer = $q.defer();
                console.log("Authenticated:" + signalrBase);
                connection = $.hubConnection(signalrBase);
                connection.qs = { "bearer": authorizationService.currentSession().accessToken };

                /*
                 * Register events
                 */
                notificationHub = connection.createHubProxy('NotificationHub');
                notificationHub.on('OnUpdate', function (data) {
                    $rootScope.$emit("NotificationHub.OnUpdate", data);
                });

                var start = connection.start();

                start.done(function() {
                    connectionDefer.resolve(connection);
                });
                start.fail(function(result) {
                    connectionDefer.reject(result.message);
                });
                return connectionDefer.promise;
            }

            return returnService;

        }
    ]);