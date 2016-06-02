/**
 * Created by Radu  Pop on 5/29/2016.
 */
(function(){
    'use strict';

    angular
        .module('app.examples.dashboards')
        .controller('DashboardGeneralController', DashboardGeneralController);


    /* @ngInject */
    function DashboardGeneralController($scope, $rootScope, $mdToast, $mdDialog, GoalsService){
        var vm = this;

        vm.dateRange = {
            start: moment().startOf('year'),
            end: moment().endOf('year')
        };

        vm.query = {
            goals: 'date',
            limit: 5,
            page: 1
        };

        vm.goalsResult = {
            goals: [],
            totalGoals: 0
        };

        vm.finishedGoals = 0;
        vm.openGoals = 0;

        vm.openGoal = openGoal;

        function openGoal(goal, $event) {
            $mdDialog.show({
                controller: 'GoalDialogController',
                controllerAs: 'vm',
                templateUrl: 'app/examples/dashboards/general/goal-dialog.tmpl.html',
                locals: {
                    goal: goal
                },
                targetEvent: $event
            }).then(function() {

                // update new data
                getGoals();

                $mdToast.show(
                    $mdToast.simple()
                        .content('Goals is updating...')
                        .position('bottom right')
                        .hideDelay(2000)
                );
            });
        }

        function getGoals(){
            GoalsService.getGoals($rootScope.globals.currentUser.userId, vm.dateRange.start.toISOString(),vm.dateRange.end.toISOString(), handleSuccess, handleFailed);
        }

        function handleSuccess(result) {
            var json = result.data.goals;

            for (var i = 0; i < json.length; i++)
            {
                if (json[i].GoalStatus === 'Open') {
                    vm.openGoals++;
                }
                if (json[i].GoalStatus === 'Done') {
                    vm.finishedGoals++;
                }
            }

            vm.goalsResult.goals = result.data.goals;
            vm.goalsResult.totalGoals = result.data.goalsCount;
        }

        function handleFailed(){
            $mdToast.show(
                $mdToast.simple()
                    .content("Ops something gos wrong with api")
            )
        }


        $scope.$on('goalsChangeDate', function(event, $event) {
            $mdDialog.show({
                controller: 'GoalsDateChangeDialogController',
                controllerAs: 'vm',
                templateUrl: 'app/examples/dashboards/general/date-change-dialog.tmpl.html',
                locals: {
                    range: vm.dateRange
                },
                targetEvent: $event
            })
                .then(function() {
                    // update new data
                    getGoals();

                    // pop a toast
                    $mdToast.show(
                        $mdToast.simple()
                            .content('Goals is updating...')
                            .position('bottom right')
                            .hideDelay(2000)
                    );
                });
        });

        /// init
        getGoals();

    }

})();