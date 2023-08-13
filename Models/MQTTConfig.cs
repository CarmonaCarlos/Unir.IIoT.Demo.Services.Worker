using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unir.IIoT.Demo.Services.Worker.Models
{
    public class MQTTConfig
    {
        public string BrokerAddress { get; set; } = string.Empty;
        public int BrokerPort { get; set; }
        public string Topic { get; set; } = string.Empty;
    }
}
