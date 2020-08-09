using SERP.Core.Entities.SERPExceptionLogging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Utilities.ExceptionHelper
{
    public class LoggingHelper
    {
        public ExceptionLogging GetExceptionLoggingObj(string actionName,string controllerName,string exceptionMessage,string exceptionNature,int isResolved)
        {
            ExceptionLogging logging = new ExceptionLogging()
            {
                ActionName = actionName,
                ControllerName = controllerName,
                ExceptionMessage = exceptionMessage,
                ExceptionNature = exceptionNature,
                IsResolved = isResolved
            };
            return logging;
        }
    }
}
