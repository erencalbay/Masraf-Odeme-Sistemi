using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Schema
{
    // Bütün responselarda bulunacak olan Base Responselar.

    public abstract class BaseResponse
    {
        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
    }
}
