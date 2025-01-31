using Microsoft.AspNetCore.Mvc;
using WebApplication1.CourseDbContext;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class TeacherRepository
    {
        public readonly CourseContext _context;

        public TeacherRepository(CourseContext context)
        {
            _context = context;
        }

        public List<Teacher> GetTeachers()
        {
            return _context.Teachers.ToList();
        }
        public void CreateTeacher(TeacherCreateDTO teacher)
        {
            Teacher t = new Teacher
            {
                Nom = teacher.Nom,
                Prenom = teacher.Prenom,
                DateNaissance = teacher.DateNaissance,
            };
            _context.Teachers.Add(t);
            _context.SaveChanges();
        }
        public ActionResult UpdateTeacher(int id, TeacherUpdateDTO teacherUpdateDTO)
        {

            var existingTeacher = _context.Teachers.Find(id);
            if (existingTeacher == null)
            {
                return new NotFoundObjectResult($"Enseignant avec l'ID {id} non trouvé.");
            }

            // Mettre à jour les détails de l'enseignant
            existingTeacher.Nom = teacherUpdateDTO.Nom;
            existingTeacher.Prenom = teacherUpdateDTO.Prenom;
            existingTeacher.DateNaissance = teacherUpdateDTO.DateNaissance;

            _context.SaveChanges(); // Sauvegarder les changements dans la base de données

            return new NoContentResult(); // Retourner 204 No Content
        }
    }
}
