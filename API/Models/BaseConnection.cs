using System;
using Microsoft.Data.SqlClient;

namespace API.Models
{
    public class BaseConnection
    {
        protected SqlConnection Con { get; private set; }
        protected SqlCommand Cmd { get; private set; }

        public BaseConnection()
        {
            Con = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            Cmd = new SqlCommand
            {
                Connection = Con
            };
        }       
    }
}
