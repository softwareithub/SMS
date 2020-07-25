using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.LibraryManagement;
using SERP.Core.Model.LibraryManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.LibraryManagement
{
    public class BookDetailReportController : Controller
    {
        private readonly IBookDetailReport _bookDetailRepo;
        private readonly IGenericRepository<StudentMaster, int> _studentMasterRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<BookMasterModel, int> _bookMasterRepo;
        private readonly IGenericRepository<BookItemModel, int> _bookItemRepo;
        private readonly IGenericRepository<BookTransaction, int> _bookTransactionRepo;
        private readonly IGenericRepository<LibrarySetting, int> _librarySettingRepo;
        public BookDetailReportController(IBookDetailReport bookDetailReport, IGenericRepository<StudentMaster, int> studentRepo, IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo, IGenericRepository<BookMasterModel, int> bookMasterRepo, IGenericRepository<BookItemModel, int> bookItemRepo, IGenericRepository<BookTransaction, int> bookTransactionRepo,
            IGenericRepository<LibrarySetting, int> librarySettingRepo)
        {
            _bookDetailRepo = bookDetailReport;
            _studentMasterRepo = studentRepo;
            _employeeRepo = employeeRepo;
            _bookItemRepo = bookItemRepo;
            _bookMasterRepo = bookMasterRepo;
            _bookTransactionRepo = bookTransactionRepo;
            _librarySettingRepo = librarySettingRepo;
        }
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => PartialView("~/Views/LibraryManagement/LibraryReport/_LibraryReportPartial.cshtml"));
        }

        public async Task<IActionResult> GetBookDetails()
        {
            var response = await _bookDetailRepo.GetBookDetailReport();
            return PartialView("~/Views/LibraryManagement/LibraryReport/BookDetailPartial.cshtml", response);
        }

        public async Task<IActionResult> GetBookTransactionDetail()
        {
            var bookIsuseDetails = (from BT in await _bookTransactionRepo.GetList(x => x.IsActive == 1)
                                    join BI in await _bookItemRepo.GetList(x => x.IsActive == 1)
                                    on BT.BookItemId equals BI.Id
                                    join BM in await _bookMasterRepo.GetList(x => x.IsActive == 1)
                                    on BI.BookId equals BM.Id
                                    select new BookIssueDetailModel
                                    {
                                        Id = BT.Id,
                                        BookItemId = BT.BookItemId,
                                        IssueDate = BT.IssueDate,
                                        ExpectedReturnDate = BT.ExpectedReturnDate,
                                        BookItemName = BI.BookBarCode,
                                        BookName = BM.TitleName,
                                        UserType = BT.UserTypeId,
                                        UserId = BT.UserId,
                                        ActualReturnDate= BT.ActualReturnDate

                                    }).ToList();
            var studentList = await _studentMasterRepo.GetList(x => x.IsActive == 1);
            var employeeList = await _employeeRepo.GetList(x => x.IsActive == 1);

            bookIsuseDetails.ForEach(item =>
            {
                if (item.UserType == 0)
                {
                    item.IssueTo = studentList.ToList().Find(x => x.Id == item.UserId).Name + " (" + "Student" + ") ";
                }
                else
                {
                    item.IssueTo = employeeList.ToList().Find(x => x.Id == item.UserId).Name + " (" + "Employee" + ") ";
                }
            });

            return PartialView("~/Views/LibraryManagement/LibraryReport/_BookTransactionPartial.cshtml", bookIsuseDetails);

        }
    }
}