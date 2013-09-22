using System.Collections.Generic;
using Ambition.School.Core.Models;
using Orchard;

namespace Ambition.School.Core.Services
{
    public interface IDepartmentService : IDependency
    {
        void CreatePosition(PositionRecord record);
        PositionRecord GetPosition(int id);
        void UpdatePosition(PositionRecord record);
        void DeletePosition(PositionRecord record);
        IList<PositionRecord> ListPositions();
        IList<DepartmentRecord> ListDepartments(int[] excludedIds);
        IList<DepartmentRecord> ListDepartments();
        DepartmentRecord GetDepartment(int id);
        void CreateDepartment(DepartmentRecord record);
        void DeletePositionsDepartment(IList<PositionsDepartmentRecord> records);
        void DeletePositionsDepartment(PositionsDepartmentRecord record);
        void DeletePositionsDepartment(int id);
        bool CheckPositionsDepartment(int[] ids);
        void UpdateDepartment(DepartmentRecord record);
    }
}