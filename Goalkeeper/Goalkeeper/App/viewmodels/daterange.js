define(['plugins/router', 'plugins/http', 'knockout'], function (router, http, ko) {
    var areas = function () {
        var self = this;
        
        this.displayName = ko.observable('Date ranges');
        this.ranges = ko.observableArray();
        this.newRange = {
            Name: ko.observable(),
            StartDate: ko.observable(),
            EndDate: ko.observable()
        };
        this.create = function() {
            http.post('api/dateranges', self.newRange).complete(function (e) {
                self.newRange.Name(undefined);
                self.newRange.StartDate(undefined);
                self.newRange.EndDate(undefined);
                self.activate();
            });
        };
        this.remove = function(r) {
            http.delete('api/dateranges/' + r.Id.replace('/', '-')).complete(function () {
                self.activate();
            });
        };
        this.activate = function() {
            return http.get('api/dateranges').success(function (data) {
                self.ranges([]);
                self.ranges(data);
            });
        };
    };

    return areas;
});