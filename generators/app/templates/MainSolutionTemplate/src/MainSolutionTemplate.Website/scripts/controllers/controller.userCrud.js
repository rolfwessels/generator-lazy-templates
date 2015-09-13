/* controller.user */

angular.module('webapp.controllers')
    .controller('controller.userCrud', ['$scope', 'service.crud', 'service.user',
        function($scope, serviceCrud, serviceUser) {   

            $scope.crudUser = serviceCrud(serviceUser);

        }
    ]);
