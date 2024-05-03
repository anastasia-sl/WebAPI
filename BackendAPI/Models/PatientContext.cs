using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BackendAPI.Models
{
    public class PatientContext : DbContext
    {
        public PatientContext(string connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
        }

        public DbSet<Patient> Patients { get; set; }
    }
}