using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.LibraryManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.CommanJson
{
    public class AdvanceSearchController : Controller
    {
        private readonly IGenericRepository<StudentMaster, int> _studentRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<BookMasterModel, int> _bookRepo;

        public AdvanceSearchController(IGenericRepository<StudentMaster, int> studentRepo, IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo, IGenericRepository<BookMasterModel, int> bookRepo)
        {
            _studentRepo = studentRepo;
            _employeeRepo = employeeRepo;
            _bookRepo = bookRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetStudent()
        {
            List<string> studentList = new List<string>();
            var studentModels = await _studentRepo.GetList(x => x.IsActive == 1);
            studentModels.ToList().ForEach(item =>
            {
                studentList.Add(item.Name + " (" + item.RegistrationNumber + ") ");
            });

            return Json(studentList);

        }

        public async Task<IActionResult> GetEmployeeDetails()
        {
            List<string> employeeList = new List<string>();
            var employeeDetails = await _employeeRepo.GetList(x => x.IsActive == 1);
            employeeDetails.ToList().ForEach(item =>
            {
                employeeList.Add(item.Name + " (" + item.EmpCode + ") ");
            });
            return Json(employeeList);
        }

        public async Task<IActionResult> GetBookDetails()
        {
            var bookDetails = await _bookRepo.GetList(x => x.IsActive == 1);
            List<string> bookList = new List<string>();

            bookDetails.ToList().ForEach(item => {
                bookList.Add(item.TitleName + " ("+item.AuthorName+") ");
            });

            return Json(bookList);
        }
    }
}