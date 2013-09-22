using System;
using System.Collections.Generic;
using System.Linq;
using Ambition.School.Core.Extensions;
using Ambition.School.Core.Models;
using Orchard.Data;
using Orchard.Utility.Extensions;

namespace Ambition.School.Core.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly Lazy<IRepository<PositionRecord>> _positionRepository;
        private readonly Lazy<IRepository<DepartmentRecord>> _departmentRepository;
        private readonly Lazy<IRepository<PositionsDepartmentRecord>> _positionsDepartmentRepository;

        public DepartmentService(Lazy<IRepository<PositionRecord>> positionRepository,
            Lazy<IRepository<DepartmentRecord>> departmentRepository,
            Lazy<IRepository<PositionsDepartmentRecord>> positionsDepartmentRepository)
        {
            _positionRepository = positionRepository;
            _departmentRepository = departmentRepository;
            _positionsDepartmentRepository = positionsDepartmentRepository;
        }

        #region Posisions

        public void CreatePosition(PositionRecord record)
        {
            _positionRepository.Value.Create(record);
        }

        public PositionRecord GetPosition(int id)
        {
            return _positionRepository.Value.Get(id);
        }

        public void UpdatePosition(PositionRecord record)
        {
            var persistedRecord = GetPosition(record.Id);
            persistedRecord.UpdateFrom(record);
            _positionRepository.Value.Update(persistedRecord);
        }

        public void DeletePosition(PositionRecord record)
        {
            var persistedRecord = GetPosition(record.Id);
            _positionRepository.Value.Delete(persistedRecord);
        }

        public IList<PositionRecord> ListPositions()
        {
            return _positionRepository.Value.Table.OrderByDescending(c => c.DisplayOrder).ToReadOnlyCollection();
        }

        #endregion

        #region "AvailableDepartments"

        public IList<DepartmentRecord> ListDepartments(int[] excludedIds)
        {
            return _departmentRepository.Value.Fetch(c => excludedIds.Contains(c.Id) == false).ToReadOnlyCollection();
        }

        public IList<DepartmentRecord> ListDepartments()
        {
            return _departmentRepository.Value.Table.ToReadOnlyCollection();
        }

        public DepartmentRecord GetDepartment(int id)
        {
            return _departmentRepository.Value.Get(id);
        }

        public void CreateDepartment(DepartmentRecord record)
        {
            _departmentRepository.Value.Create(record);
        }

        public void DeletePositionsDepartment(IList<PositionsDepartmentRecord> records)
        {
            foreach (var record in records)
            {
                _positionsDepartmentRepository.Value.Delete(record);
            }
        }

        public void DeletePositionsDepartment(PositionsDepartmentRecord record)
        {
            _positionsDepartmentRepository.Value.Delete(record);
        }

        public void UpdateDepartment(DepartmentRecord record)
        {
            _departmentRepository.Value.Update(record);
        }

        public void DeletePositionsDepartment(int id)
        {
            var record = _positionsDepartmentRepository.Value.Get(id);
            _positionsDepartmentRepository.Value.Delete(record);
        }

        public bool CheckPositionsDepartment(int[] ids)
        {
            var records = _positionsDepartmentRepository.Value.Fetch(c => ids.Contains(c.Id));
            return records.Count() == ids.Count();
        }
        #endregion
    }
}