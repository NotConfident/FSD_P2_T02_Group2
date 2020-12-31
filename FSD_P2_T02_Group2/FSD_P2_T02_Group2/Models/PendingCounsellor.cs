using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FSD_P2_T02_Group2.Models
{
    public class PendingCounsellor
    {

        [Required(ErrorMessage = "Name must be provided.")]
        public string Name { get; set; }

        public string Email { get; set; }

        public int PhoneNumber { get; set; }

        public DateTime DateBirth { get; set; }

        public string Image { get; set; }

        public string Certificate { get; set; }

    }
}
