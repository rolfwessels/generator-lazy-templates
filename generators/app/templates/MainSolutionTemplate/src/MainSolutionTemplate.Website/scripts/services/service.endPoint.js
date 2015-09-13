/* service.endPoint */

angular.module('webapp.services')
	.service('service.endPoint', ['$log', 'signalrBase', 'service.authorization`', '$q', '$rootScope', 'apiUrlBase', '$http',
		function ($log, signalrBase, authorizationService, $q, $rootScope, apiUrlBase, $http) {

			/* 
             * Service
             */
		    var returnService = function (basePath, userHub) {

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
		            onUpdate: function (scope, callBack) {
		                userHub.invoke('SubscribeToUpdates');

		                var destroy = $rootScope.$on("userHub.OnUpdate", function(onId, update) {
		                    defaultUpdate(scope, update, callBack);
		                });
		                scope.$on("$destroy", function() {
		                    userHub.invoke('UnsubscribeFromUpdates');
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
			            if (update.UpdateType === 0) {
			                callBack.push(update.Value);
			            }
			                //delete
			            else if (update.UpdateType == 2) {

			                angular.forEach(callBack, function (value, key) {
			                    if (update.Value.Id == value.Id) {
			                        callBack.splice(key);
			                    }
			                });

			            }
			                //update
			            else if (update.UpdateType == 1) {
			                angular.forEach(callBack, function (value) {
			                    if (update.Value.Id == value.Id) {
			                        angular.copy(update.Value, value);
			                    }
			                });

			            }
			        });

			    } else {
			        callBack(update);
			    }
			}

		    return returnService;

		}
	]);