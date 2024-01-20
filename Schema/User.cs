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
    public class UserRequest : BaseRequest
    {
        [JsonIgnore]
        public int UserNumber { get; set; }
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
    public class UserResponse : BaseResponse
    {
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int UserNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastActivityDate { get; set; }

        public virtual List<InfoResponse> Infos { get; set; }
        public virtual List<DemandResponse> Demands { get; set; }
    }
}
