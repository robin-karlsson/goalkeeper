define(['durandal/plugins/router','durandal/http'], function (router, http) {
    var areas = function () {
        var self = this;
        
        this.displayName = ko.observable('Goal');
        this.description = 'Loading goal description...';
        this.activities = ko.observableArray();
        
        this.activate = function(goalId) {
            return http.get('api/goals/' + goalId.splat + '/activities').success(function (data) {
                self.activities(data.activities);
                self.displayName(data.goal.Name);
            });
        };
    };

    return areas;
});