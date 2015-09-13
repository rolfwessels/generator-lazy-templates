/* service.signalr */

angular.module('webapp.services')
    .service('service.signalr', [
        '$log', 'signalrBase', 'service.authorization`', '$q', '$rootScope',
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
                getNotificationHub: function() {
                    return notificationHub;
                },
                toCamel: function(o) {
                    var build, key, destKey, value;

                    if (o instanceof Array) {
                        build = [];
                        for (key in o) {
                            value = o[key];

                            if (typeof value === "object") {
                                value = returnService.toCamel(value);
                            }
                            build.push(value);
                        }
                    } else {
                        build = {};
                        for (key in o) {
                            destKey = (key.charAt(0).toLowerCase() + key.slice(1) || key).toString();
                            value = o[key];
                            if (typeof value === "object") {
                                value = returnService.toCamel(value);
                            }
                            build[destKey] = value;
                        }
                    }
                    return build;
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


                var start = connection.start();

                notificationHub.on('OnUpdate', function(typeName, value) {
                    $rootScope.$emit("NotificationHub.OnUpdate", { type: typeName, value: value });
                });

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