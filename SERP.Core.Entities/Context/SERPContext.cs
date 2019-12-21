using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Entity.Core.Master;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Entities.Context
{
    //test
    //test2
    public class SERPContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server= DESKTOP-SF1G3N8\\VIPRAIT; Database= SERP; User Id=sa;Password = vi@pra91");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        DbSet<AcademicMaster> AcademicMasters { get; set; }
        DbSet<InstituteMaster> InstituteMaster { get; set; }
        DbSet<CourseMaster> CourseMaster { get; set; }

    }
}
