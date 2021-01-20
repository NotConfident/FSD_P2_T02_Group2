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
            cmd.CommandText = @"SELECT * FROM UserDetails";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<User> userList = new List<User>();
            while (reader.Read())
            {
                userList.Add(
                new User
                {
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    Email = reader.GetString(3),
                    Name = reader.GetString(4),
                    Alias = reader.GetString(5),
                    PhoneNo = reader.GetString(6)
                });
            }
            reader.Close();
            conn.Close();
            return userList;
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
                    user.Username = reader.GetString(1);
                    user.Password = reader.GetString(4);
                    user.Email = reader.GetString(5);
                    user.Name = reader.GetString(2);
                    user.Alias = reader.GetString(3);
                    user.PhoneNo = reader.GetString(7);
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
            var authFilePath = "/Users/joeya/Downloads/NP_ICT/FSD & P2/fir-chat-ukiyo-firebase-adminsdk.json";
            //var authFilePath = "/Users/jaxch/Downloads/fir-chat-ukiyo-firebase-adminsdk.json" 
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", authFilePath);
            FirestoreDb firestoreDb = FirestoreDb.Create(projectName);
            Console.WriteLine("Created Firestore");
            return FirestoreDb.Create(projectName);
            
        }

        public string OTP(string number)
        {
            const string accountSID = "ACb2940c2a00ccdd56852ced467d8789b2";
            const string authToken = "26b0ccfbc027d0dafb402ded896726fd";

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
