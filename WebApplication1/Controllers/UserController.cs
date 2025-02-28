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
        public readonly RecipeRepository _courseRepository;

        public CourseController(RecipeRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
    }
}
