using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = null!;
        [MaxLength(7)]
        public string Color { get; set; } = null!;
        public bool IsGlobal { get; set; }
    }
}
