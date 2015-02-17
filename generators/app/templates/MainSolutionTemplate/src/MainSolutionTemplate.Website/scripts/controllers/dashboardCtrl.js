'use strict';

/* dashboardCtrl */

angular.module('webapp.controllers')
  .controller('dashboardCtrl', ['$scope', '$mdSidenav', '$mdBottomSheet', '$log',
		function ($scope, $mdSidenav, $mdBottomSheet, $log) {
    
    $scope.toggleSidenav = toggleSideNav;
    $scope.showActions   = showActions;

  	/**
		 * Hide or Show the sideNav area
		 * @param menuId
		 */

  	function toggleSideNav(name) {
  		$mdSidenav(name).toggle();
  	}

  	/**
		 * Select the current avatars
		 * @param menuId
		 */

  	function selectAvatar(avatar) {
  		$scope.selected = angular.isNumber(avatar) ? $scope.avatars[avatar] : avatar;
  		$scope.toggleSidenav('left');
  	}

  	function showActions($event) {

  		$mdBottomSheet.show({
  			parent: angular.element(document.getElementById('content')),
  			template: '<md-bottom-sheet class="md-list md-has-header">' +
					'<md-subheader>Avatar Actions</md-subheader>' +
					'<md-list>' +
					'<md-item ng-repeat="item in vm.items">' +
					'<md-button ng-click="vm.performAction(item)">{{item.name}}</md-button>' +
					'</md-item>' +
					'</md-list>' +
					'</md-bottom-sheet>',
  			bindToController: true,
  			controllerAs: "vm",
  			controller: ['$mdBottomSheet', AvatarSheetController],
  			targetEvent: $event
  		}).then(function (clickedItem) {
  			$log.debug(clickedItem.name + ' clicked!');
  		});

  		/**
			 * Bottom Sheet controller for the Avatar Actions
			 */

  		function AvatarSheetController($mdBottomSheet) {
  			this.items = [
					{ name: 'Share', icon: 'share' },
					{ name: 'Copy', icon: 'copy' },
					{ name: 'Impersonate', icon: 'impersonate' },
					{ name: 'Singalong', icon: 'singalong' },
  			];
  			this.performAction = function (action) {
  				$mdBottomSheet.hide(action);
  			};
  		}
  	}

  }]);
