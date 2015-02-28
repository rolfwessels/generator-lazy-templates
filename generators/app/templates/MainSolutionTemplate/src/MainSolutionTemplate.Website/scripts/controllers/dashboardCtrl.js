/* dashboardCtrl */

angular.module('webapp.controllers')
    .controller('dashboardCtrl', ['$scope', '$mdSidenav', '$mdBottomSheet', '$log', 'dataService', 'messageService',
        function($scope, $mdSidenav, $mdBottomSheet, $log, dataService, messageService) {

            $scope.showActions = showActions;

            $scope.users = [];
            dataService.whenConnected().then(function() {
                dataService.users.getAll().then(function(data) {
                    angular.forEach(data, function(value, key) {
                      $scope.users.push(value);
                    });
                    
                },function(error) {
                  $log.error(error);
                });
                dataService.users.onUpdate($scope, $scope.users);
            }, messageService.error,messageService.debug);


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
                    var data =  null;
                    $log.debug(clickedItem.name + ' clicked!');
                    if (clickedItem.name == "Add") {
                            dataService.users.post({ Name : ("Sample "+new Date()), Email : "Sample" });
                    }
                    else if (clickedItem.name == "Delete") {
                        
                        angular.forEach($scope.users, function(value, key) {
                          data = value;
                        });
                        if (data !== null) {
                            dataService.users.delete(data.Id);
                        }
                    }
                    else if (clickedItem.name == "Update") {
                        
                        angular.forEach($scope.users, function(value, key) {
                          data = value;
                        });
                        if (data !== null) {
                            dataService.users.put(data.Id,{ Name : ("Sample "+new Date()), Email : "Sample312" });
                        }
                    }
                });

                /**
                 * Bottom Sheet controller for the Avatar Actions
                 */
                function AvatarSheetController($mdBottomSheet) {
                    this.items = [{
                        name: 'Add',
                        icon: 'add'
                    }, {  
                        name: 'Update',
                        icon: 'update'
                    }, {
                        name: 'Delete',
                        icon: 'delete'
                    }];
                    this.performAction = function(action) {

                        $mdBottomSheet.hide(action);
                    };
                }
            }

        }
    ]);
