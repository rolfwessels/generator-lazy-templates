/* userCtrl */

angular.module('webapp.controllers')
    .controller('userCtrl', ['$scope',  '$log', 'dataService', 'messageService',
        function($scope, $log, dataService, messageService) {
            $scope.users = {count:0};
            $scope.add = add;
            $scope.update = update;
            $scope.remove = remove;

            // initialize data
            dataService.whenConnected().then(function () {
                dataService.users.onUpdate($scope, $scope.users);
            }, messageService.error, messageService.debug);
            dataService.users.getAllPaged().then(function (data) {
                $scope.users = data;
            }, function (error) {
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
