define(['plugins/router', 'plugins/http'], function (router, http) {
    var areas = function () {
        var self = this;
        
        this.displayName = ko.observable('Activity');
        this.description = ko.observable('Loading activity description...');
        this.goalId = ko.observable();
        this.activityId = ko.observable();

        this.delete = function() {
            http.delete('api/activities/' + self.goalId()).complete(function() {
                router.navigate('goals/' + self.goalId());
            });
        };
        
        this.activate = function(activityId) {
            return http.get('api/activities/' + activityId).success(function (data) {
                self.displayName(data.Title);
                self.description(data.Abstract);
                self.goalId(data.GoalId);
            });
        };
    };

    return areas;
});