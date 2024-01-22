using Base.Schema;
using WebAPI.Entity;

namespace Schema;

// Info DTO'ları 
public class InfoRequest : BaseRequest
{
    public int UserNumber { get; set; }
    public string IBAN { get; set; }
    public string Information { get; set; }
    public string InfoType { get; set; }
    public bool isDefault { get; set; }

}

public class InfoUpdateRequest : BaseRequest
{
    public int InfoNumber { get; set; }
    public string IBAN { get; set; }
    public string Information { get; set; }
    public string InfoType { get; set; }
    public bool isDefault { get; set; }
}
public class InfoResponse : BaseResponse
{
    public int InfoId { get; set; }
    public int UserNumber { get; set; }
    public string IBAN { get; set; }
    public string Information { get; set; }
    public int InfoNumber { get; set; }
    public string InfoType { get; set; }
    public bool isDefault { get; set; }
}
