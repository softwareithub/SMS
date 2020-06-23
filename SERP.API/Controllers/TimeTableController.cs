using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TimeTableController : ControllerBase
    {
        private readonly ITimeSheetRepo _timeSheetRepo;
        public readonly IGenericRepository<StudentPromote, int> _IStudentPromoteRepo;

        public TimeTableController(ITimeSheetRepo timeSheetRepo, IGenericRepository<StudentPromote, int> studentPromoteRepo)
        {
            _timeSheetRepo = timeSheetRepo;
            _IStudentPromoteRepo = studentPromoteRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetTimeTableDetails(int studentId)
        {
            var studentPromote = await _IStudentPromoteRepo.GetSingle(x => x.IsActive == 1 && x.StudentId == studentId);
            return Ok( await _timeSheetRepo.GetTimeSheetDetailsByCourseIdBatchId(studentPromote.CourseId, studentPromote.BatchId));
        }
    }
}