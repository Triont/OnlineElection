using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineElection.ModelView
{
    public class NewPass

    {
        public long UserId { get; set; }
        [Required]
        public string Pass { get; set; }

        [Required]
        [Compare(nameof(Pass), ErrorMessage = "Passwords do not match")]
        public string ConfirmedPass { get; set; }
    }
}
