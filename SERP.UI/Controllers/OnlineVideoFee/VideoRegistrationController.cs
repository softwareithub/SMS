using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.HomeAssignment;
using SERP.Core.Entities.OnlineVideo;
using SERP.Core.Model.OnlineVideo;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.OnlineVideoFee
{
    public class VideoRegistrationController : Controller
    {
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<SubjectMaster, int> _IsubjectRepo;
        private readonly IGenericRepository<VideoRegistration, int> _IVideoRegistration;
        private readonly IGenericRepository<StudyMaterial, int> _IStudyMaterialRepo;

        public VideoRegistrationController(IGenericRepository<CourseMaster, int>  courseRepo, 
            IGenericRepository<SubjectMaster, int> subjectRepo,
            IGenericRepository<VideoRegistration, int> videoRegistrationRepo, IGenericRepository<StudyMaterial, int>  studyMaterialRepo)
        {
            _ICourseRepo = courseRepo;
            _IsubjectRepo = subjectRepo;
            _IVideoRegistration = videoRegistrationRepo;
            _IStudyMaterialRepo = studyMaterialRepo;
        }
        public async Task<IActionResult> Index()
        {
            return PartialView("");
        }

        public async Task<IActionResult> GetVideoSubscription()
        {
            ViewBag.CourseList = await _ICourseRepo.GetList(x => x.IsActive == 1);
            return PartialView("");
        }
        public async Task<IActionResult> GetVideoDetail()
        {
            var response = (from SM in await _IStudyMaterialRepo.GetList(x => x.IsActive == 1 && x.UploadType.Trim().ToLower() == "video")
                            join CM in await _ICourseRepo.GetList(x => x.IsActive == 1)
                            on SM.CourseId equals CM.Id
                            join SMN in await _IsubjectRepo.GetList(x => x.IsActive == 1)
                            on SM.SubjectId equals SMN.Id
                            select new VideoRegistrationModel
                            {
                                Id= SM.Id,
                                MaterialName= SM.MaterialName,
                                Description= SM.MaterialDescription,
                                CourseName= CM.Name,
                                SubjectName= SMN.SubjectName,
                                VideoPath= SM.MaterialPath
                            }).ToList();
            return PartialView("", response);
        }
    }
}