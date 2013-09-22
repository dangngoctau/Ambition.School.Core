using Ambition.School.Core.Extensions.Html;
using Orchard.Services;

namespace Ambition.School.Core.Services
{
    public class HtmlService : IHtmlFilter
    {
        public string ProcessContent(string text, string flavor)
        {
            return text.FormatText(false, false, true, false, false, false);
        }
    }
}