/* controller.projectCrud */

angular.module('webapp.controllers')
    .controller('controller.projectCrud', ['$scope', 'service.crud', 'service.project',
        function ($scope, crudService, serviceProject) {

            $scope.crudProject = crudService(serviceProject);

        }
    ]);
