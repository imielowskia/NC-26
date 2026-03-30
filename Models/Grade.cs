using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace NC_26.Models
{
    [PrimaryKey(nameof(CourseId), nameof(StudentId))]
    [Table("Grades")]
    public class Grade
    {
        [Required]
        [ForeignKey("CourseId")]
        public int CourseId { get; set; }


        [Required]
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }

        [Display(Name = "Przedmiot")]
        public Course Course { get; set; }

        [Display(Name = "Student")]
        public Student Student { get; set; }

        [Display(Name = "Ocena")]
        [Precision(3, 1)]
        public decimal Ocena { get; set; }

    }
}
