using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NC_26.Models;

namespace NC_26.Data
{
    public class NC_26Context : DbContext
    {
        public NC_26Context (DbContextOptions<NC_26Context> options)
            : base(options)
        {
        }

        public DbSet<NC_26.Models.Group> Group { get; set; } = default!;
        public DbSet<NC_26.Models.Student> Student { get; set; } = default!;
        public DbSet<NC_26.Models.Field> Field { get; set; } = default!;
        public DbSet<NC_26.Models.Course> Course { get; set; } = default!;
        public DbSet<NC_26.Models.Grade> Grade { get; set; } = default!;
        public DbSet<NC_26.Models.Attendance> Attendance { get; set; } = default!;
    }
}
