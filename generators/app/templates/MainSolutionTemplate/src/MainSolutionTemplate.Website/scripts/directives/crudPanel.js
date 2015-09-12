/* messageService */

angular.module('webapp.directives')
    .service('crudService', ['dataService', '$log', 'messageService', function(dataService, $log, messageService) {


        var service = function(section) {

            var crud = {
                display: false,
                list: {
                    items: [],
                    count: 0
                },
                currentItem: {
                    name: "asfd"
                }
            };
            // initialize data
            dataService.whenConnected().then(function() {
                // dataService.listonUpdate(crud, crud.lis;
            }, messageService.error, messageService.debug);
            dataService[section].getAllPaged().then(function(data) {
                crud.list = data;
            }, $log.error);

            crud.showAdd = function() {
                crud.display = true;
                crud.currentItem = {};
            };

            crud.showEdit = function(item) {
                crud.display = true;
                crud.currentItem = item;
            };

            crud.removeItem = function(item) {

                console.log(item);
                dataService[section].delete(item.id, crud.currentItem).then(function() {
                    var index = crud.list.items.indexOf(item);
                    if (index > -1) {
                        crud.list.items.splice(index, 1);
                    }
                }, function(message) {
                    messageService.error(message);
                });
            };

            crud.cancel = function() {
                crud.display = false;
            };

            crud.save = function() {
                crud.display = false;
                if (crud.currentItem.id) {

                    dataService[section].put(crud.currentItem.id, crud.currentItem).then(function() {

                    }, function(message) {
                        crud.display = true;
                        messageService.error(message);
                    });
                } else {

                    dataService[section].post(crud.currentItem).then(function(result) {
                        crud.list.items.push(result);
                    }, function(message) {
                        crud.display = true;
                        messageService.error(message);
                    });
                }
            };

            crud.delete = function() {
                crud.display = false;

            };
            return crud;
        };

        /*
         * Private methods
         */


        return service;

    }])
    .directive('crudPanel', function() {
        return {
            restrict: 'E',
            scope: {
                data: '=data'
            },
            transclude: true,
            templateUrl: 'views/partial/crudPanelPartial.html'
        };
    })
    .directive('crudForm', function() {
        return {
            restrict: 'E',
            scope: {
                data: '=data'
            },
            transclude: true,
            templateUrl: 'views/partial/crudFormPartial.html'
        };
    })
    .directive('transpose', function() {
        return {
            restrict: 'EAC',
            link: function($scope, $element, $attrs, controller, $transclude) {
                if (!$transclude) {
                    throw minErr('ngTransclude')('orphan',
                        'Illegal use of ngTransclude directive in the template! ' +
                        'No parent directive that requires a transclusion found. ' +
                        'Element: {0}',
                        startingTag($element));
                }

                var iScopeType = $attrs['transpose'] || 'sibling';

                switch (iScopeType) {
                    case 'sibling':
                        $transclude(function(clone) {
                            $element.empty();
                            $element.append(clone);
                        });
                        break;
                    case 'parent':
                        $transclude($scope, function(clone) {
                            $element.empty();
                            $element.append(clone);
                        });
                        break;
                    case 'child':
                        var iChildScope = $scope.$new();
                        $transclude(iChildScope, function(clone) {
                            $element.empty();
                            $element.append(clone);
                            $element.on('$destroy', function() {
                                iChildScope.$destroy();
                            });
                        });
                        break;
                }
            }
        }
    });
