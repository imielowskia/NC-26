using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NC_26.Data;
using NC_26.Models;

namespace NC_26.Controllers
{
    public class CoursesController : Controller
    {
        private readonly NC_26Context _context;

        public CoursesController(NC_26Context context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Course.Include(c => c.Groups).ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ECTS")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ECTS")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }


        // GET: Courses/Grade/5
        [Route("Courses/Grade/{id}/Grade/{groupid}")]
        public async Task<IActionResult> Grade(int id, int groupid)
        {
            var course = await _context.Course.FindAsync(id);
            var group = await _context.Group.FindAsync(groupid);
            ViewData["gradelist"] = GetGradesList(@course, group);
            ViewData["group"] = group;
            return View(@course);
        }

        //GET: Courses/5/SetGrade/5        
        [Route("Courses/{id}/SetGrade/{groupid}")]
        public async Task<IActionResult> SetGrade(int id, int groupid)
        {
            var @course = await _context.Course.FindAsync(id);
            var group = await _context.Group.FindAsync(groupid);
            ViewData["gradelist"] = GetGradesList(@course, group);
            ViewData["group"] = group;
            return View(@course);
        }


        // POST: Courses/5/SetGrade/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Courses/{id}/SetGrade/{groupid}")]
        public async Task<IActionResult> SetGrade(int id)
        {
            var @course = await _context.Course.FindAsync(id);
            var groupid = int.Parse(HttpContext.Request.Form["groupid"]);
            var ids = HttpContext.Request.Form["ids"];
            var grades = HttpContext.Request.Form["oceny"];
            var i = 0;
            var xid = 0;
            foreach (var studentid in ids)
            {
                xid = int.Parse(studentid);
                var xgr = _context.Grade.Where(g => g.StudentId == xid & g.CourseId == id);
                if (xgr.Any())
                {
                    var ocena = _context.Grade.Where(g => g.StudentId == xid & g.CourseId == id).Single();
                    ocena.Ocena = decimal.Parse(grades[i]);
                    _context.Update(ocena);
                }
                else
                {
                    var grade = new Grade()
                    {
                        StudentId = xid,
                        CourseId = id,
                        Ocena = decimal.Parse(grades[i])
                    };
                    _context.Add(grade);
                }
                i++;
            }
            await _context.SaveChangesAsync();
            var group = await _context.Group.FindAsync(groupid);
            return RedirectToAction("Grade", new { id = course.Id, groupid = group.Id });
        }




        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }


        //funkcja budująca listę ocen
        private List<StudentGrades> GetGradesList(Course course, Group group)
        {
            var students = _context.Student.Where(s => s.GroupId == group.Id).ToList();
            var grades = new List<StudentGrades>();
            var xocena = 0.0M;
            foreach (Student? student in students)
            {
                if (_context.Grade.Where(g => g.CourseId == course.Id & g.StudentId == student.Id).Any())
                {
                    xocena = _context.Grade.Where(g => g.CourseId == course.Id & g.StudentId == student.Id).First().Ocena;
                }
                else
                {
                    xocena = 0;
                }
                grades.Add(
                   new StudentGrades
                   {
                       StudentId = student.Id,
                       IN = student.IN,
                       Ocena = xocena,
                   }
                   );
            }
            return grades;

        }


    }


}
