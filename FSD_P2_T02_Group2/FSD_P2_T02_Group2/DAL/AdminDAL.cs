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
using FSD_P2_T2_Group2.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace FSD_P2_T02_Group2.DAL
{
    public class AdminDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public User CheckAdminLogin(string username, string password) // Change to admin model
        {
            User user = new User(); // Change to admin model

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM AdminDetails
                                WHERE Username = @username AND PASSWORD = @password";

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user.Username = reader.GetString(1);
                    user.Password = reader.GetString(2);
                    user.Email = reader.GetString(3);
                    user.Name = reader.GetString(4);
                    user.Alias = reader.GetString(5);
                    user.PhoneNo = reader.GetString(6);  // Change according to admin 
                }
            }
            reader.Close();
            conn.Close();
            return user;
        }
    }
}
