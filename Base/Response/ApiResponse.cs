using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Base.Response
{
    // Genel bir repsonse, bütün responselarda dönecek değerleri içeriyor. generic ve sabit olmak üzere ikiye ayırdık

    // Sabit ApiResponse
    public class ApiResponse
    {
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public ApiResponse(string message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Success = true;
            }
            else
            {
                Success = false;
                Message = message;
            }
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime ServerDate { get; set; } = DateTime.UtcNow;
        public Guid ReferenceNo { get; set; } = Guid.NewGuid();
    }


    // Generic ApiResponse
    public class ApiResponse<T>
    {
        public DateTime ServerDate { get; set; } = DateTime.UtcNow;
        public Guid ReferenceNo { get; set; } = Guid.NewGuid();
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Response { get; set; }

        public ApiResponse(bool isSuccess)
        {
            Success = isSuccess;
            Response = default;
            Message = isSuccess ? "Success" : "Error";
        }
        public ApiResponse(T data)
        {
            Success = true;
            Response = data;
            Message = "Success";
        }
        public ApiResponse(string message)
        {
            Success = false;
            Response = default;
            Message = message;
        }
    }
}
