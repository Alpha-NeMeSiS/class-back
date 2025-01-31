using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
        [ApiController]
        [Route("Subjects")]

        public class SubjectController : Controller
        {
            public readonly SubjectRepository _SubjectRepository;

            public SubjectController(SubjectRepository SubjectRepository)
            {
                _SubjectRepository = SubjectRepository;
            }

            [HttpGet]
            public ActionResult<ICollection<Subject>> GetSubjects()
            {
                return Ok(_SubjectRepository.GetSubjects());
            }
            [HttpPost]
            public ActionResult CreateSubject(SubjectCreateDTO Subject)
            {
                _SubjectRepository.CreateSubject(Subject);
                return Ok();
            }
        }

}
