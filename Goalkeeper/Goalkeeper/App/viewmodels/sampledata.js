define(['durandal/http','knockout'], function (http,ko) {
    var sampleData = function () {
        var self = this;

        this.displayName = 'Sample Data';
        this.description = 'Push the button to create some sample data.';
        this.sampleDataCreated = ko.observable(false);
        this.createSampleData = function() {
            http.post('api/sampledata','CompanyA').success(function(data) {
                self.sampleDataCreated(true);
            });
        };
    };

    return sampleData;
});