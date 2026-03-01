using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public class User
    {
        public int UserID { get; private set; }
        public int PersonID { get; private set; }
        public Person PersonInfo { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string RefreshTokenHash { get; set; }
        public DateTime RefreshTokenExpiredAt { get; set; }
        public DateTime? RefreshTokenRevokedAt { get; set; }

        // This constructor for EF Core to can create instance from user class
        private User() { }
        public User(Person person, int personID ,int userID)
        {
            PersonInfo = person;
            PersonID = personID;
            UserID = userID;
        }
    }
}
