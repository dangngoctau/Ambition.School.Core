using Ambition.School.Core.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Ambition.School.Core.Handlers
{
    [UsedImplicitly]
    public class TeacherPartHandler : ContentHandler
    {
        public TeacherPartHandler(IRepository<TeacherPartRecord> memberRepository)
        {
            Filters.Add(StorageFilter.For(memberRepository));
        }
    }
}