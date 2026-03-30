using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_26.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nazwisko")]
        public string? Surname { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Imię")]
        public string? Name { get; set; }

        [Required]
        [StringLength(5)]
        [MinLength(5)]
        [MaxLength(5)]
        [Display(Name = "Nr albumu")]
        public string? Index { get; set; }

        [Display(Name = "Data rozpoczęcia studiów")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataS { get; set; }

        [ForeignKey("GroupId")]
        public int? GroupId { get; set; }

        [Display(Name = "Grupa")]
        public Group? Group { get; set; }

        [Display(Name = "Oceny")]
        public ICollection<Grade>? Grades { get; set; }

        public string IN
        {
            get
            {
                return Name + " " + Surname;
            }
        }
    }
}
