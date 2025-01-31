using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Course
    {
        [Key]
        public int IdCourse { get; set; }
        public int IdMatiere { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int IdTeacher { get; set; }
        [ForeignKey("IdTeacher")]
        public Teacher Teacher { get; set; }
        public int IdSubject { get; set; }
        [ForeignKey("IdSubject")]
        public Subject Subject { get; set; }
    }
}
