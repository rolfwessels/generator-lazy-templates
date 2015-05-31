/* Filters */

angular.module('webapp.filters', []);

/* Services */

angular.module('webapp.services', ['LocalStorageModule']);

/* Controllers */

angular.module('webapp.controllers', []);
angular
    .module('webapp', [
		'angular-loading-bar',
        'ui.materialize',
        'webapp.routes',
        'webapp.controllers',
        'webapp.directives',
        'webapp.filters',
        'webapp.services'
    ])
    .value('tokenUrl', 'http://localhost:8081/token')
    .value('apiUrlBase', 'http://localhost:8081/api')
    .value('signalrBase', 'http://localhost:8081/signalr')    
    .config(function() {
        $(".button-collapse").sideNav({
          menuWidth: 300, // Default is 240
          edge: 'left', // Choose the horizontal origin
          closeOnClick: true // Closes side-nav on <a> clicks, useful for Angular/Meteor
        });
    })
    .config(['cfpLoadingBarProvider', function(cfpLoadingBarProvider) {
      cfpLoadingBarProvider.includeSpinner = false;
    }])
    .run(['authorizationService', function(authorizationService) {
      authorizationService.isAuthenticatedOrRedirect();
    }]);

/* routes */

angular
	.module('webapp.routes', ['ngRoute'])
	.config(['$routeProvider', function($routeProvider) {

        $routeProvider
        .when('/', {
              templateUrl: 'views/dashboard.html',
              controller: 'dashboardCtrl'
        })
        .when('/user/:id?', {
            templateUrl: 'views/user.html',
            controller: 'userCtrl'
        })
          
        .when('/login/', {
              templateUrl: 'views/login.html',
              controller: 'loginCtrl'
            })
        .when('/forgotPassword', {
              templateUrl: 'views/forgotPassword.html',
              controller: 'forgotPasswordCtrl'
            })
        .otherwise({ redirectTo: '/' });

      }
    ]);
/* Directives */

angular.module('webapp.directives', []);

/* dashboardCtrl */

angular.module('webapp.controllers')
  .controller('navigationCtrl', ['$scope','$rootScope', 'authorizationService', '$timeout', '$location',
		function ($scope,$rootScope, authorizationService, $timeout, $location) {
    
    
    $scope.logout = logout;
    $scope.navigateHome = navigateHome;
    $scope.login = login;
    $rootScope.$watch("isAuthenticated", function(newValue) {
      if (!newValue) {
          $timeout(function() {

          },500);           
        }
      }
    );
      
    function navigateHome() {
      $location.path("/");
    }

    function login() {
      $location.path("/login");
    }

    function logout() {
      authorizationService.logout();
    }
  }]);

/* dashboardCtrl */

angular.module('webapp.controllers')
    .controller('loginCtrl', ['$scope', '$log', 'messageService', 'authorizationService','$location',
        function($scope, $log, messageService, authorizationService,$location) {

            /*
             * Scope
             */
            var  currentUser = authorizationService.currentSession();
            $scope.model = {
                email: currentUser.email||'asdf',
                password: ''
            };
            $scope.login = login;
            $scope.forgotPassword = forgotPassword;

            /*
             * Functions
             */
            function login() {
                
                var authenticate = authorizationService.authenticate($scope.model.email, $scope.model.password);
                authenticate.then(function() {
                    authorizationService.continueToPage();
                }, function(message) {
                	 $scope.model.password = "";
                    messageService.error(message||'Invalid username or password.');
                },messageService.info);

            }

            function forgotPassword() {
	            $location.path('forgotPassword');
            }

        }
    ]);

/* dashboardCtrl */

angular.module('webapp.controllers')
    .controller('dashboardCtrl', ['$scope',  '$log', 'dataService', 'messageService',
        function ($scope, $log, dataService, messageService) {

            $scope.allCounter = [];

            mapData(dataService.users, 'users','#/user');
            mapData(dataService.projects, 'projects', '#/project');
            
            function mapData(service,onScope,link) {
                var counter = { name: onScope, items: [], count: 0 , link : link};
                $scope.allCounter.push(counter);
                service.getAllPaged('$top=5').then(function (data) {
                    counter.count = data.count;
                    counter.items = data.items;
                }, messageService.error, messageService.debug);

            }
        }
    ]);

/* userCtrl */

angular.module('webapp.controllers')
    .controller('userCtrl', ['$scope', '$log', 'dataService', 'messageService',
        function($scope, $log, dataService, messageService) {
            $scope.users = {
                count: 0
            };
            $scope.add = add;
            $scope.update = update;
            $scope.remove = remove;

            // initialize data
            dataService.whenConnected().then(function() {
                dataService.users.onUpdate($scope, $scope.users);
            }, messageService.error, messageService.debug);
            dataService.users.getAllPaged().then(function(data) {
                $scope.users = data;
            }, function(error) {
                $log.error(error);
            });

            function add(user) {
                    
            }

            function update(user) {

            }

            function remove(user) {

            }

        }
    ]);

/* dashboardCtrl */

angular.module('webapp.controllers')
    .controller('forgotPasswordCtrl', ['$scope', '$log', 'messageService', 'authorizationService','$location',
        function($scope, $log, messageService, authorizationService,$location) {

            /*
             * Scope
             */
			var  currentUser = authorizationService.currentSession();

            $scope.model = { email: currentUser.email };
            $scope.forgotPassword = forgotPassword;
            
            /*
             * Functions
             */

            function forgotPassword() {
               var authenticate = authorizationService.forgotPassword($scope.model.email);
                authenticate.then(function() {
                	messageService.info('Your password has been sent to your email');
                    $location.path("/login");
                },messageService.error);

            }

        }
    ]);

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
					return currentSession.accessToken !== null ;//&&  currentSession.expires < new Date();
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
/* messageService */

angular.module('webapp.services')
    .service('messageService', ['$log',
        function($log) {

            /*
             * Private methods
             */
            var timeSpan = 4000;
            /* 
             * Service
             */
            return {
                info: function(message) {
                    $log.info(message);
                    Materialize.toast(message, timeSpan);
                },
                warn: function(message) {
                    $log.warn(message);
                    Materialize.toast(message, timeSpan);
                },
                error: function(message) {
                    $log.error(message);
                    Materialize.toast(message, timeSpan);
                },
                debug: function(message) {
                    $log.debug(message);
                    Materialize.toast(message, timeSpan);
                }

            };

        }
    ]);

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
/* endPointService */

angular.module('webapp.services')
	.service('endPointService', ['$log', 'signalrBase', 'authorizationService', '$q', '$rootScope', 'apiUrlBase', '$http',
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
		            onUpdate: function(scope, callBack) {
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