define(['durandal/plugins/router', 'durandal/app', 'durandal/http'], function (router, app, http) {

    return {
        router: router,
        activate: function () {
            router.daterange = { Name: '', Id: undefined };
            return http.get('api/dateranges/current-latest').success(function (data) {
                if (data != null) {
                    router.daterange = data;
                }
                router.activate('areas');
            });
        }
    };
});