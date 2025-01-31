using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("teachers")]

    public class CommentController : Controller
    {
        public readonly TeacherRepository _teacherRepository;


        public CommentController(TeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        [HttpGet]
        public ActionResult<ICollection<Teacher>> GetTeachers()
        {
            return Ok(_teacherRepository.GetTeachers());
        }
        [HttpPost]
        public ActionResult CreateTeacher(TeacherCreateDTO teacher)
        {
            _teacherRepository.CreateTeacher(teacher);
            return Ok();
        }
        // Update method
        [HttpPut("{id}")]
        public ActionResult UpdateTeacher(int id, TeacherUpdateDTO teacherUpdateDTO)
        {
            var result = _teacherRepository.UpdateTeacher(id, teacherUpdateDTO);
            return result; // Retourner le résultat directement depuis le dépôt
        }

    }
}
