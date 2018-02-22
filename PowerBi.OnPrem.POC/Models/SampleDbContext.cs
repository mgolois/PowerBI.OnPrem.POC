using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PowerBi.OnPrem.POC.Models
{
    public class SampleDbContext:DbContext
    {
        public SampleDbContext():base("SampleDBConnectionString")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<SurveyResult> SurveyResults { get; set; }
    }
}