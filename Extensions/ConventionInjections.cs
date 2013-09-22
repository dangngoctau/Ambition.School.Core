using System.Collections.Generic;
using System.Linq;
using Ambition.School.Core.Models;
using Ambition.School.Core.ViewModels;
using Omu.ValueInjecter;

namespace Ambition.School.Core.Extensions
{
    public static class UpdateModel
    {
        public static void UpdateFrom(this PositionRecord targetRecord, PositionRecord sourceRecord)
        {
            targetRecord.InjectFrom<IgnoreIdConvention>(sourceRecord);
        }

        public static PositionRecord CreateRecord(this PositionCreateViewModel model)
        {
            var record = new PositionRecord();
            record.InjectFrom(model);
            return record;
        }

        public static PositionViewModel CreateAvailablePosition(this PositionRecord model)
        {
            var record = new PositionViewModel();
            record.InjectFrom(model);
            return record;
        }

        public static DepartmentEditViewModel CreateEditModel(this DepartmentRecord model, IList<DepartmentRecord> departments, IList<PositionRecord> positions)
        {
            var editModel = new DepartmentEditViewModel();
            editModel.InjectFrom(model, new object[] { "Id", "Name" });
            if (model.ParentDepartmentRecord != null)
            {
                editModel.ParentDepartmentId = model.ParentDepartmentRecord.Id;
            }
            var availablePositions = positions.Select(c => c.CreateAvailablePosition()).ToList();
            if (model.PositionDepartmentRecords.Any())
            {
                foreach (var item in availablePositions)
                {
                    item.IsSelected = model.PositionDepartmentRecords.Any(c => c.PositionRecord.Id == item.Id);
                }
            }
            editModel.AvailableDepartments = departments;
            editModel.AvailablePositions = availablePositions;
            return editModel;
        }

        public static void UpdateViewModel(this DepartmentCreateViewModel model, IList<DepartmentRecord> departments, IList<PositionRecord> positions)
        {
            var availablePositions = positions.Select(c => c.CreateAvailablePosition()).ToList();
            if (model.AvailablePositions != null)
            {
                foreach (var item in availablePositions)
                {
                    item.IsSelected = (model.AvailablePositions.Any(c => c.Id == item.Id && c.IsSelected));
                }
            }
            model.AvailableDepartments = departments;
            model.AvailablePositions = availablePositions;
        }

    }

    public class IgnoreIdConvention : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
        {
            return c.TargetProp.Name != "Id";
        }
    }

    public class DepartmentConventionInjection : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
        {
            return c.TargetProp.Name == "Id" || c.TargetProp.Name == "Name";
        }
    }
}