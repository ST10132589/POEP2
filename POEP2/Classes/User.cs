using System;

namespace POEP2.Classes
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Boolean IsAdmin { get; set; }
    }
}