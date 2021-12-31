#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EAP_Practice.Models;

namespace EAP_Practice.Data
{
    public class EAP_PracticeContext : DbContext
    {
        public EAP_PracticeContext (DbContextOptions<EAP_PracticeContext> options)
            : base(options)
        {
        }

        public DbSet<EAP_Practice.Models.Employee> Employee { get; set; }
    }
}
