/* service.user */

angular.module('webapp.services')
	.service('service.user', ['$log', 'service.endPoint',
		function ($log, serviceEndPoint) {

			/* 
             * Service
             */
		    var userHub = {};
		    var returnService = serviceEndPoint('User', userHub);

		    return returnService;

		}
	]);