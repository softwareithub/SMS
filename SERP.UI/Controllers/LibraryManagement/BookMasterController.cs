using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.LibraryManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.LibraryManagement
{
    public class BookMasterController : Controller
    {
        private readonly IGenericRepository<BookMasterModel, int> _IBookRepo;
        public BookMasterController(IGenericRepository<BookMasterModel, int> bookRepo)
        {
            _IBookRepo = bookRepo;
        }
        public async Task<IActionResult> CreateBook(int id)
        {
            return PartialView("~/Views/LibraryManagement/_BookMasterPartial.cshtml",await _IBookRepo.GetSingle(x=>x.Id==id));
        }
    }
}