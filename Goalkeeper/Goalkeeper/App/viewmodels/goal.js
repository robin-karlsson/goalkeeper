define(['plugins/router', 'plugins/http'], function (router, http) {
    var areas = function() {
        var self = this;

        this.displayName = ko.observable('Goal');
        this.description = 'Loading goal description...';
        this.activities = ko.observableArray();

        this.findActivitiesInState = function(stateToFind) {
            var a = self.activities();
            var result = new Array();
            ko.utils.arrayForEach(a, function(activity) {
                if (activity.ActivityState === stateToFind) {
                    result.push(activity);
                }
            });
            return result;
        };

        this.goalId = ko.observable();
        this.voteCount = ko.observable(0);
        this.voteEnabled = ko.observable(true);
        this.voteText = ko.observable('Vote now!');

        this.completedActivities = ko.computed(function() {
            return self.findActivitiesInState(2);
        });

        this.inProgressActivities = ko.computed(function() {
            return self.findActivitiesInState(1);
        });

        this.notStartedActivities = ko.computed(function() {
            return self.findActivitiesInState(0);
        });

        this.suggestions = ko.observableArray();
        this.suggestion = ko.observable();
        
        this.suggestionEnabled = ko.computed(function() {
            return (self.suggestion() || '') != '';
        });

        this.sendSuggestion = function() {
            http.post('api/activitysuggestions', { description: self.suggestion(), goalId: self.goalId(), suggestionState: 'Open' }).complete(function (data) {
                self.suggestion('');
                self.suggestions.push(data);
            });
        };

        this.voteUp = function () {
            var currentVoteCount = self.voteCount() || 0;
            self.voteCount(currentVoteCount + 1);
            self.voteText('Thank you for voting!');
            self.voteEnabled(false);
            http.put('api/goals/' + self.goalId() + '/vote');
        };

        this.activate = function(goalId) {
            self.goalId(goalId);
            return http.get('api/goals/' + goalId + '/activities').success(function(data) {
                self.activities(data.activities);
                self.displayName(data.goal.Name);
                self.voteCount(data.goal.VoteCount);
                http.get('api/goals/' + goalId + '/open-suggestions').success(function (suggestionsData) {
                    self.suggestions(suggestionsData.activitySuggestions);
                });
            });
        };
    };

    return areas;
});