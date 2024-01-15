using Base.Schema;
using Data.Enum;
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
        public int DemandId { get; set; }
        public string Description { get; set; }
        public bool isDefault { get; set; }
        public int DemandNumber { get; set; }
        public DemandType DemandType { get; set; }
        public string RejectionResponse { get; set; }
    }
    public class DemandResponse : BaseResponse
    {
        public int Id { get; set; }
        public int DemandId { get; set; }
        public string Description { get; set; }
        public bool isDefault { get; set; }
        public int DemandNumber { get; set; }
        public DemandType DemandType { get; set; }
        public string RejectionResponse { get; set; }
    }
}
