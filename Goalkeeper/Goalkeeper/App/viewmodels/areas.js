define(function() {
    var areas = function () {
        this.displayName = 'Areas';
        this.description = 'Organize your organizational goals and keep daily track of the progress.';
        this.areas = [
            {id: '123',name: 'asdf'}
        ];
    };

    areas.prototype.viewAttached = function (view) {
        //you can get the view after it's bound and connected to it's parent dom node if you want
    };

    return areas;
});