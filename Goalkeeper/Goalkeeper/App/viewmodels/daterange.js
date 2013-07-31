define(['plugins/router', 'plugins/http', 'knockout'], function (router, http, ko) {
    var areas = function () {
        var self = this;
        
        this.displayName = ko.observable('Date ranges');
        this.ranges = ko.observableArray();
        
        this.activate = function() {
            return http.get('api/dateranges').success(function (data) {
                self.ranges(data);
            });
        };
    };

    return areas;
});