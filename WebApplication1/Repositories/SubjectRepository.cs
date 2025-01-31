using WebApplication1.CourseDbContext;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class SubjectRepository(CourseContext context)
    {
        public readonly CourseContext _context = context;

        public List<Subject> GetSubjects()
        {
            return _context.Subjects.ToList();
        }
        public void CreateSubject(SubjectCreateDTO Subject)
        {
            Subject s = new Subject
            {
                Name = Subject.Name,
            };
            _context.Subjects.Add(s);
            _context.SaveChanges();
        }
    }
}