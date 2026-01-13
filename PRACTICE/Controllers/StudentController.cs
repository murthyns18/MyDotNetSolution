using Microsoft.AspNetCore.Mvc;
using PRACTICE.Models;
using PRACTICE.Repositories;

namespace PRACTICE.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentRepository _repo;

        public StudentController(StudentRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Student std)
        {
            _repo.AddStudent(std);
            TempData["Message"] = "Student Added successfully!";

            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            var students = _repo.GetAllStudents();

            return View(students);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _repo.GetStudentByID(id);

            return View("Add", student);
        }

        [HttpPost]
        public IActionResult Edit(Student std)
        {
            _repo.UpdateStudent(std);
            TempData["Message"] = "Student Updated successfully!";

            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _repo.DeleteStudentByID(id);
            TempData["Message"] = "Student deleted successfully!";
            TempData["Type"] = "danger";
           
            return RedirectToAction("List");
        }

    }
}
