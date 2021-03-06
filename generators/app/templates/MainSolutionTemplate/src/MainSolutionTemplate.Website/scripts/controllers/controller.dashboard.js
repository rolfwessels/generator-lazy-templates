﻿/* controller.dashboard */

angular.module('webapp.controllers')
    .controller('controller.dashboard', ['$scope', '$log', 'service.message', 'service.project', 'service.user',
        function ($scope, $log,  messageService, serviceProject, serviceUser) {

            $scope.allCounter = [];
            mapData(serviceUser, 'users', '#/user');
            mapData(serviceProject, 'projects', '#/project');
            
            function mapData(service,onScope,link) {
                var counter = { name: onScope, items: [], count: 0 , link : link};
                $scope.allCounter.push(counter);
                service.onUpdate($scope, function (call) {
                    $scope.$apply(function() {
                        if (call.updateType == 0) {
                            counter.count += 1;
                        } else if (call.updateType == 2) {
                            counter.count += -1;
                        }
                    });
                });
                update(service, counter);
            }

            function update(service, counter) {
                service.getAllPaged('$top=5').then(function (data) {
                    counter.count = data.count;
                    counter.items = data.items;
                }, messageService.error, messageService.debug);
            }
        }
    ]);
