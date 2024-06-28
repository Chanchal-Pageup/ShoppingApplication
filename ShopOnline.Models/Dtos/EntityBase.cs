using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Models.Dtos
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }

}
