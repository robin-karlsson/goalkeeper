define(['durandal/plugins/router','durandal/http'], function (router, http) {
    var areas = function () {
        var self = this;
        
        this.displayName = ko.observable('Goal');
        this.description = 'Loading goal description...';
        this.activities = ko.observableArray();

        this.findActivitiesInState = function(stateToFind) {
            var a = self.activities();
            var result = new Array();
            ko.utils.arrayForEach(a, function (activity) {
                if (activity.ActivityState === stateToFind) {
                    result.push(activity);
                }
            });
            return result;
        };

        this.completedActivities = ko.computed(function() {
            return self.findActivitiesInState(2);
        });

        this.inProgressActivities = ko.computed(function () {
            return self.findActivitiesInState(1);
        });

        this.notStartedActivities = ko.computed(function () {
            return self.findActivitiesInState(0);
        });

        this.activate = function(goalId) {
            return http.get('api/goals/' + goalId.splat + '/activities').success(function (data) {
                self.activities(data.activities);
                self.displayName(data.goal.Name);
            });
        };
    };

    return areas;
});