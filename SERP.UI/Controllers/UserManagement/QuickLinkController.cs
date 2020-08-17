using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Entities.UserManagement;
using SERP.Core.Model.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.UserManagement
{
    public class QuickLinkController : Controller
    {
        private readonly IGenericRepository<ModuleMaster, int> _moduleRepo;
        private readonly IGenericRepository<SubModuleMaster, int> _subModuleRepo;
        private readonly IGenericRepository<QuickLink, int> _quickLinkRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public QuickLinkController(IGenericRepository<ModuleMaster, int> moduleRepo, IGenericRepository<SubModuleMaster, int> subModuleRepo, IGenericRepository<QuickLink, int> quickLinkRepo, IGenericRepository<ExceptionLogging, int> exceptionRepo)
        {
            _moduleRepo = moduleRepo;
            _subModuleRepo = subModuleRepo;
            _quickLinkRepo = quickLinkRepo;
            _exceptionLoggingRepo = exceptionRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var models = await _moduleRepo.GetList(x => x.IsActive == 1);
                ViewBag.ModuleList = models;
                return PartialView("~/Views/UserManagement/_QuickLinkPartial.cshtml");
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        public async Task<IActionResult> GetSubModuleDetails(int moduleId)
        {
            try
            {
                List<QuickLinkVm> quickLinkVm = new List<QuickLinkVm>();
                var subModules = await _subModuleRepo.GetList(x => x.ModuleId == moduleId && x.IsActive == 1);
                var quickModules = await _quickLinkRepo.GetList(x => x.IsActive == 1 && x.ModuleId == moduleId);

                if (quickModules!=null && quickModules.Count() > 0)
                {
                    subModules.ToList().ForEach(item =>
                    {
                        QuickLinkVm link = new QuickLinkVm();
                        link.SubModuleId = item.Id;
                        link.SubModuleName = item.SubModuleName;
                        link.ModuleId = item.ModuleId;
                        quickModules.ToList().ForEach(x =>
                        {
                            if (item.Id == x.SubModuleId)
                            {
                                link.IsMapped = true;
                            }
                        });
                        quickLinkVm.Add(link);
                    });
                    return Json(quickLinkVm);
                }
                else
                {
                    subModules.ToList().ForEach(item => {
                        QuickLinkVm link = new QuickLinkVm();
                        link.SubModuleId = item.Id;
                        link.SubModuleName = item.SubModuleName;
                        link.ModuleId = item.ModuleId;
                        link.IsMapped = false;
                        quickLinkVm.Add(link);

                    });
                    return Json(quickLinkVm);
                }
               
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }


        }


        public async Task<IActionResult> SaveSubModuleLinks(string subModuleIds, string moduleIds)
        {
            try
            {
                var ids = subModuleIds.Split(",");
                var modules = moduleIds.Split(","); 
                List<QuickLink> quickLinks = new List<QuickLink>();
                for (int i = 0; i < ids.Count(); i++)
                {
                    QuickLink model = new QuickLink()
                    {
                        SubModuleId = Convert.ToInt32(ids[i]),
                        RoleId = 1,
                        ModuleId =Convert.ToInt32(modules[i])
                    };
                    quickLinks.Add(model);
                }
                var response = await _quickLinkRepo.Add(quickLinks.ToArray());
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.AddedSuccessfully));
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }


    }
}