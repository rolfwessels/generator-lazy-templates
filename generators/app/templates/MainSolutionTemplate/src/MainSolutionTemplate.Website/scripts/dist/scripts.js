angular
    .module('webapp', [
        'ngMaterial',
				'angular-loading-bar',
        'webapp.routes',
        'webapp.controllers',
        'webapp.directives',
        'webapp.filters',
        'webapp.services'
    ])
    .value('tokenUrl', 'http://localhost:8081/token')
    .value('apiUrlBase', 'http://localhost:8081/api')
    .value('signalrBase', 'http://localhost:8081/signalr')    
    .config(function($mdThemingProvider) {
        $mdThemingProvider.theme('default')
            .primaryPalette('light-green')
            .accentPalette('amber');

    })
    .config(['cfpLoadingBarProvider', function(cfpLoadingBarProvider) {
      cfpLoadingBarProvider.includeSpinner = false;
    }])
    .run(['authorizationService', function(authorizationService) {
      authorizationService.isAuthenticatedOrRedirect();
    }]);
;/* Controllers */

angular.module('webapp.controllers', ['ngMaterial']);;/* dashboardCtrl */

angular.module('webapp.controllers')
    .controller('dashboardCtrl', ['$scope', '$mdSidenav', '$mdBottomSheet', '$log', 'dataService', 'messageService',
        function($scope, $mdSidenav, $mdBottomSheet, $log, dataService, messageService) {

            $scope.showActions = showActions;

            $scope.users = [];
            dataService.whenConnected().then(function() {
                dataService.users.getAll().then(function(data) {
                    angular.forEach(data, function(value, key) {
                      $scope.users.push(value);
                    });
                    
                },function(error) {
                  $log.error(error);
                });
                dataService.users.onUpdate($scope, $scope.users);
            }, messageService.error,messageService.debug);

            

            /**
             * Select the current avatars
             * @param menuId
             */

            function showActions($event) {

                $mdBottomSheet.show({
                    parent: angular.element(document.getElementById('content')),
                    templateUrl: 'views/dashboard_bottom.html',
                   
                    bindToController: true,
                    controllerAs: "vm",
                    controller: ['$mdBottomSheet', AvatarSheetController],
                    targetEvent: $event
                }).then(function(clickedItem) {
                    var data =  null;
                    $log.debug(clickedItem.name + ' clicked!');
                    if (clickedItem.name == "Add") {
                        var result = dataService.users.post({ Name : ("Sample "+new Date()), Email : "Sample" });
                        console.log(result);
                        result.fail(messageService.error);

                    }
                    else if (clickedItem.name == "Delete") {
                        
                        angular.forEach($scope.users, function(value, key) {
                          data = value;
                        });
                        if (data !== null) {
                            dataService.users.delete(data.Id);
                        }
                    }
                    else if (clickedItem.name == "Update") {
                        
                        angular.forEach($scope.users, function(value, key) {
                          data = value;
                        });
                        if (data !== null) {
                            dataService.users.put(data.Id,{ Name : ("Sample "+new Date()), Email : "Sample312" });
                        }
                    }
                });

                /**
                 * Bottom Sheet controller for the Avatar Actions
                 */
               

                function AvatarSheetController($mdBottomSheet) {
                    this.items = [{
                        name: 'Add',
                        icon: 'add'
                    }, {  
                        name: 'Update',
                        icon: 'update'
                    }, {
                        name: 'Delete',
                        icon: 'delete'
                    }];
                    this.performAction = function(action) {

                        $mdBottomSheet.hide(action);
                    };
                }
            }

        }
    ]);
;/* dashboardCtrl */

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
;/* dashboardCtrl */

