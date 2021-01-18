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
                    ProfilePicture = !reader.IsDBNull(8) ? reader.GetString(8) : null,
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
                    user.ProfilePicture = !reader.IsDBNull(8) ? reader.GetString(8) : null;
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

            cmd.CommandText = @"SELECT * FROM [User]
                                WHERE Username = @username AND Password = @password";

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
                    user.ProfilePicture = !reader.IsDBNull(8) ? reader.GetString(8) : null;
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
            cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public int UpdateUser(User user)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"UPDATE [User] SET Password = @password, Name = @name, Alias = @alias, Status = @status, PhoneNo = @phoneno, ProfilePicture = @profpic WHERE UserID = @userid";
            cmd.Parameters.AddWithValue("@userid", user.UserID);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@alias", user.Alias);
            cmd.Parameters.AddWithValue("@status", user.Status);
            cmd.Parameters.AddWithValue("@phoneno", user.PhoneNo);
            if (String.IsNullOrEmpty(user.ProfilePicture))
            {
                cmd.Parameters.AddWithValue("@profpic", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@profpic", user.ProfilePicture);
            }

            try
            {
                conn.Open();
                int count = cmd.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine("Number of rows affected in update:" + count);
                conn.Close();
                return count;
            }
            catch
            {
                conn.Close();
                return 0;
            }
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

        public async Task sendMessage(User user, ChatMessage message, string room)
        {

            var firestoreDb = CreateFirestoreDb();;
            
            await firestoreDb.Collection(room).AddAsync(new ChatMessage
            {
                Alias = user.Alias,
                CreatedAt = Google.Cloud.Firestore.Timestamp.FromDateTime(DateTime.UtcNow),
                Message = message.Message
        });
        }

        private FirestoreDb CreateFirestoreDb()
        { 
            var projectName = "fir-chat-ukiyo";
            //var authFilePath = "/Users/joeya/Downloads/NP_ICT/FSD & P2/fir-chat-ukiyo-firebase-adminsdk.json";
            //var authFilePath = "/Users/jaxch/Downloads/fir-chat-ukiyo-firebase-adminsdk.json"; 
            var authFilePath = "/Users/gekteng/Downloads/fir-chat-ukiyo-firebase-adminsdk.json"; 
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", authFilePath);
            FirestoreDb firestoreDb = FirestoreDb.Create(projectName);
            Console.WriteLine("Created Firestore");
            return FirestoreDb.Create(projectName);
            
        }

        public string OTP(string number)
        {
            const string accountSID = "ACb2940c2a00ccdd56852ced467d8789b2";
            const string authToken = "d4fa2167bc11ccc0450d2e1249c06f13";

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
    }
}
