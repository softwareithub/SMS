using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.Master
{
    public class InstituteMasterController : Controller
    {
        private readonly IGenericRepository<InstituteMaster, int> _IGenericRepo;
        private readonly IHostingEnvironment _hostingEnvironment;

        public InstituteMasterController(IGenericRepository<InstituteMaster, int> genericRepository, IHostingEnvironment hostingEnvironment)
        {
            _IGenericRepo = genericRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _IGenericRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return PartialView(result.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Create(InstituteMaster model,IFormFile InstituteLogo)
        {
            model.IsActive = 1;
            model.IsDeleted = 0;
            model.InstituteLogo =model.InstituteLogo?? await UploadImage(InstituteLogo);
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
                    return "Images//" + formFile.FileName;
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