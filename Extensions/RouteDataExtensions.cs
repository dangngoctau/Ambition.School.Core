using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Ambition.School.Core.Extensions
{
    public static class RouteDataExtensions
    {
        public static bool CheckAction(this RouteData route, string action)
        {
            return route.Values["Action"].ToString().Equals(action, StringComparison.OrdinalIgnoreCase);
        }
    }
}