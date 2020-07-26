﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Utilities
{
    public static class SqlConstant
    {
        #region TimeSheet
        public static  string GetTimeSheet = @"select MasterId,AttendenceId, CourseName,BatchName, EmployeeName, Photo, Phone, PeriodName, FromTime, ToTime, SubjectName,TimeTableDay, TeacherAttendence,CourseId,BatchId,TeacherId, Id from HR.Vw_EmployeeBatchTimeSheet where CourseId=@courseId and BatchId=@batchId";
        public static string GetTeacherBySubjectId = @"exec HR.usp_GetTeacherBySubjectId @subjectId";
        public static string GetFreeTeacher = @"select MasterId,AttendenceId, CourseName,BatchName, EmployeeName, Photo, Phone, PeriodName, FromTime, ToTime, SubjectName,TimeTableDay, TeacherAttendence,CourseId,BatchId,TeacherId, Id from HR.Vw_EmployeeBatchTimeSheet where FromTime<>@fromTime and ToTime <> @toTime  and TeacherAttendence='P'";

        public static string GetFeeDepositReciept = @"usp_GetStudentFeeDeposiDetail";
        #endregion
    }
}