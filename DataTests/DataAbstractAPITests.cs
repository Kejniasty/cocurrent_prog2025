using NUnit.Framework;
using Data;

namespace DataTests
{
    [TestFixture]
    public class DataAbstractAPITest
    {
        [Test]
        public void CreateAPITest()
        {
            DataAbstractAPI api = DataAbstractAPI.CreateDataAPI();
            Assert.NotNull(api);
        }
    }
}
