﻿namespace ProjetoFinal.Models
{
    public class ProfileViewModel
    {

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public string Gender { get; set; }
        public int Mobile { get; set; }

        public IEnumerable<Publication> Publications { get; set; }
    }
}