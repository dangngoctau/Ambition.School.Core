ko.safeObservable = function (initialValue) {
    var result = ko.observable(initialValue);
    result.safe = ko.dependentObservable(function () {
        return result() || {};
    });
    return result;
};

function Department(id, name) {
    var self = this;
    self.id = id;
    self.name = name;
    self.positions = ko.observableArray([]);
}

function Position(positionDepartmentId, positionName, selected) {
    var self = this;
    self.pdId = positionDepartmentId;
    self.positionName = positionName;
    self.selected = ko.observable(selected);
}

function DepartmentViewModel() {
    var self = this;
    self.isFetchingData = false;
    self.positionsData = [];
    self.fetchingDataUrl = "/OrchardLocal/ambition.school.core/departmentadmin/ListDepartmentsAjax";
    self.fetchedData = false;
    self.requestToken = "";
    self.departments = ko.observableArray([]);
    self.chosenDepartment = ko.safeObservable();
    self.postData = ko.observable("");
    self.fetchData = function () {
        if (self.isFetchingData) {
            return;
        }
        self.isFetchingData = true;
        $.ajax({
            type: 'POST',
            url: encodeURI(self.fetchingDataUrl),
            data: { __RequestVerificationToken: self.requestToken },
            beforeSend: function () {
            },
            success: function (data) {
                ko.utils.arrayForEach(data.depts, function (dpt) {
                    var dept = new Department(dpt.di, dpt.dn);
                    ko.utils.arrayForEach(dpt.pds, function (pd) {
                        var selected = ko.utils.arrayFirst(self.positionsData, function (per) {
                            return parseInt(per) == pd.pdi;
                        });
                        dept.positions.push(new Position(pd.pdi, pd.pn, selected));
                    });
                    self.departments.push(dept);
                });
                self.fetchedData = true;
            },
            error: function () {
            },
            completed: function () {
                self.isFetchingData = false;
            }
        });
    };
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
                var dept1 = new Department(item0.id, item0.name);
                dept1.positions(pds);
                result.push(dept1);
            }
        });
        self.postData(willpostData);
        return result;
    });
}