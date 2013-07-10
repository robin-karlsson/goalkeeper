define(['durandal/plugins/router', 'durandal/app', 'durandal/http'], function (router, app, http) {

    return {
        router: router,
        activate: function () {
            return http.get('api/dateranges/current-latest').success(function (data) {
                if (data != null) {
                    router.daterange = data;
                } else {
                    router.daterange = { Name: '', Id: undefined };
                }
                router.activate('areas');
            });
        }
    };
});