using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FSD_P2_T02_Group2.Models
{
    public class PendingCounsellorSession
    {

        public int SessionID { get; set; }

        public int Feeling { get; set; }

        public int Thought { get; set; }

        public string Problems { get; set; }

        public DateTime DateCreated { get; set; }

        public int UserID { get; set; }

        public string Alias { get; set; }
    }
}
