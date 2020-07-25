using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.Transport;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Transport
{
    public class RouteStudentMappingController : Controller
    {
        private readonly IGenericRepository<RouteStudentMapping, int> _routeStudentMappingRepo;
        private readonly IGenericRepository<RouteMaster, int> _routeRepo;
        private readonly IGenericRepository<RouteStopageModel, int> _routeStopageRepo;
        private readonly IGenericRepository<StudentMaster, int> _studentMasterRepo;
        private readonly IGenericRepository<StopageModel, int> _stopageRepo;
        public RouteStudentMappingController(IGenericRepository<RouteStudentMapping, int> routeStudentRepo, IGenericRepository<RouteMaster, int> routeRepo, IGenericRepository<RouteStopageModel, int> routeStopageRepo, IGenericRepository<StudentMaster, int> studentMasterRepo, IGenericRepository<StopageModel, int> stopageRepo)
        {
            this._routeStudentMappingRepo = routeStudentRepo;
            this._routeRepo = routeRepo;
            this._routeStopageRepo = routeStopageRepo;
            this._studentMasterRepo = studentMasterRepo;
            this._stopageRepo = stopageRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.RouteDetails = await _routeRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/Transport/_RouteStudentMappingPartial.cshtml");
        }

        public async Task<IActionResult> GetStopageDetails(int routeId)
        {
            List<StopageModel> models = new List<StopageModel>();
            var routestopageModels = await _routeStopageRepo.GetList(x => x.IsActive == 1);
            var stopageModel = await _stopageRepo.GetList(x => x.IsActive == 1);
            models = (from RS in routestopageModels
                      join SM in stopageModel
                      on RS.StopageId equals SM.Id
                      where RS.RouteId == routeId
                      select new StopageModel
                      {
                          Id = SM.Id,
                          StopageName = SM.StopageName
                      }).ToList();

            return Json(models);
        }

        public async Task<IActionResult> RouteStudents()
        {
            return await Task.Run(() => PartialView("~/Views/Transport/RouteStudentSearchPartial.cshtml"));
        }

        public async Task<IActionResult> GetStudentList(string name, string rollCode, string fatherName, string motherName, string registrationNUmber)
        {
            var studentModels = await _studentMasterRepo.GetList(x => x.IsActive == 1);
            if (!string.IsNullOrEmpty(name))
            {
                studentModels = studentModels.Where(x => x.Name.Trim().ToLower() == name.Trim().ToLower()).ToList();
            }
            if (!string.IsNullOrEmpty(rollCode))
            {
                studentModels = studentModels.Where(x => x.RollCode.Trim().ToLower() == rollCode.Trim().ToLower()).ToList();
            }
            if (!string.IsNullOrEmpty(fatherName))
            {
                studentModels = studentModels.Where(x => x.FatherName.Trim().ToLower() == fatherName.ToLower().Trim()).ToList();
            }
            if (!string.IsNullOrEmpty(motherName))
            {
                studentModels = studentModels.Where(x => x.MotherName.Trim().ToLower() == motherName.ToLower().Trim()).ToList();
            }
            if (!string.IsNullOrEmpty(registrationNUmber))
            {
                studentModels = studentModels.Where(x => x.RegistrationNumber.Trim().ToLower() == registrationNUmber.ToLower().Trim()).ToList();
            }

            return PartialView("~/Views/Transport/StudentSearchPartial.cshtml", studentModels);
        }

        [HttpPost]
        public async Task<IActionResult> StudentRouteMapp(string RouteName, string Stopage, string[] StudentId)
        {
            if (RouteName != "0" || Stopage != "0")
            {
                List<RouteStudentMapping> models = new List<RouteStudentMapping>();
                for (int i = 0; i < StudentId.Count(); i++)
                {
                    RouteStudentMapping model = new RouteStudentMapping()
                    {
                        StopageId = Convert.ToInt32(Stopage),
                        RouteId = Convert.ToInt32(RouteName),
                        StudentId = Convert.ToInt32(StudentId[i]),
                        CreatedBy = 1,
                        CreatedDate = DateTime.Now.Date
                    };
                    models.Add(model);
                }
                return Json(ResponseData.Instance.GenericResponse(await _routeStudentMappingRepo.Add(models.ToArray())));
            }
            else
            {
                return Json("Route Or Stopage are not selected.Please select or route.");
            }


        }

        public async Task<IActionResult> GetMappedStudent(int routeId, int stopageId)
        {
            var mappedStudentModels = await _routeStudentMappingRepo.GetList(x => x.IsActive == 1 && x.RouteId == routeId && x.StopageId == stopageId);
            var studentModels = await _studentMasterRepo.GetList(x => x.IsActive == 1);
            List<StudentMaster> models = new List<StudentMaster>();
            studentModels.ToList().ForEach(item =>
            {
                mappedStudentModels.ToList().ForEach(x =>
                {
                    if(item.Id==x.StudentId)
                    {
                        models.Add(item);
                    }
                });
            });

            return PartialView("~/Views/Transport/AllocatedStudentRoute.cshtml", models);
        }
    }
}