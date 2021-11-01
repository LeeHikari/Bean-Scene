using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReservationProject.Areas.Admin.Controllers;
using ReservationProject.Data;
using System;
using System.Threading.Tasks;

namespace Controller_Tests
{
    [TestClass]
    public class UnitTest1
    {
        private ApplicationDbContext GetDatabaseContext()
        {
            var connectionstring = "Data Source = localhost; Initial Catalog = Reservation - bs; Integrated Security = True; MultipleActiveResultSets = true";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            ApplicationDbContext dbContext = new ApplicationDbContext(optionsBuilder.Options);
            return dbContext;


        }
        [TestMethod()]
        public void CreateSitting()
        {

            var context = GetDatabaseContext();
            //var sitting = new Sitting();
            var model = new ReservationProject.Areas.Admin.Models.Sitting.Create
            { Name = "Test",
                SitStartTime = DateTime.Now,
                SitEndTime = DateTime.Now.AddHours(3),
                Capacity = 30,
                RestaurantId = 1,
                IsClosed = false,
             };

            SittingController controller = new SittingController(context);



            // Act
            Task<IActionResult> task = controller.Create(model);



            // Assert

            Assert.IsNotNull(context.Sittings.FirstOrDefaultAsync(s => s.Name == "Test"));



        }
        //[TestMethod()]
        //public void IndexPopulated()
        //{
        //    var fakeRepo = new Mock<>

            
        //}
    }
}
