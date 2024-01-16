using Base.Schema;
using WebAPI.Entity;

namespace Schema;

public class InfoRequest : BaseRequest
{
    public int UserId { get; set; }
    public string IBAN { get; set; }
    public string Information { get; set; }
    public int InfoNumber { get; set; }
    public string InfoType { get; set; }
    public bool isDefault { get; set; }

}
public class InfoResponse : BaseResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string IBAN { get; set; }
    public string Information { get; set; }
    public int InfoNumber { get; set; }
    public string InfoType { get; set; }
    public bool isDefault { get; set; }
}
