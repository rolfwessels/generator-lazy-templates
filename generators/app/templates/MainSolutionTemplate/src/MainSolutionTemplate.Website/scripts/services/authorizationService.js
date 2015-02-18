'use strict';

/* authorizationService */

angular.module('webapp.services')
    .service('authorizationService', ['$log', '$http', 'localStorageService', 'tokenUrl', '$q', '$location', '$timeout',
        function($log, $http, localStorageService, tokenUrl, $q, $location, $timeout) {
            var clientId = "asdf"

            return {
                authorize: function(email, password) {
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
                            deferred.resolve(userData);
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
                }
            };

        }
    ]);
