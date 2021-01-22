using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using FSD_P2_T02_Group2.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace FSD_P2_T02_Group2.DAL
{
    public class CounsellorDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public CounsellorDAL()
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

        public Counsellor CheckLogin(string email, string password)
        {
            Counsellor counsellor = new Counsellor();

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM Counsellor
                                WHERE Email = @email AND Password = @password";

            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    counsellor.counsellorID = reader.GetInt32(0);
                    counsellor.Name = reader.GetString(1);
                    counsellor.Password = reader.GetString(2);
                    counsellor.Email = reader.GetString(3);
                    counsellor.DateCreated = reader.GetDateTime(4);
                    counsellor.PhoneNumber = reader.GetString(5);
                    counsellor.Image = !reader.IsDBNull(6) ? reader.GetString(6) : null;
                    counsellor.Certificate = !reader.IsDBNull(7) ? reader.GetString(7) : null;
                    counsellor.DateBirth = reader.GetDateTime(8);
                    //counsellor.AvgRating = !reader.IsDBNull(9) ? reader.GetFloat(9) : null;
                    counsellor.Status = reader.GetString(10);

                }
            }
            reader.Close();
            conn.Close();
            return counsellor;
        }

        public void CounsellorForm(PendingCounsellor counsellorForm)
        {
            SqlCommand cmd = conn.CreateCommand();
            //string status = "Pending";
            //DateTime currentDT = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt"));
            //string defaultPassword = "counsellor";
            DateTime temp = Convert.ToDateTime(DateTime.Now.ToString("2020-01-01"));

            cmd.CommandText = @"INSERT INTO PendingCounsellor(Name, Email, PhoneNo, DateBirth, Image, Certificate)
                                VALUES(@name, @email, @phoneno, @datebirth, @image, @certificate)";

            cmd.Parameters.AddWithValue("@name", counsellorForm.Name);
            //cmd.Parameters.AddWithValue("@password", defaultPassword);
            cmd.Parameters.AddWithValue("@email", counsellorForm.Email);
            //cmd.Parameters.AddWithValue("@datecreated", currentDT);
            cmd.Parameters.AddWithValue("@phoneno", counsellorForm.PhoneNumber);
            cmd.Parameters.AddWithValue("@datebirth", temp);
            cmd.Parameters.AddWithValue("@image", counsellorForm.Image);
            cmd.Parameters.AddWithValue("@certificate", counsellorForm.Certificate);
            //cmd.Parameters.AddWithValue("@avgrating", counsellorForm.AvgRating);
            //cmd.Parameters.AddWithValue("@status", status);

            conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public List<PendingCounsellorSession> retrieveUserForms() // Change to admin model
        {
            PendingCounsellorSession pcSession = new PendingCounsellorSession(); // Change to admin model
            List<PendingCounsellorSession> pcSessionList = new List<PendingCounsellorSession>();

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM PendingSessionView";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                pcSessionList.Add(
                new PendingCounsellorSession
                {
                    SessionID = reader.GetInt32(0),
                    Feeling = reader.GetInt32(1),
                    Thought = reader.GetInt32(2),
                    Problems = reader.GetString(3),
                    DateCreated = reader.GetDateTime(4),
                    UserID = reader.GetInt32(5),
                    //Alias = reader.GetString(6),
                });
            }
            reader.Close();
            conn.Close();
            return pcSessionList;
        }

    }
}
