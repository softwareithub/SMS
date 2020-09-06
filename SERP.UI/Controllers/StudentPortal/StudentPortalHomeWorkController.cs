using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.HomeAssignment;
using SERP.Core.Model.AssignmentHomeModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.StudentPortal
{
    public class StudentPortalHomeWorkController : Controller
    {
        private readonly IGenericRepository<HomeWorkModel, int> _IHomeWorkRepository;
        private readonly IGenericRepository<HomeWorkAllocationModel, int> _IHomeWorkSubmmisionRepository;

        public StudentPortalHomeWorkController(IGenericRepository<HomeWorkModel,int> homeWorkRepository, IGenericRepository<HomeWorkAllocationModel, int> homeWorkSubmissionRepository)
        {
            _IHomeWorkRepository = homeWorkRepository;
            _IHomeWorkSubmmisionRepository = homeWorkSubmissionRepository;
        }
        public IActionResult Index()
        {
            return PartialView("~/Views/StudentPortal/StudentHomeWork/_HomeWorkDetailPartial.cshtml");
        }
    }
}