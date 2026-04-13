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
    public class GroupsController : Controller
    {
        private readonly NC_26Context _context;

        public GroupsController(NC_26Context context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            return View(await _context.Group.Include(g => g.Field).Include(g => g.Students).Include(g=>g.Courses).ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .Include(g => g.Field)
                .Include(g => g.Students)
                .Include(g => g.Courses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Name");
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,FieldId")] Group @group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Name", group.FieldId);
            return View(@group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Name", group.FieldId);
            GetCourseList(id);
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FieldId")] Group @group)
        {
            if (id != @group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@group);
                    var xcr = @group.Courses;
                    var SC = HttpContext.Request.Form["selectedCourses"];
                    var gr = _context.Group.Include(g => g.Courses).Single(g => g.Id == id);
                    if (gr.Courses != null) { gr.Courses.Clear(); }
                    foreach (var sc in SC)
                    {
                        var course = _context.Course.Single(course => course.Id == int.Parse(sc));
                        gr.Courses.Add(course);
                    }
                    _context.Update(gr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.Id))
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
            ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Name", group.FieldId);
            GetCourseList(id);
            return View(@group);
        }


        //GET: Group/5/Attendance/5
        [Route("Group/{id}/Attendance/{courseid}")]
        public async Task<IActionResult> Attendance(int id, int courseid)
        {
            var @course = _context.Course.Single(c => c.Id == courseid);
            var @group = _context.Group.Include(g => g.Students).Single(g => g.Id == id);
            ViewData["attnlist"] = GetAttnList(@group, @course);
            ViewData["course"] = @course;
            return View(@group);
        }

        //GET: Group/5/GetAttn/5
        [Route("Group/{id}/GetAttn/{courseid}")]
        public async Task<IActionResult> GetAttn(int id, int courseid)
        {
            var @course = _context.Course.Single(c => c.Id == courseid);
            var @group = _context.Group.Include(g => g.Students).Single(g => g.Id == id);
            ViewData["attnlist"] = GetAttnList(@group, @course);
            ViewData["course"] = @course;
            ViewData["data"] = DateTime.Now.ToString("yyyy-MM-dd");
            return View(@group);
        }


        // POST: Group/5/GetAttn/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Group/{id}/GetAttn/{courseid}")]
        public async Task<IActionResult> GetAttn(int id)
        {
            var courseid = int.Parse(HttpContext.Request.Form["courseid"]);
            var data = DateOnly.Parse(HttpContext.Request.Form["data"]);
            var present = HttpContext.Request.Form["present"];
            foreach (var sid in present)
            {
                var xid = int.Parse(sid);
                var attn = new Attendance()
                {
                    CourseId = courseid,
                    StudentId = xid,
                    Data = data
                };
                _context.Add(attn);
            }
            await _context.SaveChangesAsync();
            var @group = _context.Group.Include(g => g.Students).Single(g => g.Id == id);
            var @course = _context.Course.Single(c => c.Id == courseid);
            ViewData["attnlist"] = GetAttnList(@group, @course);
            ViewData["course"] = @course;
            ViewData["data"] = DateTime.Now;
            return View("Attendance", @group);
        }


        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .Include(g => g.Field)
                .Include(g => g.Students)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @group = await _context.Group.FindAsync(id);
            if (@group != null)
            {
                _context.Group.Remove(@group);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return _context.Group.Any(e => e.Id == id);
        }


        //Funkcja tworząca liste kursów
        private void GetCourseList(int? id)
        {
            var Courses = _context.Course.ToList();
            var Selected = _context.Group.Include(g => g.Courses).Single(g => g.Id == id);
            var coursestocheck = new List<CGcheck>();
            foreach (var course in Courses)
            {
                var xcheck = "";
                if (Selected.Courses.Contains(course)) { xcheck = "checked"; }
                ;
                coursestocheck.Add(
                   new CGcheck
                   {
                       CourseId = course.Id,
                       Name = course.Name,
                       Checked = xcheck,
                   }
                   );
            }
            ViewData["courses"] = coursestocheck;
        }

        private List<AttnList> GetAttnList(Group group, Course course)
        {
            var attnlist = new List<AttnList>();
            var students = group.Students;
            var @daty = new List<DateOnly>();
            var attnd = _context.Attendance.Where(a => a.CourseId == course.Id).OrderBy(a => a.Data).ToList();
            var i = 0;
            foreach (var student in students)
            {
                var stud_attn = attnd.Where(a => a.StudentId == student.Id).ToList();
                attnlist.Add(
                        new AttnList
                        {
                            Student = student,
                            Daty = []
                        }
                        );
                foreach (var sa in stud_attn)
                {
                    if (!@daty.Contains(sa.Data))
                    {
                        @daty.Add(sa.Data);
                    }
                    attnlist[i].Daty.Add(sa.Data);
                }
                i++;
            }

            ViewData["daty"] = @daty.Order();
            return attnlist;
        }

    }
}
