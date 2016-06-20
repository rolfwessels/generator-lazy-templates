/* controller.projectCrud */

angular.module('webapp.controllers')
    .controller('controller.projectCrud', ['$scope', 'service.crud', 'service.project',
        function ($scope, crudService, serviceProject) {

            $scope.crudProject = crudService(serviceProject);

        }
    ]);

    /* scaffolding [
        {
          "FileName": "route.js",
          "Indexline": ".otherwise",
          "InsertAbove": true,
          "InsertInline": false,
          "Lines": [
            ".when('/project/:id?', {",
            "    templateUrl: 'views/project.html',",
            "    controller: 'controller.projectCrud'",
            "})"
          ]
        },
        {
          "FileName": "controller.dashboard.js",
          "Indexline": "mapData(serviceProject",
          "InsertAbove": true,
          "InsertInline": false,
          "Lines": [
            "mapData(serviceProject, 'projects', '#/project');"
          ]
        },
        {
          "FileName": "controller.dashboard.js",
          "Indexline": ", 'service.project'",
          "InsertAbove": false,
          "InsertInline": true,
          "Lines": [
            ", 'service.project'"
          ]
        },
        {
          "FileName": "controller.dashboard.js",
          "Indexline": ", serviceProject",
          "InsertAbove": false,
          "InsertInline": true,
          "Lines": [
            ", serviceProject"
          ]
        }
    ] scaffolding */
