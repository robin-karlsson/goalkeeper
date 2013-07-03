define(['durandal/http','knockout'], function (http,ko) {
    var areas = {
        displayName: 'Areas',
        description: 'Organize your organizational goals and keep daily track of the progress.',
        areas: ko.observableArray()
    };

    http.get('api/areas').success(function(data) {
        areas.areas(data);
    });

    return areas;
});