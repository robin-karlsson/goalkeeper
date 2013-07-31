define(['plugins/router', 'plugins/http', 'durandal/app', 'knockout'], function (router, http, app, ko) {
    var areas = function () {
        var self = this;
        
        this.displayName = ko.observable('Area');
        this.description = 'Loading area description...';
        this.goals = ko.observableArray();
        
        this.activate = function(areaId) {
            return http.get('api/areas/' + areaId + '/goals/' + router.daterange.Id.replace('/','-')).success(function (data) {
                self.goals(data.goals);
                self.displayName(data.area.Name);
                app.title = data.area.Name;
            });
        };
    };

    return areas;
});