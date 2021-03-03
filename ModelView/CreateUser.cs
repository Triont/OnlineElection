using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OnlineElection.ModelView
{
    public class CreateUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        [Required]
        public string ThirdName { get; set; }
        [Required]
        public DateTime _DateTime { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        //    public long PersonSecureDataId { get; set; }
        [Required]
        public string Pass { get; set; }
        [Required]
        public string AgainPass { get; set; }
    }
}
