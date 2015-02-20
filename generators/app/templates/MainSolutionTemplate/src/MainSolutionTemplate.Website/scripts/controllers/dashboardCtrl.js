'use strict';

/* dashboardCtrl */

angular.module('webapp.controllers')
    .controller('dashboardCtrl', ['$scope', '$mdSidenav', '$mdBottomSheet', '$log', 'dataService', 'messageService',
        function($scope, $mdSidenav, $mdBottomSheet, $log, dataService, messageService) {

            $scope.showActions = showActions;

            $scope.users = [];
            dataService.whenConnected().then(function() {
                dataService.users.getAll().then(function(data) {
                    $scope.users = data;
                },function(error) {
                  $log.error(error);
                })
            }, messageService.error,messageService.debug)

            /**
             * Select the current avatars
             * @param menuId
             */

            function showActions($event) {

                $mdBottomSheet.show({
                    parent: angular.element(document.getElementById('content')),
                    templateUrl: 'views/dashboard_bottom.html',
                   
                    bindToController: true,
                    controllerAs: "vm",
                    controller: ['$mdBottomSheet', AvatarSheetController],
                    targetEvent: $event
                }).then(function(clickedItem) {
                    $log.debug(clickedItem.name + ' clicked!');
                });

                /**
                 * Bottom Sheet controller for the Avatar Actions
                 */
               

                function AvatarSheetController($mdBottomSheet) {
                    this.items = [{
                        name: 'Share',
                        icon: 'share'
                    }, {
                        name: 'Copy',
                        icon: 'copy'
                    }, {
                        name: 'Impersonate',
                        icon: 'impersonate'
                    }, {
                        name: 'Singalong',
                        icon: 'singalong'
                    }, ];
                    this.performAction = function(action) {
                        $mdBottomSheet.hide(action);
                    };
                }
            }

        }
    ]);
