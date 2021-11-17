using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using ReservationProject.Data;
using ReservationProject.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ReservationSystemTesting.Controllers
{
    [TestClass()]
    public class HomeControllerTests
    {

        private readonly DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = 
            new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Data Source=localhost;Initial Catalog=Reservation-bs;Integrated Security=True;MultipleActiveResultSets=true");

        [TestMethod("1. Check If Database is accessible")]
        public void DatabaseNotNull()
        {
            var db = new ApplicationDbContext(optionsBuilder.Options);

            Assert.IsNotNull(db);
        }

        [TestMethod("2. Database contains Restaurant Named Bean Scene")]
        public void DatabaseContainsCorrectData()
        {

            var db = new ApplicationDbContext(optionsBuilder.Options);

            var restaurants = db.Restaurants.Include(r => r.Id);

            var results = db.Restaurants.FirstOrDefaultAsync();

            Assert.IsTrue(results.Result.Name == "Bean Scene");

        }

        [TestMethod("3. Check If Index is accessible")]
        public void IndexTestNonNull()
        {

            HomeController controller = new HomeController(null, null, null, null, null);

            var actualResult = controller.Index();

            Assert.IsNotNull(actualResult);
        }

        [TestMethod("4. Title contains Home")]
        public void IndexTitleTest()
        {
            //Arrange
            HomeController controller = new HomeController(null, null, null, null, null);
            //Act
            ViewResult actualResult = (ViewResult)controller.Index().Result;

            //Assert
            Assert.IsNotNull(actualResult.ViewData["Title"]);
            Assert.IsInstanceOfType(actualResult.ViewData["Title"], typeof(string));
            Assert.IsTrue(actualResult.ViewData["Title"].ToString().Contains("Home"));
        }

        [TestMethod("5. Check If Admin Index is accessible")]
        public void AdminView()
        {
            //Arrange
            var db = new ApplicationDbContext(optionsBuilder.Options);

            ReservationProject.Areas.Admin.Controllers.HomeController controller = new ReservationProject.Areas.Admin.Controllers.HomeController(db);

            //Act
            ViewResult actualResult = (ViewResult)controller.Index();

            //Assert
            Assert.IsNotNull(actualResult.ViewData["Title"]);
            Assert.IsInstanceOfType(actualResult.ViewData["Title"], typeof(string));
            Assert.IsTrue(actualResult.ViewData["Title"].ToString().Contains("Admin"));
        }

    }
}
