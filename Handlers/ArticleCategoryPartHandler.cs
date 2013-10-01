using Ambition.School.Core.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Ambition.School.Core.Handlers {
    [UsedImplicitly]
    public class ArticleCategoryPartHandler : ContentHandler
    {
        public ArticleCategoryPartHandler(IRepository<ArticleCategoryPartRecord> articleCategoryRepository)
        {
            Filters.Add(StorageFilter.For(articleCategoryRepository));
        }
    }
}