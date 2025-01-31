using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;

namespace WebApplication1.DTO
{
    public class CoursCreateDTO
    {
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
