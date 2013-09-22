using Orchard.ContentManagement;

namespace Ambition.School.Core.Models
{
    public class ArticlePermissionPart : ContentPart<ArticlePermissionPartRecord>
    {
        public bool IsEnabled
        {
            get { return Record.IsEnabled; }
            set { Record.IsEnabled = value; }
        }
        public string Permissions
        {
            get { return Record.Permissions; }
            set { Record.Permissions = value; }
        }
    }
}