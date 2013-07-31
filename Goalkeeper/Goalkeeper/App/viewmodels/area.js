define(['plugins/router', 'plugins/http', 'knockout'], function (router, http, ko) {
    var areas = function () {
        var self = this;
        
        this.displayName = ko.observable('Area');
        this.description = 'Loading area description...';
        this.goals = ko.observableArray();
        
        this.activate = function(areaId) {
            return http.get('api/areas/' + areaId + '/goals/' + router.daterange.Id.replace('/','-')).success(function (data) {
                self.goals(data.goals);
                self.displayName(data.area.Name);
            });
        };
    };

    return areas;
});