using Base.Schema;
using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Schema
{
    public class EmployeeRequest : BaseRequest
    {
        [JsonIgnore]
        public int EmployeeNumber { get; set; }
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual List<InfoRequest> Infos { get; set; }
        public virtual List<DemandRequest> Demands { get; set; }


    }
    public class EmployeeResponse : BaseResponse
    {
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int EmployeeNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastActivityDate { get; set; }

        public virtual List<InfoResponse> Infos { get; set; }
        public virtual List<DemandResponse> Demands { get; set; }
    }
}
