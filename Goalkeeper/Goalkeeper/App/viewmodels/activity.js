define(['plugins/router', 'plugins/http'], function (router, http) {
    var areas = function () {
        var self = this;
        
        this.displayName = ko.observable('Activity');
        this.description = ko.observable('Loading activity description...');
        this.longDescription = ko.observable();
        this.goal = ko.observable();
        this.activityId = ko.observable();
        this.performer = ko.observable();
        this.newPerformer = ko.observable();
        this.performers = ko.observableArray();
        this.setPerformerMode = ko.observable(false);
        this.states = ko.observableArray();
        this.original = ko.observable();

        this.delete = function () {
            var goalId = self.goalId();
            http.delete('api/activities/' + goalId).complete(function() {
                router.navigate('goals/' + goalId);
            });
        };

        this.startAssignPerformer = function() {
            http.get('api/performers').complete(function (data) {
                self.performers(data.responseJSON);
                self.newPerformer(self.performer());
                self.setPerformerMode(true);
            });
        };

        this.cancelAssignPerformer = function () {
            self.setPerformerMode(false);
            self.performers([]);
        };

        this.assignPerformer = function() {
            var item = self.original();
            var oldPerformer = item.Performer;
            var newPerformer = self.newPerformer();
            item.Performer = newPerformer;
            http.put('api/activities/' + self.activityId() + '/change', item).fail(function() {
                item.Performer = oldPerformer;
            });
            self.cancelAssignPerformer();
        };

        this.startActivity = function() {
            http.put('api/activities/' + self.activityId() + '/start').success(function(data) {
                self.original(data.JSONdata);
            });
        };
        
        this.activate = function(activityId) {
            return http.get('api/activities/' + activityId).success(function (data) {
                self.activityId(data.Id.replace('/','-'));
                self.displayName(data.Title);
                self.description(data.Abstract);
                self.longDescription(data.Description);
                self.goal(data.Goal);
                self.performer(data.Performer);
                self.original(data);
                
                if (!data.Performer) {
                    self.performer({ Name: 'Not set', Id: undefined });
                }
            });
        };
    };

    return areas;
});