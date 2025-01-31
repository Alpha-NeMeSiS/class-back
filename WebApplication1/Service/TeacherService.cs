using WebApplication1.Controllers;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Service
{
    public class TeacherService
    {
        TeacherRepository repository;
        public TeacherService (TeacherRepository repository)
        {
            this.repository = repository;
        }
    public void CreateTeacher(TeacherCreateDTO teacher)
        {
            if ((teacher.DateNaissance.Year - DateTime.UtcNow.Year) > 18)
            {
                throw new Exception("le prof doit être majeur");
            }
            Teacher t = new Teacher
            {
                Nom = teacher.Nom,
                Prenom = teacher.Prenom,
                DateNaissance = teacher.DateNaissance
            };
            repository.CreateTeacher(teacher);
        }
    }
}
