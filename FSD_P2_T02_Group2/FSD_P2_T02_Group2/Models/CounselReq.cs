using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FSD_P2_T02_Group2.Models
{
    public class CounselReq
    {
        //1-Low priority, 4-High Priority
        public int Feelings { get; set; }
        public bool Thought { get; set; }
        public string Problems { get; set; }
        public int Queue { get; set; }

    }
}