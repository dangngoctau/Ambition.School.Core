using System.Web.Mvc;
using Ambition.School.Core.Models;

namespace Ambition.School.Core.ViewModels
{
    [Bind(Exclude = "Id")]
    public class PositionCreateViewModel : PositionRecord
    {
    }

}