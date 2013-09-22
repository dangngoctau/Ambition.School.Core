using System;
using System.Collections.Generic;
using Ambition.School.Core.Models;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;

namespace Ambition.School.Core.Services
{
    public class MemberService : IMemberService {
        private readonly IContentManager _contentManager;

        public MemberService(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public IContentQuery<ContentItem> QueryTeacher()
        {
            return _contentManager
                .Query(VersionOptions.Latest, ContentTypes.Teacher)
                .Join<CommonPartRecord>()
                .WithQueryHints(new QueryHints().ExpandRecords<TeacherPartRecord, MemberPartRecord>());
        }

        public IEnumerable<ContentItem> QueryTeacher(int pageSize, int pageNumber)
        {
            return QueryTeacher().Slice((Math.Max(pageSize - 1, 0)) * pageSize, pageSize);
        }
    }
}