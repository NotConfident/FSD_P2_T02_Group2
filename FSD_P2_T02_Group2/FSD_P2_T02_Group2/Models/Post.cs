using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSD_P2_T02_Group2.Models
{
    [FirestoreData]
    public class Post
    {
        //[FirestoreProperty]
        //public int PostID { get; set; }

        [FirestoreProperty]
        public int UserID { get; set; }

        //[FirestoreProperty]
        //public string UserDP { get; set; }

        [FirestoreProperty]
        public string Description { get; set; }

        [FirestoreProperty]
        public int[] Likes { get; set; }

        //[FirestoreProperty]
        //public string Tag { get; set; }

        //[FirestoreProperty]
        //public string Media { get; set; }
        
        public DateTime TimeCreated { get; set; }

        [FirestoreProperty]
        public bool hasMedia { get; set; }

        [FirestoreProperty]
        public string Tag { get; set; }
    }
}