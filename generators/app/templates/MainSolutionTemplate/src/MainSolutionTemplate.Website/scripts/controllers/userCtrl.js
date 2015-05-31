/* userCtrl */

angular.module('webapp.controllers')
    .controller('userCtrl', ['$scope', '$log', 'dataService', 'messageService', 'crudService',
        function($scope, $log, dataService, messageService, crudService) {
   
     
            $scope.crudUser = crudService('users');

            

        }
    ]);
