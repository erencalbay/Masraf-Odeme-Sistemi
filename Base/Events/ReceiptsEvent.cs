using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Events
{
    // RabbitMQ ile FileAPI'den gelecek olan bilgilerin event edilmesi
    public class ReceiptsEvent
    {
        public string Path { get; set; }
        public string Description { get; set; }
        public int UserNumber { get; set; }
    }
}
