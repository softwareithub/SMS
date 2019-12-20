using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Entity.Core.Master;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Entities.Context
{
    public class SERPContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server= DESKTOP-SF1G3N8\\VIPRAIT; Database= SERP; Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        DbSet<AcademicMaster> AcademicMasters { get; set; }

    }
}
