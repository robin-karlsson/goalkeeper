define(['plugins/http', 'durandal/system', 'knockout'], function (http, system, ko) {
    var areas = function () {
        var self = this;
        
        this.displayName = 'Areas';
        this.description = 'Organize your organizational goals and keep daily track of the progress.';
        this.areas = ko.observableArray();

        this.newArea = ko.observable();
        this.statusMessage = ko.observable();

        this.cancelAddNew = function() {
            self.newArea(undefined);
        };

        this.addNewArea = function() {
            self.newArea({ name: ko.observable() });
        };

        this.saveNewArea = function() {
            http.post('api/areas', self.newArea()).success(function() {

            });
        };

        this.activate = function() {
            return http.get('api/areas').success(function (data) {
                self.areas(data);
            });
        };
    };

    var vm = new areas();
    vm.activate();

    return vm;
});