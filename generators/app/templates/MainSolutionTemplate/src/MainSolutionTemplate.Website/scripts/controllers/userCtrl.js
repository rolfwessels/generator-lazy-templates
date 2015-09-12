/* userCtrl */

angular.module('webapp.controllers')
    .controller('userCtrl', ['$scope', 'crudService',
        function($scope, crudService) {   

            $scope.crudUser = crudService('users');

        }
    ]);
