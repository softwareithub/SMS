using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Utilities
{
    public static class SqlConstant
    {
        #region TimeSheet
        public static  string GetTimeSheet = @"select MasterId,AttendenceId, CourseName,BatchName, EmployeeName, Photo, Phone, PeriodName, FromTime, ToTime, SubjectName,TimeTableDay, TeacherAttendence from HR.Vw_EmployeeBatchTimeSheet";
        #endregion
    }
}
