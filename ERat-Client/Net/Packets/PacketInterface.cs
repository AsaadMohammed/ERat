using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERat.Net.Packets {
    interface PacketInterface {
        byte[] Buffer { get; set; }
    }
}
