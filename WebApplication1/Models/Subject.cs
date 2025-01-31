using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Subject
    {
        [Key]
        public int IdSubject { get; set; }
        public string Name {  get; set; }
    }
}
