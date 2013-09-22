using Orchard.ContentManagement.Records;

namespace Ambition.School.Core.Models
{
    public class ArticlePermissionPartRecord : ContentPartRecord
    {
        public virtual bool IsEnabled { get; set; }
        public virtual string Permissions { get; set; }
    }
}