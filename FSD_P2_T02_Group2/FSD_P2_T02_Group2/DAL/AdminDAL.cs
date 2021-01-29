using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using FSD_P2_T02_Group2.Models;
using FSD_P2_T02_Group2.DAL;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace FSD_P2_T02_Group2.DAL
{
    public class AdminDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public AdminDAL()
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

        public User CheckAdminLogin(string username, string password) // Change to admin model
        {
            User user = new User(); // Change to admin model

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"EXEC uspAdminLogin @username, @password";

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user.Username = reader.GetString(1);
                    user.Password = reader.GetString(3);
                    //user.Email = reader.GetString(3);
                    //user.Name = reader.GetString(4);
                    user.Alias = reader.GetString(2);
                    user.PhoneNo = reader.GetString(4);  // Change according to admin 
                }
            }
            reader.Close();
            conn.Close();
            return user;
        }


        public List<PendingCounsellor> retrievePendingCounsellor() // Change to admin model
        {
            PendingCounsellor pCounsellor = new PendingCounsellor(); // Change to admin model
            List<PendingCounsellor> pCounsellorList = new List<PendingCounsellor>();

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM PendingCounsellor";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                pCounsellorList.Add(
                new PendingCounsellor
                {
                    PCounsellorID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    PhoneNumber = reader.GetString(3),
                    DateBirth = reader.GetDateTime(4),
                    Image = reader.GetString(5),
                    Certificate = reader.GetString(6)

                });
            }
            reader.Close();
            conn.Close();
            return pCounsellorList;
        }

        public PendingCounsellor retrieveSpecificPendingCounsellor(int id) // Change to admin model
        {
            PendingCounsellor pCounsellor = new PendingCounsellor(); // Change to admin model

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM PendingCounsellor
                                WHERE PCounsellorID = @id";

            conn.Open();
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    pCounsellor.PCounsellorID = reader.GetInt32(0);
                    pCounsellor.Name = reader.GetString(1);
                    pCounsellor.Email = reader.GetString(2);
                    pCounsellor.PhoneNumber = reader.GetString(3);
                    pCounsellor.DateBirth = reader.GetDateTime(4);
                    pCounsellor.Image = reader.GetString(5);
                    pCounsellor.Certificate = reader.GetString(6);
                }
            }
            reader.Close();
            conn.Close();
            return pCounsellor;
        }

        public bool ApproveCounsellor(int id)
        {
            SqlCommand cmd = conn.CreateCommand();
            string password = "counsellor";
            DateTime datetime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt"));
            DateTime tempDate = Convert.ToDateTime(DateTime.Now.ToString("2020-01-01"));
            string status = "Active";

            cmd.CommandText = @"INSERT INTO Counsellor(Name, Password, Email, DateCreated, PhoneNo, Image, Certificate, DateBirth, AvgRating, Status)
                                VALUES(@name, @password, @email, @datecreated, @phoneno, @image, @certificate, @datebirth, @avgrating, @status)";

            PendingCounsellor retrieved = retrieveSpecificPendingCounsellor(id);

            cmd.Parameters.AddWithValue("@name", retrieved.Name);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@email", retrieved.Email);
            cmd.Parameters.AddWithValue("@datecreated", datetime);
            cmd.Parameters.AddWithValue("@phoneNo", retrieved.PhoneNumber);
            cmd.Parameters.AddWithValue("@image", retrieved.Image);
            cmd.Parameters.AddWithValue("@certificate", retrieved.Certificate);
            cmd.Parameters.AddWithValue("@datebirth", tempDate.Date);
            cmd.Parameters.AddWithValue("@avgrating", DBNull.Value);
            cmd.Parameters.AddWithValue("@status", status);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();


            cmd.CommandText = @"DELETE FROM PendingCounsellor
                                WHERE PCounsellorID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            return true;
        }

        public bool RejectCounsellor(int id)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"DELETE FROM PendingCounsellor
                                WHERE PCounsellorID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            return true;
        }
    }
}
