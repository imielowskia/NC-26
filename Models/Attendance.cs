using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_26.Models
{
    [Table("Attendances")]
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("CourseId")]
        public int CourseId { get; set; }

        [Display(Name = "Przedmiot")]
        public Course? Course { get; set; }

        [ForeignKey("StudentId")]
        public int StudentId { get; set; }

        [Display(Name = "Student")]
        public Student? Student { get; set; }

        [Display(Name = "Data")]
        public DateOnly Data { get; set; }
    }
}
