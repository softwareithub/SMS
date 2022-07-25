using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class StudentAttendenceController : ControllerBase
    {
        private readonly IReportRepository _IReportRepository;
        private readonly IGenericRepository<AcademicCalender, int> _IAcademicCalenderRepo;
        private readonly IGenericRepository<AcademicMaster, int> _IAcademicMasterRepo;
        public StudentAttendenceController(IReportRepository reportRepository, 
            IGenericRepository<AcademicCalender, int> academicCalenderRepo, IGenericRepository<AcademicMaster, int>  academicRepo )
        {
            _IReportRepository = reportRepository;
            _IAcademicCalenderRepo = academicCalenderRepo;
            _IAcademicMasterRepo = academicRepo;
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAttendenceDetails(int studentId, int yearId, int monthId)
        {
            var response = await _IReportRepository.GetAttendanceModels(studentId, yearId, monthId);
            return Ok(response);
        }

        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetYears()
        {
            List<int> Year = new List<int>();
            for (int i = 2022; i < 2030; i++)
            {
                Year.Add(i);
            }
            return Ok(Year);
        }

        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAcademicCalender(int sessionId)
        {
            var response = await _IAcademicCalenderRepo.GetList(x => x.IsActive == 1);
            return Ok(response);
        }
    }
}