/* messageService */

angular.module('webapp.directives')
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
                        var object = $scope.$new();
                        var iChildScope = object;
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
