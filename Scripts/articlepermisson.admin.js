(function ($) {
    var token = $('input[name=__RequestVerificationToken]').val();
    var isProcessing = false;
    var permissionData = [];

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
        self.isEnabled.subscribe(function (value) {
            if (value == true && self.fetchedData == false) {
                self.fetchData();
            }
        });
        self.postData = ko.observable("");
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
            if (isProcessing) {
                return;
            }
            isProcessing = true;
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
                            var selected = ko.utils.arrayFirst(permissionData, function (per) {
                                return _.string.toNumber(per) == pd.pdi;
                            });
                            dept.positions.push(new position(pd.pdi, pd.pn, selected));
                        });
                        self.departments.push(dept);
                    });
                    self.fetchedData = true;
                },
                error: function () {
                },
                completed: function () {
                    isProcessing = false;
                }
            });
        };
        self.initData = function () {
            var dataField = $('#article-permssion');
            self.isEnabled(_.string.toBoolean(dataField.attr('data-isenabled')));
            permissionData = _.string.words(dataField.attr('data-permission'), ',');
        };
    };

    var department = function (id, name) {
        var self = this;
        self.id = id;
        self.name = name;
        self.positions = ko.observableArray([]);
    };

    var position = function (positionDepartmentId, positionName, selected) {
        var self = this;
        self.pdId = positionDepartmentId;
        self.positionName = positionName;
        self.selected = ko.observable(selected);
    };

    $(document).ready(function () {
        var model = new articlePermissions();
        ko.applyBindings(model);
        model.initData();
    });
})(jQuery);