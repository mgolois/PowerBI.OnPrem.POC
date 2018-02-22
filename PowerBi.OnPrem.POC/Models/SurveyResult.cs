using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PowerBi.OnPrem.POC.Models
{
    [Table("SurveyResults")]
    public class SurveyResult
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Race { get; set; }
        public string Language { get; set; }
        public string Gender { get; set; }
        public string Company { get; set; }
        public decimal Salary { get; set; }
        public string JobTitle { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}