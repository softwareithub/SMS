using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Master
{
    public class InstituteMasterController : Controller
    {
        private readonly IGenericRepository<InstituteMaster, int> _IGenericRepo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public InstituteMasterController(IGenericRepository<InstituteMaster, int> genericRepository,
                                         IHostingEnvironment hostingEnvironment,
                                         IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IGenericRepo = genericRepository;
            _hostingEnvironment = hostingEnvironment;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _IGenericRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return PartialView(result.FirstOrDefault());
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

        [HttpPost]
        public async Task<IActionResult> Create(InstituteMaster model,IFormFile InstituteLogo)
        {
            try
            {
                model.IsActive = 1;
                model.IsDeleted = 0;
                if (InstituteLogo != null)
                {
                    if (model.InstituteLogo != InstituteLogo.FileName)
                    {
                        model.InstituteLogo = await UploadImage(InstituteLogo);
                    }
                }
                else
                {
                    model.InstituteLogo = string.Empty;
                }


                if (model.Id == 0)
                {
                    model.CreatedBy = 1;
                    model.CreatedDate = DateTime.Now.Date;
                    model.UpdatedDate = DateTime.Now.Date;
                    var result = await _IGenericRepo.CreateEntity(model);
                    return Json("Institute Detail Created Successfully.");
                }
                else
                {
                    model.UpdatedBy = 1;
                    model.UpdatedDate = DateTime.Now.Date;

                    var result = _IGenericRepo.Update(model);
                    return Json("Institute data updated successfully");
                }
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }
        }

        private async Task<string> UploadImage(IFormFile formFile)
        {
            try
            {
                if (formFile.Length > 0)
                {
                    var upload = Path.Combine(_hostingEnvironment.WebRootPath, "Images//");
                    using (FileStream fs = new FileStream(Path.Combine(upload, formFile.FileName), FileMode.Create))
                    {
                        await formFile.CopyToAsync(fs);
                    }
                    return "/Images//" + formFile.FileName;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return string.Empty;
        }
    }
}