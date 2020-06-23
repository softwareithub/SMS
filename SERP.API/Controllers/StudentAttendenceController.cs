using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.API.ResponseModel;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.StudentTransaction;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
   
    public class StudentAttendenceController : ControllerBase
    {
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        public readonly IGenericRepository<StudentPromote, int> _IStudentPromoteRepo;
        private readonly IGenericRepository<StudentMaster, int> _IStudentRepo;
        public readonly IGenericRepository<StudentAttendenceModel, int> _IStudentAttendence;
        public StudentAttendenceController(IGenericRepository<CourseMaster, int> courseRepo,
                    IGenericRepository<BatchMaster, int> batchRepo,
                    IGenericRepository<StudentPromote, int> studentPromoteRepo,
                    IGenericRepository<StudentMaster, int> studentRepo,
                     IGenericRepository<StudentAttendenceModel, int> AttendenceRepo)
        {
            _ICourseRepo = courseRepo;
            _IBatchRepo = batchRepo;
            _IStudentPromoteRepo = studentPromoteRepo;
            _IStudentRepo = studentRepo;
            _IStudentAttendence = AttendenceRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveCourse()
        {
            var result = await _ICourseRepo.GetList(x => x.IsActive == 1);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSectionById(int id)
        {
            var result = await _IBatchRepo.GetList(x => x.CourseId == id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> StudentAttendence([FromQuery]int courseId, [FromQuery]int batchId, [FromQuery]string attendenceDate)
        {
             DateTime.TryParse(attendenceDate, out DateTime attendenceDateParse);

            var result = (from SP in await _IStudentPromoteRepo.GetList(x => x.IsActive == 1)
                          join SM in await _IStudentRepo.GetList(x => x.IsActive == 1)
                          on SP.StudentId equals SM.Id
                          where SP.CourseId == courseId && SP.BatchId == batchId
                          select new StudentAttendenceReponseModel
                          {
                              RollCode = SM.RollCode,
                              Registration = SM.RegistrationNumber,
                              StudentId = SM.Id,
                              StudentName = SM.Name,
                              StudentPhoto = SM.StudentPhoto

                          }).ToList();
            var attendeceList = await _IStudentAttendence.GetList(x => x.AttendenceDate == attendenceDateParse);

            if (attendeceList.Count() == 0)
            {
                return Ok(result);
            }
            else
            {
                result.ForEach(item =>
                {
                    var attendenceModel = attendeceList.ToList().Find(x => x.Student == item.StudentId);
                    item.AttendenceDate = attendenceDateParse;
                    item.AttendenceType = attendenceModel.AttendenceType;
                });
                return Ok(result);
            }
        }

      



    }
}