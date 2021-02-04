using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FSD_P2_T02_Group2.Models;
using FSD_P2_T02_Group2.DAL;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;
using Google.Cloud.Firestore;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Exceptions;

namespace FSD_P2_T02_Group2.DAL
{
    public class UserDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public UserDAL()
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

        public List<User> GetUsers()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM [User]";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<User> userList = new List<User>();
            while (reader.Read())
            {
                userList.Add(
                new User
                {
                    UserID = reader.GetInt32(0),
                    Username = !reader.IsDBNull(1) ? reader.GetString(1) : null,
                    Name = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                    Alias = !reader.IsDBNull(3) ? reader.GetString(3) : null,
                    Password = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                    Email = !reader.IsDBNull(5) ? reader.GetString(5) : null,
                    PhoneNo = !reader.IsDBNull(7) ? reader.GetString(7) : null,
                    Image = !reader.IsDBNull(8) ? reader.GetString(8) : null,
                    DateCreated = reader.GetDateTime(6),
                    Status = !reader.IsDBNull(9) ? reader.GetString(9) : null
                });
            }
            reader.Close();
            conn.Close();
            return userList;
        }

        public User GetUser(int id)
        {
            User user = new User();

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM [User]
                                WHERE UserID = @userid";

            cmd.Parameters.AddWithValue("@userid", id);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user.UserID = reader.GetInt32(0);
                    user.Username = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    user.Name = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    user.Alias = !reader.IsDBNull(3) ? reader.GetString(3) : null;
                    user.Password = !reader.IsDBNull(4) ? reader.GetString(4) : null;
                    user.Email = !reader.IsDBNull(5) ? reader.GetString(5) : null;
                    user.PhoneNo = !reader.IsDBNull(7) ? reader.GetString(7) : null;
                    user.Image = !reader.IsDBNull(8) ? reader.GetString(8) : null;
                    user.Status = !reader.IsDBNull(9) ? reader.GetString(9) : null;
                    //user.Username = reader.GetString(1);
                    //user.Name = reader.GetString(2);
                    //user.Alias = reader.GetString(3);
                    //user.Password = reader.GetString(4);
                    //user.Email = reader.GetString(5);
                    //user.PhoneNo = reader.GetString(7);
                    //user.ProfilePicture = reader.GetString(8);
                    //user.Status = reader.GetString(9);
                }
            }
            reader.Close();
            conn.Close();
            return user;
        }

        public User CheckLogin(string username, string password)
        {
            User user = new User();

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"EXEC uspUserLogin @username, @password";
            //cmd.CommandText = @"SELECT * FROM [User]
            //                    WHERE Username = @username AND Password = @password";

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user.UserID = reader.GetInt32(0);
                    user.Username = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    user.Name = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    user.Alias = !reader.IsDBNull(3) ? reader.GetString(3) : null;
                    user.Password = !reader.IsDBNull(4) ? reader.GetString(4) : null;
                    user.Email = !reader.IsDBNull(5) ? reader.GetString(5) : null;
                    user.PhoneNo = !reader.IsDBNull(7) ? reader.GetString(7) : null;
                    user.Image = !reader.IsDBNull(8) ? reader.GetString(8) : null;
                    user.Status = !reader.IsDBNull(9) ? reader.GetString(9) : null;
                }
            }
            reader.Close();
            conn.Close();
            return user;
        }

        public void RegisterUser(User user)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO [User](Username, Name, Alias, Password, Email, PhoneNo, DateCreated)
                                VALUES(@username, @name, @alias, @password, @email, @phoneNo, @date)";

            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@alias", user.Alias);
            cmd.Parameters.AddWithValue("@phoneNo", user.PhoneNo);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public int UpdateUser(User user)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"UPDATE [User] SET Password = @password, Name = @name, Alias = @alias, Status = @status, PhoneNo = @phoneno, Image = @image WHERE UserID = @userid";
            cmd.Parameters.AddWithValue("@userid", user.UserID);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@alias", user.Alias);
            cmd.Parameters.AddWithValue("@phoneno", user.PhoneNo);
            if (String.IsNullOrEmpty(user.Status))
            {
                cmd.Parameters.AddWithValue("@status", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@status", user.Status);
            }
            if (String.IsNullOrEmpty(user.Image))
            {
                cmd.Parameters.AddWithValue("@image", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@image", user.Image);
            }

            conn.Open();
            int count = cmd.ExecuteNonQuery();
            System.Diagnostics.Debug.WriteLine("Number of rows affected in update:" + count);
            conn.Close();
            return count;
        }

        public int CountUser()
        {
            int? count = 0;
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT COUNT(userID) FROM  [User]";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    count = !reader.IsDBNull(0) ? (int?)reader.GetInt32(0) : null;
                }
            }
            reader.Close();
            conn.Close();   
            return count.Value;
        }





        public void reqHelp(int userid, CounselReq c)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO PendingCounsellingSession(Feeling,Thought,Problems,DateCreated,UserID)
                                        VALUES(@feelings,@thought,@problems,@datetime,@userID)";
            cmd.Parameters.AddWithValue("@feelings", c.Feelings);
            cmd.Parameters.AddWithValue("@thought", c.Thought);
            cmd.Parameters.AddWithValue("@problems", c.Problems);
            cmd.Parameters.AddWithValue("@datetime", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@userID", userid);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public bool checkReq(int userid)
        {
            SqlCommand cmd = conn.CreateCommand();
            bool inQueue = false;
            cmd.CommandText = @"SELECT * FROM PendingCounsellingSession WHERE UserID = @userID";
            cmd.Parameters.AddWithValue("@userID", userid);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                inQueue = true;
            }
            reader.Close();
            conn.Close();
            return inQueue;
        }
        public int getSessions()
        {
            int? count = 0;
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT COUNT(*) FROM  PendingCounsellingSession";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    count = !reader.IsDBNull(0) ? (int?)reader.GetInt32(0) : null;
                }
            }
            reader.Close();
            conn.Close();   
            return count.Value;
        }
        public async Task sendMessage(User user, ChatMessage message, string room)
        {

            var firestoreDb = CreateFirestoreDb();
            
            await firestoreDb.Collection(room).AddAsync(new ChatMessage
            {
                Alias = user.Alias,
                CreatedAt = Google.Cloud.Firestore.Timestamp.FromDateTime(DateTime.UtcNow),
                Message = message.Message
        });
        }
        public async Task sendCMessage(string alias, ChatMessage message, string room)
        {

            var firestoreDb = CreateFirestoreDb();

            await firestoreDb.Collection("CounsellingChat").Document(room).Collection("Messages").AddAsync(new ChatMessage
            {
                Alias = alias,
                CreatedAt = Google.Cloud.Firestore.Timestamp.FromDateTime(DateTime.UtcNow),
                Message = message.Message
            });
        }

        private FirestoreDb CreateFirestoreDb()
        { 
            var projectName = "fir-chat-ukiyo";
            //var authFilePath = "/Users/Ivan/Desktop/WeiJie Ang/Year 2/SEM 2/FSD/fir-chat-ukiyo-firebase-adminsdk.json";
            var authFilePath = "/Users/joeya/Downloads/NP_ICT/FSD & P2/fir-chat-ukiyo-firebase-adminsdk.json";
            //var authFilePath = "/Users/jaxch/Downloads/fir-chat-ukiyo-firebase-adminsdk.json"; 
            //var authFilePath = "/Users/gekteng/Downloads/fir-chat-ukiyo-firebase-adminsdk.json"; 
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", authFilePath);
            FirestoreDb firestoreDb = FirestoreDb.Create(projectName);
            Console.WriteLine("Created Firestore");
            return FirestoreDb.Create(projectName);
            
        }

        public string OTP(string number)
        {
            const string accountSID = "ACb2940c2a00ccdd56852ced467d8789b2";
            const string authToken = "b00a4373595c7bb516da7a8c6a920557";
            // Initialize the TwilioClient.
            TwilioClient.Init(accountSID, authToken);
            string randNum = "";
            try
            {
                Random random = new Random();
                randNum = random.Next(100000, 999999).ToString();
                // Send an SMS message.
                var message = MessageResource.Create(
                    to: new PhoneNumber(number),
                    from: new PhoneNumber("+12566854677"),
                    body: "Hi, Your OTP Number is " + randNum);
            }
            catch (TwilioException ex)
            {
                // An exception occurred making the REST call
                Console.WriteLine(ex.Message);
            }
            return randNum;
        }
        public async Task<string> CheckStatusAsync(string room)
        {
            var firestoreDb = CreateFirestoreDb();

            DocumentReference doc = firestoreDb.Collection("CounsellingChat").Document(room);
            DocumentSnapshot docSnapshot = await doc.GetSnapshotAsync();
            var status = docSnapshot.GetValue<string>("Status");
            Console.WriteLine(status);
            return status;
        }

        public async Task CreatePostAsync(Post newPost, string base64image)
        {
            var projectName = "fir-chat-ukiyo";
            //var authFilePath = "/Users/Ivan/Desktop/WeiJie Ang/Year 2/SEM 2/FSD/fir-chat-ukiyo-firebase-adminsdk.json";
            var authFilePath = "/Users/joeya/Downloads/NP_ICT/FSD & P2/fir-chat-ukiyo-firebase-adminsdk.json";
            //var authFilePath = "/Users/jaxch/Downloads/fir-chat-ukiyo-firebase-adminsdk.json";
            //var authFilePath = "/Users/gekteng/Downloads/fir-chat-ukiyo-firebase-adminsdk.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", authFilePath);
            FirestoreDb firestoreDb = FirestoreDb.Create(projectName);
            FirestoreDb db = FirestoreDb.Create(projectName);
            newPost.TimeCreated = DateTime.UtcNow;

            //Storing post into "All" and selectedCategory document in Firestore
            Dictionary<string, object> newPostDictionary = new Dictionary<string, object>
            {
                { "Description", newPost.Description },
                {"Likes", newPost.Likes},
                {"Tag", newPost.Tag },
                {"TimeCreated", newPost.TimeCreated },
                {"UserID", newPost.UserID },
                {"hasMedia", newPost.hasMedia }
            };
            DocumentReference docRef = await db.Collection("Posts").Document("Category").Collection("All").AddAsync(newPostDictionary);

            if (newPost.Tag != "All")
            {
                await db.Collection("Posts").Document("Category").Collection(newPost.Tag).Document(docRef.Id).CreateAsync(newPost);
            }
            
            if (newPost.hasMedia is true)
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = @"INSERT INTO PostMedia(DocumentKey, Image)
                                    VALUES(@key, @image)";

                cmd.Parameters.AddWithValue("@key", docRef.Id);
                cmd.Parameters.AddWithValue("@image", base64image);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public async Task<List<PostViewModel>> RetrievePostsAsync(string category)
        {
            var projectName = "fir-chat-ukiyo";
            //var authFilePath = "/Users/Ivan/Desktop/WeiJie Ang/Year 2/SEM 2/FSD/fir-chat-ukiyo-firebase-adminsdk.json";
            var authFilePath = "/Users/joeya/Downloads/NP_ICT/FSD & P2/fir-chat-ukiyo-firebase-adminsdk.json";
            //var authFilePath = "/Users/jaxch/Downloads/fir-chat-ukiyo-firebase-adminsdk.json";
            //var authFilePath = "/Users/gekteng/Downloads/fir-chat-ukiyo-firebase-adminsdk.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", authFilePath);
            FirestoreDb firestoreDb = FirestoreDb.Create(projectName);
            FirestoreDb db = FirestoreDb.Create(projectName);

            Query allPostsQuery = db.Collection("Posts").Document("Category").Collection(category);
            List<PostViewModel> postList = new List<PostViewModel>();
            QuerySnapshot allPostsSnapshot = await allPostsQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allPostsSnapshot.Documents)
            {
                Post post = documentSnapshot.ConvertTo<Post>();
                PostViewModel postVM = new PostViewModel();
                postVM.id  = documentSnapshot.Id;
                postVM.post = post;
                if (post.hasMedia is true)
                    postVM.Image = GetPostImageBase64(postVM.id);
                postList.Add(postVM);     //add each post to postList
            }
            List<PostViewModel> orderedPostList = postList.OrderByDescending(p => p.post.TimeCreated).ToList();
            return orderedPostList;
        }

        public async Task<List<PostViewModel>> RetrievePostsAsync(int id)
        {
            var projectName = "fir-chat-ukiyo";
            //var authFilePath = "/Users/Ivan/Desktop/WeiJie Ang/Year 2/SEM 2/FSD/fir-chat-ukiyo-firebase-adminsdk.json";
            var authFilePath = "/Users/joeya/Downloads/NP_ICT/FSD & P2/fir-chat-ukiyo-firebase-adminsdk.json";
            //var authFilePath = "/Users/jaxch/Downloads/fir-chat-ukiyo-firebase-adminsdk.json";
            //var authFilePath = "/Users/gekteng/Downloads/fir-chat-ukiyo-firebase-adminsdk.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", authFilePath);
            FirestoreDb firestoreDb = FirestoreDb.Create(projectName);
            FirestoreDb db = FirestoreDb.Create(projectName);
            CollectionReference postRef = db.Collection("Posts").Document("Category").Collection("All");
            List<PostViewModel> postList = new List<PostViewModel>();

            Query query = postRef.WhereEqualTo("UserID", id);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Post post = documentSnapshot.ConvertTo<Post>();
                PostViewModel postVM = new PostViewModel();
                postVM.id = documentSnapshot.Id;
                postVM.post = post;
                if (post.hasMedia is true)
                    postVM.Image = GetPostImageBase64(postVM.id);
                postList.Add(postVM);     //add each post to postList
            }
            List<PostViewModel> orderedPostList = postList.OrderByDescending(p => p.post.TimeCreated).ToList();
            return orderedPostList;
        }

        public string GetPostImageBase64(string id)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM [PostMedia] WHERE DocumentKey = @id";
            cmd.Parameters.AddWithValue("@id", @id);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string media = "";
            while (reader.Read())
            {
                media = reader.GetString(1);
            }
            reader.Close();
            conn.Close();
            return media;

        }

        public List<User> GetUsersProfilePicture()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT UserID, Image FROM [User]";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<User> userList = new List<User>();
            while (reader.Read())
            {
                userList.Add(
                new User
                {
                    UserID = reader.GetInt32(0),
                    Image = !reader.IsDBNull(1) ? reader.GetString(1) : null,
                });
            }
            reader.Close();
            conn.Close();
            return userList;
        }


    }
}
