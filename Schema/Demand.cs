using Base.Schema;
using Data.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Schema
{
    public class DemandRequest : BaseRequest
    {
        [JsonIgnore]
        public int UserNumber { get; set; }
        public string Description { get; set; }
        public IFormFile Receipt { get; set; }
    }
    public class DemandResponse : BaseResponse
    {
        public int UserNumber { get; set; }
        public string Description { get; set; }
        public int DemandNumber { get; set; }
        public IFormFile Receipt { get; set; }
        public DemandResponseType DemandResponseType { get; set; }
        public string RejectionResponse { get; set; }
    }
    public class DemandRequestFromAdmin : BaseRequest
    {
        public int UserNumber { get; set; }
        public double? Amount { get; set; }
        public DemandResponseType DemandResponseType { get; set; }
        public string RejectionResponse { get; set; }
    }

    public class DemandResponseForAdmin : BaseResponse
    {
        public int DemandId { get; set; }
        public int UserNumber { get; set; }
        public string Description { get; set; }
        public int DemandNumber { get; set; }
        public IFormFile Receipt { get; set; }
        public DemandResponseType DemandResponseType { get; set; }
        public string RejectionResponse { get; set; }
    }
}
