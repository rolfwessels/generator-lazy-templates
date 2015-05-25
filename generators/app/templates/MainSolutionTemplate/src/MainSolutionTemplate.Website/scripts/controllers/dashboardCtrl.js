/* dashboardCtrl */

angular.module('webapp.controllers')
    .controller('dashboardCtrl', ['$scope',  '$log', 'dataService', 'messageService',
        function($scope, $log, dataService, messageService) {
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

        }
    ]);
