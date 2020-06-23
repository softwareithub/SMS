using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.OnlineVideo;
using SERP.Core.Model.FeeDetails;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoFeeController : ControllerBase
    {
        private readonly IGenericRepository<OnlineVideoFeeDetail, int> _IOnlineVideoFeeRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectRepo;

        public VideoFeeController(IGenericRepository<OnlineVideoFeeDetail, int> onlineVideoRepo,
          IGenericRepository<CourseMaster, int> courseRepo, IGenericRepository<SubjectMaster, int> _subjectRepo)
        {
            _IOnlineVideoFeeRepo = onlineVideoRepo;
            _ICourseRepo = courseRepo;
            _ISubjectRepo = _subjectRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetVideoFeeDetails()
        {
            var response = (from VF in await _IOnlineVideoFeeRepo.GetList(x => x.IsActive == 1)
                            join CM in await _ICourseRepo.GetList(x => x.IsActive == 1)
                            on VF.CourseId equals CM.Id
                            select new OnlineFeeModel
                            {
                                Id = VF.Id,
                                CourseName = CM.Name,
                                SubjectId = VF.SubjectId,
                                Amount = VF.FeeAmount,
                            }).ToList();

            var subjectDetails = await _ISubjectRepo.GetList(x => x.IsActive == 1);

            response.ForEach(item => {
                if (item.SubjectId != 0)
                {
                    item.SubjectName = subjectDetails.FirstOrDefault(x => x.Id == item.SubjectId).SubjectName;
                }
                else
                {
                    item.SubjectName = item.CourseName;
                }
            });

            return Ok(response);
        }
    }
}