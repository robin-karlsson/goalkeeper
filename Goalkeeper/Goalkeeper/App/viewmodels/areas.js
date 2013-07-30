define(['plugins/http', 'durandal/system', 'knockout'], function (http, system, ko) {
    var areas = function () {
        var self = this;
        
        this.displayName = 'Areas';
        this.description = 'Organize your organizational goals and keep daily track of the progress.';
        this.areas = ko.observableArray();

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