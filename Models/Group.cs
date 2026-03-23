using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_26.Models
{
    [Table("Groups")]
    public class Group
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Grupa")]
        public string? Name { get; set; }

        [ForeignKey("FieldId")]
        public int? FieldId { get; set; }

        [Display(Name = "Kierunek")]
        public Field? Field { get; set; }

        [Display(Name = "Studenci")]
        public virtual ICollection<Student>? Students { get; set; }

        [Display(Name = "Przedmioty")]
        public virtual ICollection<Course>? Courses { get; set; }

    }
}
