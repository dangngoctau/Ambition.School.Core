using Orchard.ContentManagement;

namespace Ambition.School.Core.Models {
    public class ArticleCategoryPart : ContentPart<ArticleCategoryPartRecord>
    {
        public string Name
        {
            get { return Record.Name; }
            set { Record.Name = value; }
        }

        public string Code
        {
            get { return Record.Code; }
            set { Record.Code = value; }
        }

        public ArticleCategoryPartRecord Parent
        {
            get { return Record.Parent; }
            set { Record.Parent = value; }
        }
    }
}