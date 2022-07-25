using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.LibraryManagement;
using SERP.Core.Model.LibraryManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LibraryManagementApi : ControllerBase
    {
        private readonly IGenericRepository<StudentMaster, int> _studentMasterRepo;
        private readonly IGenericRepository<BookMasterModel, int> _bookMasterRepo;
        private readonly IGenericRepository<BookItemModel, int> _bookItemRepo;
        private readonly IGenericRepository<BookTransaction, int> _bookTransactionRepo;
        private readonly IGenericRepository<LibrarySetting, int> _librarySettingRepo;


        public LibraryManagementApi(IGenericRepository<StudentMaster, int> studentRepo,
                                IGenericRepository<BookMasterModel, int> bookMasterRepo,
                                IGenericRepository<BookItemModel, int> bookItemRepo,
                                IGenericRepository<BookTransaction, int> bookTransactionRepo,
                                IGenericRepository<LibrarySetting, int> librarySettingRepo
                           )
        {
            _studentMasterRepo = studentRepo;
            _bookItemRepo = bookItemRepo;
            _bookMasterRepo = bookMasterRepo;
            _bookTransactionRepo = bookTransactionRepo;
            _librarySettingRepo = librarySettingRepo;
        }

        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetBookIssue(int studentId)
        {
            var bookIsuseDetails = (from BT in await _bookTransactionRepo.GetList(x => x.IsActive == 1)
                                    join BI in await _bookItemRepo.GetList(x => x.IsActive == 1)
                                    on BT.BookItemId equals BI.Id
                                    join BM in await _bookMasterRepo.GetList(x => x.IsActive == 1)
                                    on BI.BookId equals BM.Id
                                    where BT.ActualReturnDate.Year == 1
                                    && BT.UserId==studentId && BT.UserTypeId==0
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
                                        BookActualReturnDate= BT.ActualReturnDate.ToString(),
                                        BookIssueDate=BT.IssueDate.ToString(),
                                        BookReturnDate= BT.ExpectedReturnDate.ToString()
                                    }).ToList();
            var studentList = await _studentMasterRepo.GetList(x => x.IsActive == 1);

            bookIsuseDetails.ForEach(item =>
            {
                if (item.UserType == 0)
                {
                    item.IssueTo = studentList.ToList().Find(x => x.Id == item.UserId).Name + " (" + "Student" + ") ";
                }
               
            });

            return Ok(bookIsuseDetails);

        }
    }
}
