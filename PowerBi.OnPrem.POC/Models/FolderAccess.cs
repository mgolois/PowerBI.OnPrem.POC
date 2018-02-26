using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PowerBi.OnPrem.POC.Models
{
    [Table("FolderAccess")]
    public class FolderAccess
    {
        [Key]
        public int Id { get; set; }
        public Guid FolderId { get; set; }
        public string FolderName { get; set; }
        public int UserId { get; set; }
        public bool CanEdit { get; set; }
        public User User { get; set; }
    }
}