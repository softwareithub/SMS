using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.TransactionLog;
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
            modelBuilder.Entity<ReligionMaster>().Property(r => r.IsActive).HasDefaultValue(1);
        }

        public DbSet<AcademicMaster> AcademicMasters { get; set; }
        public DbSet<InstituteMaster> InstituteMaster { get; set; }
        public DbSet<CourseMaster> CourseMaster { get; set; }
        public DbSet<LogMaster> LogMasters { get; set; }
        public DbSet<BatchMaster> BatchMasters { get; set; }
        public DbSet<CasteMaster> CasteMasters { get; set; }
        public DbSet<ReligionMaster> ReligionMasters { get; set; }
        public DbSet<StudentMaster> StudentMasters { get; set; }
        public DbSet<StudentPromote> StudentPromotes { get; set; }
        public DbSet<SubjectMaster> SubjectMasters { get; set; }
        public DbSet<FeeCategory> FeeCategories { get; set; }
        public DbSet<FeeDetailClassWise> FeeDetailClassWises { get; set; }
        public DbSet<FeeDiscountModel> FeeDiscountModels { get; set; }
        public DbSet<FeeDiscountParticularWiseModel> FeeDiscountParticularWiseModels { get; set; }
        public DbSet<DepartmentModel> DepartmentModels { get; set; }
        public DbSet<DesignationModel> DesignationModels { get; set; }
        public DbSet<BranchInfoModel> BranchInfoModels { get; set; }
        public DbSet<PayHeadesModel> PayHeadesModels { get; set; }
        public DbSet<EmployeeBasicInfoModel> EmployeeBasicInfoModels { get; set; }
        public DbSet<EmployeeProfessionalInfoModel> EmployeeProfessionalInfoModels { get; set; }
        public DbSet<EmployeeSalaryModel> EmployeeSalaryModels { get; set; }
        public DbSet<EmployeeAttendenceModel> EmployeeAttendenceModels { get; set; }

    }
}
