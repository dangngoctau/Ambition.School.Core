using System.Linq;
using System.Web.Mvc;
using Ambition.School.Core.Services;
using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Settings;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;

namespace Ambition.School.Core.Controllers
{
    [Admin]
    public class MemberAdminController : Controller
    {
        private readonly IMemberService _memberService;
        public ILogger Logger { get; set; }
        public Localizer T { get; set; }
        dynamic Shape { [UsedImplicitly] get; set; }
        private readonly ISiteService _siteService;
        private readonly IContentManager _contentManager;
        public MemberAdminController(IMemberService memberService,
            IShapeFactory shapeFactory,
            ISiteService siteService,
            IContentManager contentManager)
        {
            _memberService = memberService;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
            Shape = shapeFactory;
            _siteService = siteService;
            _contentManager = contentManager;
        }

        public ActionResult List(PagerParameters pagerParameters)
        {
            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);
            var query = _memberService.QueryTeacher();
            var pagerShape = Shape.Pager(pager).TotalItemCount(query.Count());
            var pageOfContentItems = query.Slice(pager.GetStartIndex(), pager.PageSize).ToList();
            var list = Shape.List();
            list.AddRange(pageOfContentItems.Select(ci => _contentManager.BuildDisplay(ci, "SummaryAdmin")));
            dynamic viewModel = Shape.ViewModel()
                .Pager(pagerShape)
                .ContentItems(list);
            return View((object)viewModel);
        }
    }
}