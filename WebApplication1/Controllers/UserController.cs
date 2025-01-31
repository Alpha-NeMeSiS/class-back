using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("courses")]

    public class CourseController : Controller
    {
        public readonly CourseRepository _courseRepository;

        public CourseController(CourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        [HttpGet]
        public ActionResult<ICollection<Course>> GetCourses()
        {
            return Ok(_courseRepository.GetCourses());
        }
        [HttpPost]
        public ActionResult CreateCourse(CoursCreateDTO course)
        {
            _courseRepository.CreateCourse(course);
            return Ok();
        }
    }
}
