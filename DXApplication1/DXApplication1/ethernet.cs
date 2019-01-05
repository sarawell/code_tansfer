using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXApplication1
{
    class Ethernet
    {
        byte[] SrcMac;
        byte[] DstMac;
        int Type;
        IPlay ip = null;
        public Ethernet(byte[] ethernet)
        {
            SrcMac = new byte[6];
            Array.Copy(ethernet, 6, SrcMac, 0, SrcMac.Length);
            DstMac = new byte[6];
            Array.Copy(ethernet, 0, DstMac, 0, SrcMac.Length);
            string temp = BitConverter.ToString(ethernet, 12,2).Replace("-", string.Empty).ToLower();
            Type = int.Parse(temp);
            byte[] iptemp = new byte[ethernet.Length - 14];
            Array.Copy(ethernet, 14, iptemp, 0, iptemp.Length);
            ip = new IPlay(iptemp);
        }
    }
}
