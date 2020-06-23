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
        public async Task Test1()
        {
            await  new UploadVideo().UploadVideoToYouTube();
            Assert.Pass();
        }
    }
}