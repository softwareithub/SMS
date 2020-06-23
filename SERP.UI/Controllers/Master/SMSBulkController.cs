using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.Master
{
    public class SMSBulkController : Controller
    {
        private readonly IGenericRepository<SMSBulk, int> _ISMSbulkRepo;
        public SMSBulkController(IGenericRepository<SMSBulk, int> smsBulkRepo)
        {
            _ISMSbulkRepo = smsBulkRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}