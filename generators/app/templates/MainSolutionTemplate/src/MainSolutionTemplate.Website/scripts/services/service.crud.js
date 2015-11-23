/* messageService */

angular.module('webapp.directives')
    .service('service.crud', [ '$log', 'service.message', function($log, messageService) {


        var service = function(endpoint) {

            var crud = {
                display: false,
                list: {
                    items: [],
                    count: 0
                },
                currentItem: {}
            };
            
            
            crud.subscribe = function (scope) {
                endpoint.onUpdate(scope, function(callb) {
                    scope.$apply(function () {
                        endpoint.applyUpdateToList(callb, crud.list.items);
                    });
                });
            };

            endpoint.getAllPaged().then(function (data) {
                angular.extend(crud.list, data);
                
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
                endpoint.delete(item.id, crud.currentItem).then(function() {
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

                    endpoint.put(crud.currentItem.id, crud.currentItem).then(function() {

                    }, function(message) {
                        crud.display = true;
                        messageService.error(message);
                    });
                } else {

                    endpoint.post(crud.currentItem).then(function (result) {
                        var found = false;
                        var list = crud.list.items;
                        angular.forEach(list, function (value) {
                            if (result.id == value.id) {
                                angular.copy(result, value);
                                found = true;
                            }
                        });
                        if (!found) {
                            list.push(result);
                        }

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

    }]);
