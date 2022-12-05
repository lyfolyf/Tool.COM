using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lead.Tool.COM
{
    public enum ReadModeEnum
    {
        Readline = 0,
        ReadExsting 
    }


    public class COMConfig
    {
        public string Name { get; set; }
        public string COM { get; set; }
        public int BaudRate { get; set; }
        public Parity CheckBit { get; set; }
        public int DataBit { get; set; }
        public StopBits StopBit { get; set; }

        public ReadModeEnum ReadMode { get; set; }

        public UInt32 SendSize { get; set; }

        public UInt32 ReciveSize { get; set; }

    }
}
