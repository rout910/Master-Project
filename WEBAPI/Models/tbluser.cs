using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBAPI.Models
{
    public class tbluser
    {
        public int c_id {get; set;}
        public string  c_emailid { get; set; }
        public string c_username {get; set;}
        public string c_password{get; set;}
        public string c_userrole{get; set;}
    }
}