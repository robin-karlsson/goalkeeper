define(['durandal/http'], function (http) {
    var areas = function () {
        var self = this;
        
        this.displayName = 'Areas';
        this.description = 'Organize your organizational goals and keep daily track of the progress.';
        this.areas = ko.observableArray();

        this.loadAreas = function() {
            http.get('api/areas').success(function (data) {
                self.areas(data);
            });
        };

        this.loadAreas();
    };

    return areas;
});