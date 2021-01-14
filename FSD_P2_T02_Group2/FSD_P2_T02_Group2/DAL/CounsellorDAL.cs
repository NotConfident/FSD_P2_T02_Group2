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
    }
}
