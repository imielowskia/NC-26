using Microsoft.AspNetCore.Mvc;
using NC_26.Models;
using Microsoft.EntityFrameworkCore;
using NC_26.Data;

namespace NC_26.ViewComponents
{
    public class AttendanceViewComponent : ViewComponent
    {
        private readonly NC_26Context _context;
        public AttendanceViewComponent(NC_26Context context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id, int courseid, string typ = "")
        {
            if (typ == "")
            {
                var group = await _context.Group.FindAsync(id);
                var course = await _context.Course.FindAsync(courseid);
                ViewData["course"] = course;
                ViewData["group"] = group;
                return View("blank");
            }
            else if (typ == "show")
            {
                var group = await _context.Group.Include(g=>g.Students).SingleAsync(g => g.Id == id);
                var course = await _context.Course.FindAsync(courseid);                
                ViewData["attnlist"] = GetAttnList(group, course);
                ViewData["course"] = @course;
                ViewData["group"] = group;
                return View("show");

            }
            else
            {
                var course = await _context.Course.SingleAsync(c => c.Id == courseid);
                var group = await _context.Group.Include(g => g.Students).SingleAsync(g => g.Id == id);
                ViewData["attnlist"] = GetAttnList(group, course);
                ViewData["course"] = course;
                ViewData["data"] = DateTime.Now.ToString("yyyy-MM-dd");
                ViewData["group"] = group;                
                return View("add");
            }

        }


        //Funkcja tworząca listę obecności
        private List<AttnList> GetAttnList(Group group, Course course)
        {
            var attnlist = new List<AttnList>();
            var students = group.Students;
            var @daty = new List<DateOnly>();
            var attnd = _context.Attendance.Where(a => a.CourseId == course.Id).ToList();
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

            ViewData["daty"] = @daty;
            return attnlist;
        }


    }
}
