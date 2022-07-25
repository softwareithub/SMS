using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.API.ResponseModel;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.FrontOffice;
using SERP.Core.Model.FrontOffice;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [EnableCors()]
    public class StudentProfileController : ControllerBase
    {
        private readonly IGenericRepository<StudentMaster, int> _IStudentMaster;
        private readonly IGenericRepository<BatchMaster, int> _IBatchMaster;
        private readonly IGenericRepository<CourseMaster, int> _ICourseMaster;
        private readonly IGenericRepository<ReligionMaster, int> _IReligionMaster;
        public readonly IGenericRepository<StudentPromote, int> _IStudentPromote;
        private readonly IGenericRepository<FeeDiscountModel, int> _IFeeDiscountRepo;
        private readonly IGenericRepository<GuardianMaster, int> _IGuardianRepo;
        private readonly IGenericRepository<StudentGatePass, int> _studentGatePassRepo;

        public StudentProfileController(IGenericRepository<StudentMaster, int> studentRepo,
        IGenericRepository<BatchMaster, int> batchRepo,
        IGenericRepository<CourseMaster, int> courseRepo,
        IGenericRepository<ReligionMaster, int> religionRepo,
        IGenericRepository<CasteMaster, int> categoryRepo,
        IGenericRepository<AcademicMaster, int> academicRepo,
         IGenericRepository<StudentPromote, int> studentPromote,
         IGenericRepository<FeeDiscountModel, int> feeDiscountRepo,
         IGenericRepository<GuardianMaster, int> guardianRepo,
         IGenericRepository<StudentGatePass, int> studentGatePassRepo
        )
        {
            _IStudentMaster = studentRepo;
            _IBatchMaster = batchRepo;
            _ICourseMaster = courseRepo;
            _IReligionMaster = religionRepo;
            _IStudentPromote = studentPromote;
            _IFeeDiscountRepo = feeDiscountRepo;
            _IGuardianRepo = guardianRepo;
            _studentGatePassRepo = studentGatePassRepo;
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentProfile(int studentId)
        {
            return Ok(await GetStudentVm(studentId));
        }

        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GuardianList(int studentId) {

            var models = (from GM in await _IGuardianRepo.GetList(x => x.IsActive == 1)
                          join SM in await _IStudentMaster.GetList(x => x.IsActive == 1)
                          on GM.StudentId equals SM.Id
                          select new GuardianMaster
                          {
                              Id = GM.Id,
                              StudentName = SM.Name,
                              StudentId = GM.StudentId,
                              GuradianName = GM.GuradianName,
                              GuardianImage = GM.GuardianImage,
                              Phone = GM.Phone,
                              Email = GM.Email,
                              IsVerified = GM.IsVerified,
                              RelationShip= GM.RelationShip
                          }).ToList();

            return Ok(models.Where(x=>x.StudentId==studentId).ToList());
        }

        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> StudentEarlyLeaving(int studentId) {

            var models = (from SG in await _studentGatePassRepo.GetList(x => x.IsActive == 1)
                          join SM in await _IStudentMaster.GetList(x => x.IsActive == 1)
                          on SG.StudentId equals SM.Id
                          join GM in await _IGuardianRepo.GetList(x => x.IsActive == 1)
                          on SG.GuradianId equals GM.Id
                          where SM.Id==studentId
                          select new StudentGatePassModel
                          {
                              Id = SG.Id,
                              StudentName = SM.Name + " (" + SM.RegistrationNumber + ") ",
                              GuardianName = GM.GuradianName,
                              ApprovedBy = SG.ApproveBy,
                              ReasonOfLeaving = SG.Reason,
                              LeavingTime = SG.StudentLeave
                          }).ToList();

            return Ok(models);
        }

        private async Task<List<StudentProfileResponseModel>> GetStudentVm(int studentId)
        {
            try
            {
                var studentPromotes = await _IStudentPromote.GetList(x => x.IsActive == 1 && x.IsDeleted == 0 && x.StudentId == studentId);
                await _IStudentPromote.CreateNewContext();
                var studentList = await _IStudentMaster.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
                await _IStudentMaster.CreateNewContext();
                var courseList = await _ICourseMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                await _IStudentMaster.CreateNewContext();
                var sectionList = await _IBatchMaster.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
                await _IStudentMaster.CreateNewContext();
                var religionList = await _IReligionMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                await _IStudentMaster.CreateNewContext();
                var categoryList = await _IFeeDiscountRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);


                var studentViewModel = (from sp in studentPromotes
                                        from sl in studentList
                                        where (sp.StudentId == sl.Id
                                         && sp.CourseId == sl.CourseId
                                         && sp.BatchId == sl.BatchId)
                                        join Cl in courseList
                                        on sp.CourseId equals Cl.Id
                                        join bl in sectionList
                                        on sp.BatchId equals bl.Id
                                        select new StudentProfileResponseModel
                                        {
                                            Id = sl.Id,
                                            RollCode = sl.RollCode,
                                            RegistrationNumber = sl.RegistrationNumber,
                                            ClassName = Cl.Name,
                                            SectionName = bl.BatchName,
                                            StudentName = sl.Name,
                                            DateOfBirth = sl.DateOfBirth,
                                            JoiningDate = sl.JoiningDate,
                                            FatherEmail = sl.FatherEmail,
                                            StudentEmail = sl.StudentEmail,
                                            FatherPhone = sl.FatherPhone,
                                            StudentPhoto = sl.StudentPhoto,
                                            StudentPhone = sl.StudentPhone,
                                            MotherPhone = sl.MotherPhone,
                                            MotherName = sl.MotherName,
                                            P_Address = sl.P_Address,
                                            C_Address = sl.C_Address,
                                            ParentPhoto = sl.ParentsPhoto,
                                            FatherName=sl.FatherName
                                        }).ToList();
                return studentViewModel;
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message;
                return null;

            }

        }
    }
}