using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReservationProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemTesting.Controllers
{
    [TestClass()]
    public class ReservationControllerTests
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

        [TestMethod("2. No Current Test Data")]
        public void DatabaseNoTestData()
        {

            var db = new ApplicationDbContext(optionsBuilder.Options);

            var result = db.Reservations.FirstOrDefault(r => r.Note == "Unit Testing");

            Assert.IsTrue(result == null);

        }

        [TestMethod("3. Reservation Controller Adds Reservation")]
        public void TestControllerAddsReservation()
        {

            var db = new ApplicationDbContext(optionsBuilder.Options);

            Person person = new Person
            {
                Id = 4,
                Email = "testdummy@bs.com",
                FirstName = "Test",
                LastName = "Dummy",
                Phone = "9999999999",
                UserId = "1"
            };

            //create new reservation assign the person id

            var reservation = new Reservation
            {
                StartTime = new DateTime(),
                Duration = 90,
                PersonId = person.Id,
                Guests = 3,
                ReservationSourceId = 4,
                ReservationStatusId = 1,
                SittingId = 2,
                Note = "Unit Testing"

            };

            db.Reservations.Add(reservation);
            var results = db.SaveChangesAsync();

            Assert.IsTrue(results.Result >= 1);
        }

        [TestMethod("4. Compare if Correct Data")]
        public void DatabaseCorrectTestData()
        {
            var db = new ApplicationDbContext(optionsBuilder.Options);

            var result = db.Reservations.FirstOrDefault(r => r.Note == "Unit Testing");

            Assert.IsTrue(result != null);

        }
    }
}
