(function ($) {
    var token = $('input[name=__RequestVerificationToken]').val();

    ko.safeObservable = function (initialValue) {
        var result = ko.observable(initialValue);
        result.safe = ko.dependentObservable(function () {
            return result() || {};
        });

        return result;
    };

    var articlePermissions = function () {
        var self = this;
        self.fetchedData = false;
        self.departments = ko.observableArray([]);
        self.isEnabled = ko.observable(false);
        self.isEnabled.subscribe(function (newValue) {
            if (newValue == true && self.fetchedData == false) {
                self.fetchData();
            }
        });
        self.postData = ko.observable("ddd");
        self.chosenDepartment = ko.safeObservable();
        self.selectedDepartments = ko.computed(function () {
            var result = [];
            var willpostData = "";
            ko.utils.arrayForEach(self.departments(), function (item0) {
                var pds = [];
                ko.utils.arrayForEach(item0.positions(), function (item1) {
                    if (item1.selected()) {
                        pds.push(item1);
                        willpostData += (item1.pdId + ",");
                    }
                });
                if (pds.length > 0) {
                    var dept1 = new department(item0.id, item0.name);
                    dept1.positions(pds);
                    result.push(dept1);
                }
            });
            self.postData(willpostData);
            return result;
        });
        self.fetchData = function () {
            $.ajax({
                type: 'POST',
                url: encodeURI("/OrchardLocal/ambition.school.core/departmentadmin/ListDepartmentsAjax"),
                data: { __RequestVerificationToken: token },
                beforeSend: function () {
                },
                success: function (data) {
                    ko.utils.arrayForEach(data.depts, function (dpt) {
                        var dept = new department(dpt.di, dpt.dn);
                        ko.utils.arrayForEach(dpt.pds, function (pd) {
                            dept.positions.push(new position(pd.pdi, pd.pn));
                        });
                        self.departments.push(dept);
                    });
                    self.fetchedData = true;
                },
                error: function () {
                }
            });
        };
    };

    var department = function (id, name) {
        var self = this;
        self.id = id;
        self.name = name;
        self.positions = ko.observableArray([]);
    };

    var position = function (positionDepartmentId, positionName) {
        var self = this;
        self.pdId = positionDepartmentId;
        self.positionName = positionName;
        self.selected = ko.observable(false);
    };

    $(document).ready(function () {
        var model = new articlePermissions();
        //model.initData();
        ko.applyBindings(model);
    });
})(jQuery);