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
        [FirestoreProperty]
        public int PostID { get; set; }

        [FirestoreProperty]
        public int UserID { get; set; }

        [FirestoreProperty]
        public string Description { get; set; }

        [FirestoreProperty]
        public int[] Likes { get; set; }

        [FirestoreProperty]
        public string[] Media { get; set; }

        [FirestoreProperty]
        public string[] Tags { get; set; }

        [FirestoreProperty]
        public DateTime TimeCreated { get; set; }
    }
}