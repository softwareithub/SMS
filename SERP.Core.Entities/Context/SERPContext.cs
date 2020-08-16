using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.TransactionLog;
using System;
using System.Collections.Generic;
using System.Text;
using SERP.Core.Entities.StudentTransaction;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.HomeAssignment;
using SERP.Core.Entities.LibraryManagement;
using SERP.Core.Entities.UserManagement;
using SERP.Core.Entities.LessionMaster;
using SERP.Core.Entities.OnlineTest;
using SERP.Core.Entities.OnlineVideo;
using SERP.Core.Entities.Transport;
using SERP.Core.Entities.Accounts;
using SERP.Core.Entities.Student;
using SERP.Core.Entities.FrontOffice;
using SERP.Core.Entities.SERPExceptionLogging;

namespace SERP.Core.Entities.Context
{

    public class SERPContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server= DESKTOP-SF1G3N8\\VIPRAIT; Database= VinobaEducationPortal; User Id=sa;Password = vi@pra91");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReligionMaster>().Property(r => r.IsActive).HasDefaultValue(1);
        }


        #region
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
        public DbSet<TimeTableMasterModel> TimeTableMasterModels { get; set; }
        public DbSet<TimeTableAssignSubjTeacherModel> TimeTableAssignSubjTeacherModels { get; set; }
        public DbSet<StudentAttendenceModel> StudentAttendenceModels { get; set; }
        public DbSet<FeeDeposit> FeeDeposits { get; set; }
        public DbSet<StudentFeeDepositParticular> StudentFeeDepositParticulars { get; set; }
        public DbSet<Exam> ExamMasters { get; set; }
        public DbSet<ExamSheet> ExamSheets { get; set; }
        public DbSet<AssignmentModel> Assignments { get; set; }
        public DbSet<GradeMaster> GradeMasters { get; set; }
        public DbSet<StudentMarkAllocation> StudentMarkAllocations { get; set; }
        public DbSet<BookMasterModel> BookMasterModels { get; set; }
        public DbSet<Authenticate> AuthenticateModels { get; set; }
        public DbSet<LessonMaster> LessonMasters { get; set; }
        public DbSet<LessonTopicMapping> LessonTopicMappings { get; set; }
        public DbSet<ModuleMaster> ModuleMasters { get; set; }
        public DbSet<SubModuleMaster> SubModules { get; set; }
        public DbSet<UserAccessRight> UserRights { get; set; }
        public DbSet<GuardianMaster> GurdianMasters { get; set; }
        public DbSet<SMSTemplateModel> SMSTemplateModels { get; set; }
        public DbSet<SMSBulk> SMSBulks { get; set; }
        public DbSet<HomeWorkModel> HomeWorkModels { get; set; }
        public DbSet<StudentHomeWork> StudentHomeWorks { get; set; }
        public DbSet<StudyMaterial> StudyMaterials { get; set; }
        public DbSet<AcademicCalender> AcademicCalenders { get; set; }
        public DbSet<ExamUpdate> ExamUpdates { get; set; }
        public DbSet<QuestionModel> QuestionModels { get; set; }
        public DbSet<OptionMaster> OptionMasters { get; set; }
        public DbSet<TestMaster> TestMasters { get; set; }
        public DbSet<TestQuestionMapping> TestQuestionMappings { get; set; }
        public DbSet<OnlineVideoFeeDetail> OnlineVideoFeeDetails { get; set; }
        public DbSet<LeaveMaster> LeaveMasters { get; set; }
        public DbSet<LeaveAllotment> LeaveAllotments { get; set; }
        public DbSet<LeaveTransactionModel> LeaveTransactionModels { get; set; }
        public DbSet<CategoryMaster> CategoryMasters { get; set; }
        public DbSet<BookItemModel> BookItemModels { get; set; }
        public DbSet<BookStatusModel> BookStatusModels { get; set; }
        public DbSet<BookTransaction> BookTransactions { get; set; }
        public DbSet<LibrarySetting> LibrarySettings { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<StopageModel> StopageModels { get; set; }
        public DbSet<RouteMaster> RouteMasters { get; set; }
        public DbSet<RouteStopageModel> RouteStopageModels { get; set; }
        public DbSet<VehicleFuelDetail> VehicleFuelDetails { get; set; }
        public DbSet<RouteStudentMapping> StudentMappings { get; set; }
        public DbSet<IncomeHeads> IncomeHeads { get; set; }
        public DbSet<ExpenseHead> ExpenseHeads { get; set; }
        public DbSet<AccountDetail> Accounts { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }
        public DbSet<RoleMaster> RoleMasters { get; set; }
        public DbSet<UserAnswereSheetModel> UserAnswereSheetModels { get; set; }
        public DbSet<UserTestDetailModel> UserTestDetailModels { get; set; }
        public DbSet<AssginmentUpload> AssginmentUploads { get; set; }
         public DbSet<StudentEducationalDetail> StudentEducationDetails { get; set; }
        public DbSet<StudentDocumentUpload> StudentDocumentUploads { get; set; }
        public DbSet<InstituteSettingModel> InstituteSettingModels { get; set; }
        public DbSet<StudentGatePass> StudentGatePasses { get; set; }
        public DbSet<VisitorBook> VisitorBooks { get; set; }
        public DbSet<ExceptionLogging> ExceptionLoggings { get; set; }
        public DbSet<QuickLink> QuickLinks { get; set; }
        #endregion

    }
}
