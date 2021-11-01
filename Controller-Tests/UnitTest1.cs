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
        public void ReservationController()
        {
            var sittingController = new SittingController(null);

            var result = sittingController.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            //var viewResult = result as ViewResult;
            //Assert.AreEqual(typeof(AboutViewItem), viewResult);
            

            



        }
    }
}
