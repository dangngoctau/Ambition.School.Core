using Ambition.School.Core.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Ambition.School.Core.Handlers
{
    [UsedImplicitly]
    public class MemberPartHandler : ContentHandler
    {
        public MemberPartHandler(IRepository<MemberPartRecord> memberRepository)
        {
            Filters.Add(StorageFilter.For(memberRepository));
        }
    }
}