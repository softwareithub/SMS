using BarCodeGenerator;
using NUnit.Framework;
using System.Threading.Tasks;
using UploadVideoYouTube;

namespace SERP.UnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            BarCodeHelper barCodeHelper = new BarCodeHelper();
            var barCode=barCodeHelper.ReadQRCode();
            Assert.Pass();
        }
    }
}