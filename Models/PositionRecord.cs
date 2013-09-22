using System.ComponentModel.DataAnnotations;

namespace Ambition.School.Core.Models
{
    public class PositionRecord
    {
        public virtual int Id { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string Name { get; set; }
        public virtual int DisplayOrder { get; set; }
    }
}