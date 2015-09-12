/* projectCtrl */

angular.module('webapp.controllers')
    .controller('projectCtrl', ['$scope', 'crudService',
        function($scope, crudService) {   

            $scope.crudProject = crudService('projects');

        }
    ]);
