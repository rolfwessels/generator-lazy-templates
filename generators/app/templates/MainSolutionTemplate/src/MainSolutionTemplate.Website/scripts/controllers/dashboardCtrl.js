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
