using System.Web;
using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;

namespace FSD_P2_T02_Group2.Models
{
    [FirestoreData]
    public class ChatMessage
    {
        [FirestoreProperty]
        public string Message { get; set; }

        [FirestoreProperty]
        public Timestamp CreatedAt { get; set; }

        [FirestoreProperty]
        public string Alias { get; set; }   
    }
}
