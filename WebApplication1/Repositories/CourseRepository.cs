using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.CourseDbContext;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class CourseRepository
    {
        public readonly CourseContext _context;

        public CourseRepository(CourseContext context)
        {
            _context = context;
        }

        public List<Course> GetCourses()
        {
            return _context.Courses.ToList();
        }
        public void CreateCourse(CoursCreateDTO course)
        {
            Course c = new Course
            {
                IdMatiere = course.IdMatiere,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                IdTeacher = course.IdTeacher,
                Teacher = course.Teacher,
                IdSubject = course.IdSubject,
                Subject = course.Subject
            };
            _context.Courses.Add(c);
            _context.SaveChanges();
        }
    }
}
