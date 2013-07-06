define(['durandal/http'], function (http) {
    var sampleData = function () {
        var self = this;

        this.displayName = 'Sample Data';
        this.description = 'Push the button to create some sample data.';
        this.message = ko.observable();
        this.showMessage = ko.computed(function () {
            return (self.message() || '') != '';
        });
        this.sampleDataCreated = ko.observable(false);
        this.createSampleData = function() {
            http.post('api/sampledata','CompanyA').success(function() {
                self.sampleDataCreated(true);
                self.message('Sample data created.');
                setTimeout(function() { self.message(undefined); }, 10000);
            });
        };
        this.clearAllData = function () {
            http.delete('api/sampledata', {}).success(function () {
                self.message('All data deleted.');
                setTimeout(function () { self.message(undefined); }, 10000);
            });
        };
    };

    return sampleData;
});