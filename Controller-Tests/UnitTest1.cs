using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReservationProject.Areas.Admin.Controllers;

namespace Controller_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod()]
        public void IndexTestNotNull()
        {
            SittingController sittingController = new SittingController(null);
            var actualResult = sittingController.Index();

            Assert.IsNotNull(actualResult);
        }
        [TestMethod()]
        public void IndexPopulated()
        {
            var fakeRepo = new Mock<>

            
        }
    }
}
