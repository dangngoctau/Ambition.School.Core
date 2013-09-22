using System.ComponentModel;
using System.Web;
using Orchard;
using Orchard.Localization.Services;

namespace Ambition.School.Core.Extensions
{
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly string _scope;

        public LocalizedDisplayNameAttribute(string displayName, string scope)
            : base(displayName)
        {
            _scope = scope;
        }

        public override string DisplayName
        {
            get
            {
                var workContextAccessor = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["IWorkContextAccessor"] as IWorkContextAccessor;
                if (workContextAccessor == null)
                {
                    return base.DisplayName;
                }
                var workContext = workContextAccessor.GetContext();
                var localizedStringManager = workContext.Resolve<ILocalizedStringManager>();
                var localizedString = localizedStringManager.GetLocalizedString(_scope, base.DisplayName, workContext.CurrentCulture);
                return localizedString;
            }
        }


    }

}