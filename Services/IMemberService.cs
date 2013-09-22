using System.Collections.Generic;
using Orchard;
using Orchard.ContentManagement;

namespace Ambition.School.Core.Services
{
    public interface IMemberService : IDependency
    {
        IContentQuery<ContentItem> QueryTeacher();
        IEnumerable<ContentItem> QueryTeacher(int pageSize, int pageNumber);
    }
}