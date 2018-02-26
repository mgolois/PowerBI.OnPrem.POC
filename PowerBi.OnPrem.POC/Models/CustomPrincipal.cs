using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace PowerBi.OnPrem.POC.Models
{
    public class CustomPrincipal : GenericPrincipal
    {
        public int UserId { get; set; }
        public bool IsAdmin => IsInRole("Admin");
        public CustomPrincipal(IIdentity identity, string[] roles, int userId) : base(identity, roles)
        {
            UserId = userId;
        }
    }
    public class UserData
    {
        public int UserId { get; set; }
        public string[] Roles { get; set; }
    }
}