/* service.endPoint */

angular.module('webapp.services')
	.service('service.endPoint', ['$log', 'service.signalr', 'service.authorization`', '$q', '$rootScope', 'apiUrlBase', '$http',
		function ($log, serviceSignalr, authorizationService, $q, $rootScope, apiUrlBase, $http) {

			/* 
             * Service
             */
		    var returnService = function (basePath) {

		        return {
		            getAll: function (filter) {
		                return httpCall('GET', pathCombine(apiUrlBase, basePath) + "?" + filter);
		            },
		            getAllPaged: function (filter) {
		                return httpCall('GET', pathCombine(apiUrlBase, basePath) + "?" + filter + '&$inlinecount=allpages');
		            },
		            getDetailAll: function (filter) {
		                return httpCall('GET', pathCombine(apiUrlBase, basePath, 'Detail') + "?" + filter);
		            },
		            getDetailAllPaged: function (filter) {
		                return httpCall('GET', pathCombine(apiUrlBase, basePath, 'Detail') + "?" + filter + '&$inlinecount=allpages');
		            },
		            get: function(id) {
		                return httpCall('GET', pathCombine(apiUrlBase, basePath,id));
		            },
		            post: function(user) {
		                return httpCall('POST', pathCombine(apiUrlBase, basePath),user);
		            },
		            put: function(id, user) {
		                return httpCall('PUT', pathCombine(apiUrlBase, basePath,id), user);
		            },
		            delete: function (id) {
		                return httpCall('DELETE', pathCombine(apiUrlBase, basePath, id));
		            },
		            applyUpdateToList: applyUpdateToList,
		            toCamel: serviceSignalr.toCamel,
		            onUpdate: function (scope, callBack) {
		                
		                var notificationHub = null;
		                serviceSignalr.whenConnected().then(function () {
		                    notificationHub = serviceSignalr.getNotificationHub();
		                    notificationHub.invoke('subscribeToUpdates', basePath);
		                });
		                var destroy = $rootScope.$on("NotificationHub.OnUpdate", function (onId, update) {
		                    if (update.type == basePath) {
		                        var camel = serviceSignalr.toCamel(update.value);
		                        defaultUpdate(scope, camel, callBack);
		                    }
		                });
		                scope.$on("$destroy", function () {
		                    notificationHub.invoke('UnsubscribeFromUpdates', basePath);
		                    destroy();
		                });
		                return destroy;
		            }
		        };
		    };

		    /* 
             * Private
             */
		    function pathCombine() {
		        var path = arguments[0];
		        for (var i = 1; i < arguments.length; i++) {
		            path += '/' + arguments[i];
		        }
		        return path;
		    }

		    function httpCall(method, url, data) {
		        var deferred = $q.defer();

		        var config = {
		            method: method,
		            url: url,
		            headers: {
		                'Authorization': 'bearer ' + authorizationService.currentSession().accessToken,
		            },
                    data : data
		        };

		        $http(config)
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (data) {
                        $log.error("Request ERROR: ", data);
                        if (data && data.message) {
                            deferred.reject(data.message);
                        } else {
                            deferred.reject('Unable to contact server; please, try again later.');
                        }
                    });
		        return deferred.promise;
		    }

			function defaultUpdate(scope, update, callBack) {
			    if (angular.isArray(callBack)) {
			        scope.$apply(function () {
			            applyUpdateToList(update, callBack);
			        });

			    } else {
			        callBack(update);
			    }
			}

			function applyUpdateToList(update, list) {
			    var found = false;
			    angular.forEach(list, function (value,key) {
			        if (update.value.id == value.id) {
			            if (update.updateType !== 2) {
			                angular.copy(update.value, value);
			            } else {
			                list.splice(key, 1);
			            }
			            found = true;
			        }
			    });
			    if (!found && update.updateType === 0) {
			        list.push(update.value);
			    }
		    }

		    return returnService;

		}
	]);