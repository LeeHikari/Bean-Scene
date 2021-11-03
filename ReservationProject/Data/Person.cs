namespace ReservationProject.Data
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Fullname { get => FirstName + " " + LastName; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserId { get; set; }

    }
}