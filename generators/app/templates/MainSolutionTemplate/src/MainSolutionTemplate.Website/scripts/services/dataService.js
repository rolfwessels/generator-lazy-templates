'use strict';

/* authorizationService */

angular.module('webapp.services')
    .service('dataService', ['$log', 'signalrBase', 'authorizationService', '$q','$rootScope',
        function($log, signalrBase, authorizationService, $q,$rootScope) {


            var currentConnection = null;
            var userHub = {};

            /*
             * Private methods
             */
            function createConnection() {
                var connectionDefer = $q.defer();
                console.log("Authenticated:" + authorizationService.isAuthenticate());
                $.connection.hub.url = signalrBase;
                
                var connection = $.hubConnection();
                connection.qs = { "bearer" : authorizationService.currentSession().accessToken };
                userHub = connection.createHubProxy('UserHub');
                
                /*
                 * Register events
                 */
                userHub.on('OnUpdate', function (data) {
                    $rootScope.$emit("userHub.OnUpdate", data);
                });
        

                var start = connection.start();
                
                
                start.done(function() {
                    connectionDefer.resolve(connection);
                });
                start.fail(function(result) {
                    $log.error("signalRFail:" + result.message)
                    connectionDefer.reject(result.message);
                });
                return connectionDefer.promise;
            }

            function defaultUpdate(scope,update,callBack) {
                            if (angular.isArray(callBack)) {
                                
                                scope.$apply(function () {
                                    if (update.UpdateType == 0)  {
                                        callBack.push(update.Value);
                                    }
                                    //delete
                                    else if (update.UpdateType == 2)  {
                                        
                                        angular.forEach(callBack, function(value, key) {
                                           
                                          if (update.Value.Id  == value.Id) {
                                                callBack.splice(key);
                                            }
                                        });
                                        
                                    }
                                    //update
                                    else if (update.UpdateType == 1)  {
                                        angular.forEach(callBack, function(value, key) {
                                          if (update.Value.Id  == value.Id) {
                                                angular.copy(update.Value, value);
                                            }
                                        });
                                        
                                    }
                                });
                                
                            }
                            else {
                                callBack(update);
                            }
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
                    },
                    onUpdate: function(scope,callBack) {
                        var destroy = $rootScope.$on("userHub.OnUpdate",function(onId,update) {
                            defaultUpdate(scope,update,callBack);
                        });
                        scope.$on("$destroy", function () {destroy();});
                        return destroy;
                    }
                }
            };

        }
    ]);
