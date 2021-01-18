using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FSD_P2_T02_Group2.DAL
{
    public class PostDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public PostDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "userAccount");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        private FirestoreDb CreateFirestoreDb()
        {
            var projectName = "fir-chat-ukiyo";
            var authFilePath = "/Users/gekteng/Downloads/fir-chat-ukiyo-firebase-adminsdk.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", authFilePath);
            FirestoreDb firestoreDb = FirestoreDb.Create(projectName);
            return FirestoreDb.Create(projectName);
        }
    }
}
