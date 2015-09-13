/* controller.dashboard */

angular.module('webapp.controllers')
    .controller('controller.dashboard', ['$scope', '$log', 'service.project', 'messageService',
        function ($scope, $log, serviceProject, messageService) {

            $scope.allCounter = [];
            mapData(serviceProject, 'users', '#/user');
            mapData(serviceProject, 'projects', '#/project');
            
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
