define(['plugins/router', 'plugins/http'], function (router, http) {
    var areas = function () {
        var self = this;
        
        this.displayName = ko.observable('Activity');
        this.description = ko.observable('Loading activity description...');
        
        this.activate = function(activityId) {
            return http.get('api/activities/' + activityId).success(function (data) {
                self.displayName(data.Title);
                self.description(data.Abstract);
            });
        };
    };

    return areas;
});