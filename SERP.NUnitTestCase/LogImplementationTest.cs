using NUnit.Framework;
using SERP.Core.Entities.TransactionLog;
using SERP.Infrastructure.Implementation.Infratructure.Implementation;
using SERP.Utilities.ResponseUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.NUnitTestCase
{
    public class LogImplementationTest
    {
        [Test]
        public async Task TestLogTransaction()
        {
            var expectedResult = ResponseStatus.AddedSuccessfully;
            LogMaster logMaster = new LogMaster()
            {
                CompleteLog = "complete Log Test",
                CreatedBy = 1,
                CreatedDate = DateTime.Now.Date,
                InnerException = "Test Inner exception",
                Message = "Test Message",
                StackTrace = "Stack Trace"
            };
            var result = await new LogImplementation().CreateEntity(logMaster);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
