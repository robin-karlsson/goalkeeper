define(['plugins/router', 'plugins/http'], function (router, http) {
    return {
        router: router,
        activate: function () {
            router.map([
                { route: '', title:'Areas', moduleId: 'viewmodels/areas', nav: true },
                { route: 'goal/*goalId', title: 'Goal', moduleId: 'viewmodels/goal', nav: false },
                { route: 'goals', title:'Goals', moduleId: 'viewmodels/goals', nav: true },
                { route: 'area/*areaId', title:'Area', moduleId: 'viewmodels/area', nav: false },
                { route: 'areas', title: 'Areas', moduleId: 'viewmodels/areas', nav: false },
                { route: 'activity/*activityId', title: 'Activity', moduleId: 'viewmodels/activity', nav: false },
                { route: 'sampledata', title: 'Sample data', moduleId: 'viewmodels/sampledata', nav: false }
            ]).buildNavigationModel();

            return http.get('api/dateranges/current-latest').success(function(data) {
                if (data != null) {
                    router.daterange = data;
                } else {
                    router.daterange = { Name: 'No date range found', Id: undefined };
                }
                router.activate();
            //}).then(function() {
            });
        }
    };
});