angular.module('webapp.controllers')
    .controller('loginCtrl', ['$scope', '$log', 'messageService', 'authorizationService','$location',
        function($scope, $log, messageService, authorizationService,$location) {

            /*
             * Scope
             */
            var  currentUser = authorizationService.currentSession();
            $scope.model = {
                email: currentUser.email,
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
;/* dashboardCtrl */

angular.module('webapp.controllers')
  .controller('navigationCtrl', ['$scope','$rootScope', '$mdSidenav', 'authorizationService', '$timeout', '$location',
		function ($scope,$rootScope, $mdSidenav, authorizationService, $timeout, $location) {
    
    $scope.toggleSidenav = toggleSideNav;
    $scope.logout = logout;
    $rootScope.$watch("isAuthenticated", function(newValue) {
      if (!newValue) {
          $timeout(function() {
          	$mdSidenav('left').close();
          },500);           
        }
      }
    );
    

  	/**
		 * Hide or Show the sideNav area
		 * @param menuId
		 */

  	function toggleSideNav(name) {
  		$mdSidenav(name).toggle();
  	}


    function navigateHome() {
      $location.path("/");
    }


    function logout(name) {
      authorizationService.logout();
      $mdSidenav(name).toggle();
    }


  }]);
;/* Directives */

angular.module('webapp.directives', []);
;/* Filters */

angular.module('webapp.filters', []);
;/* routes */

angular
	.module('webapp.routes', ['ngRoute'])
	.config(['$routeProvider', function($routeProvider) {

        $routeProvider
        .when('/', {
              templateUrl: 'views/dashboard.html',
              controller: 'dashboardCtrl'
            })
        .when('/login', {
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
;/* Services */

angular.module('webapp.services', ['LocalStorageModule']);
;/* authorizationService */

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
	]);;/* authorizationService */

angular.module('webapp.services')
	.service('dataService', ['$log', 'signalrBase', 'authorizationService', '$q', '$rootScope',
		function($log, signalrBase, authorizationService, $q, $rootScope) {

			var currentConnectionString = null;
			var currentConnectionDefer = null;
			var connection = null;
			var userHub = {};

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
				userHub.on('OnUpdate', function(data) {
					$rootScope.$emit("userHub.OnUpdate", data);
				});


				var start = connection.start();


				start.done(function() {
					connectionDefer.resolve(connection);
				});
				start.fail(function(result) {
					$log.error("signalRFail:" + result.message);
					connectionDefer.reject(result.message);
				});
				return connectionDefer.promise;
			}

			function defaultUpdate(scope, update, callBack) {
				if (angular.isArray(callBack)) {

					scope.$apply(function() {
						if (update.UpdateType === 0) {
							callBack.push(update.Value);
						}
							//delete
						else if (update.UpdateType == 2) {

							angular.forEach(callBack, function(value, key) {

								if (update.Value.Id == value.Id) {
									callBack.splice(key);
								}
							});

						}
							//update
						else if (update.UpdateType == 1) {
							angular.forEach(callBack, function(value, key) {
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

			/* 
             * Service
             */
			return {
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
					onUpdate: function(scope, callBack) {
						userHub.invoke('SubscribeToUpdates');
						
						var destroy = $rootScope.$on("userHub.OnUpdate", function(onId, update) {
							defaultUpdate(scope, update, callBack);
						});
						scope.$on("$destroy", function () {
						    userHub.invoke('UnsubscribeFromUpdates');
						    destroy();
						});
						return destroy;
					}
				}
			};

		}
	]);;/* authorizationService */

angular.module('webapp.services')
    .service('messageService', ['$log', '$mdToast',
        function($log, $mdToast) {

            var toastPosition = "top left right";

            /*
             * Private methods
             */

            /* 
             * Service
             */
            return {
                info: function(message) {
                    $log.info(message);
                    $mdToast.show(
                        $mdToast.simple()
                        .content(message)
                        .position(toastPosition)
                        .hideDelay(3000)
                    );
                },
                warn: function(message) {
                    $log.warn(message);
                    $mdToast.show(
                        $mdToast.simple()
                        .content(message)
                        .position(toastPosition)
                        .hideDelay(3000)
                    );
                },
                error: function(message) {
                    $log.error(message);
                    $mdToast.show(
                        $mdToast.simple()
                        .content(message)
                        .position(toastPosition)
                        .hideDelay(3000)
                    );
                },
                debug: function(message) {
                    $log.debug(message);

                }

            };

        }
    ]);
