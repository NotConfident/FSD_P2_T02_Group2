﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FSD_P2_T02_Group2.Models
{
    public class Counsellor
    {
        public int counsellorID { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime DateCreated { get; set; }

        public string PhoneNumber { get; set; }

        public string? Image { get; set; }
        
        public string? Certificate { get; set; }

        public DateTime DateBirth { get; set; }

        public float? AvgRating { get; set; }

        public string? Status { get; set; }
    }
}
