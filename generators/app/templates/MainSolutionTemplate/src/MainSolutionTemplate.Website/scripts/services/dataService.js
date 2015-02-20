'use strict';

/* authorizationService */

angular.module('webapp.services')
    .service('dataService', ['$log', 'signalrBase', 'authorizationService', '$q',
        function($log, signalrBase, authorizationService, $q) {


            var currentConnection = null;
            var userHub = {};

            /*
             * Private methods
             */
            function createConnection() {
                var connectionDefer = $q.defer();
                console.log("Authenticated:" + authorizationService.isAuthenticate());
                $.connection.hub.url = signalrBase;
                $.connection.hub.qs = { "bearer" : authorizationService.currentSession().accessToken };
                
                var connection = $.hubConnection();

                userHub = connection.createHubProxy('UserHub');

                var start = connection.start();
                connectionDefer.notify("connect to " + signalrBase + " with token " + $.connection.hub.qs.bearer );
                start.done(function() {
                    connectionDefer.resolve(connection);
                });
                start.fail(function(result) {
                    $log.error("signalRFail:" + result.message)
                    connectionDefer.reject(result.message);
                });
                return connectionDefer.promise;
            }

            /* 
             * Service
             */
            return {
                whenConnected: function() {
                    if (currentConnection == null) {
                        currentConnection = createConnection();
                    }
                    return currentConnection;
                },
                users: {
                    getAll: function() {
                        return userHub.invoke('Get');
                    },
                    get: function(id) {
                        return userHub.invoke('Get', id);
                    },
                    post: function(user) {
                        return userHub.invoke('Post', user);
                    },
                    put: function(id, user) {
                        return userHub.invoke('Put', id, user);
                    },
                    delete: function(id) {
                        return userHub.invoke('Delete', id);
                    }
                }
            };

        }
    ]);
