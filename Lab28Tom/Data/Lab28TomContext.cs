using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lab28Tom.Models
{
    public class Lab28TomContext : DbContext
    {
        public Lab28TomContext (DbContextOptions<Lab28TomContext> options)
            : base(options)
        {
        }

        public DbSet<Lab28Tom.Models.LFG> LFG { get; set; }
    }
}
