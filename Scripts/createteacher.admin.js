(function ($) {
    DepartmentViewModel.prototype = {
        constructor: DepartmentViewModel,
        initData: function () {
            var dataField = $('#teacher-positions');
            this.positionsData = _.string.words(dataField.attr('data-positions'), ',');
            this.requestToken = $('input[name=__RequestVerificationToken]').val();
            this.fetchData();
        }
    };

    $(document).ready(function () {
        var model = new DepartmentViewModel();
        ko.applyBindings(model);
        model.initData();
    });
})(jQuery);