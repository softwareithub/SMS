using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.HomeAssignment;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudyMaterialController : ControllerBase
    {
        public readonly IGenericRepository<StudentPromote, int> _IStudentPromoteRepo;
        public readonly IGenericRepository<StudyMaterial, int> _IStudyMaterialRepo;
        public readonly IGenericRepository<SubjectMaster, int> _ISubjectRepo;

        public StudyMaterialController(IGenericRepository<StudentPromote, int>  studentPromoteRepo,
            IGenericRepository<StudyMaterial, int> studyRepo, IGenericRepository<SubjectMaster, int> subjectRepo)
        {
            _IStudentPromoteRepo = studentPromoteRepo;
            _IStudyMaterialRepo = studyRepo;
            _ISubjectRepo = subjectRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubjetc(int studentId)
        {
            var studentDetail = await _IStudentPromoteRepo.GetSingle(x => x.StudentId == studentId);
            return Ok(await _ISubjectRepo.GetList(x => x.CourseId == studentDetail.CourseId));
        }

        [HttpGet]
        public async Task<IActionResult>GetStudyMaterial(int subjectId)
        {
            return Ok(await _IStudyMaterialRepo.GetList(z => z.IsActive == 1 && z.SubjectId == subjectId));
        }

        [HttpGet]
        public async Task<IActionResult> GetDocument(int id)
        {
            return Ok(await _IStudyMaterialRepo.GetSingle(x => x.Id == id));
        }

    }
}