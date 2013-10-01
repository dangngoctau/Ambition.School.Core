using Orchard.ContentManagement.Records;

namespace Ambition.School.Core.Models
{
    public class ArticleCategoryPartRecord : ContentPartRecord
    {
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual ArticleCategoryPartRecord Parent { get; set; }
    }
}