﻿define(['durandal/http'], function (http) {
    var areas = function () {
        var self = this;
        
        this.displayName = 'Goals';
        this.description = 'All your current set goals.';
        this.goals = ko.observableArray();

        this.activate = function () {
            return http.get('api/goals').success(function (data) {
                self.goals(data);
            });
        };
    };

    return areas;
});