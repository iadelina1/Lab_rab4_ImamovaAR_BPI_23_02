using System;

namespace Lab_rab5_ImamovaAR_BPI_23_02
{
    public class Person
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public Person() { }

        public Person(int id, int roleId, string firstName, string lastName, DateTime birthday)
        {
            Id = id;
            RoleId = roleId;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
        }
    }
}