using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSD_P2_T02_Group2.Models
{
    public class PostViewModel
    { 
        public Post post { get; set; }
        public string Image { get; set; }
        public string id { get; set; }
        public string Category { get; set; }
        //public List<SelectListItem> chooseCategory { get; set; }
        //public List<Post> postList { get; set; }
        //public List<String> postMediaList { get; set; }

        public PostViewModel()
        {
            post = new Post();
        }
    }
}
