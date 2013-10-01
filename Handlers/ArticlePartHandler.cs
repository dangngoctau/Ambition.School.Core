using Ambition.School.Core.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Ambition.School.Core.Handlers {
    [UsedImplicitly]
    public class ArticlePartHandler : ContentHandler
    {
        public ArticlePartHandler(IRepository<ArticlePartRecord> articleRepository)
        {
            Filters.Add(StorageFilter.For(articleRepository));
        }
    }
}