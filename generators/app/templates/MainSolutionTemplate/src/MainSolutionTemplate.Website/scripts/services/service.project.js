/* service.project */

angular.module('webapp.services')
	.service('service.project', ['$log', 'service.endPoint',
		function ($log, serviceEndPoint) {

			/* 
             * Service
             */
		    var projectHub = {};
		    var returnService = serviceEndPoint('Project', projectHub);

		    return returnService;

		}
	]);