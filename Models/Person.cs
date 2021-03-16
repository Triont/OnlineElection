using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OnlineElection.Models
{
    public class Person
    {
        public long Id { get; set; }

        [Required(ErrorMessage ="Not correct")]
        
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Not correct")]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "Not correct")]
        public string ThirdName { get; set; }
        [Required]
        public DateTime _DateTime { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string  WasVotedId { get; set; }
    //    public long PersonSecureDataId { get; set; }
        [Required]
        public string Pass { get; set; }
        public string Salt { get; set; }
        public bool EmailWasConfirmed { get; set; }
    }

    public class PersonSecureData
    {
        public long Id { get; set; }
        public string Pass { get; set; }
        public string Salt { get; set; }
    }
}
