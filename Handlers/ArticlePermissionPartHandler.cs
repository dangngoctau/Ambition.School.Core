using Ambition.School.Core.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Ambition.School.Core.Handlers
{
    [UsedImplicitly]
    public class ArticlePermissionPartHandler : ContentHandler
    {
        public ArticlePermissionPartHandler(IRepository<ArticlePermissionPartRecord> memberRepository)
        {
            Filters.Add(StorageFilter.For(memberRepository));
        }
    }
}