using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Data;

namespace DXApplication1
{
    class PcapAnalysis
    {
        public string filePath = null;
        Form1 form1;
        public PcapAnalysis(Form1 form)
        {
            form1 = form;
        }

        public void initAnalysis()
        {
            filePath = form1.textEdit_dir.Text;
            FileStream fs = File.OpenRead(filePath);
            byte[] pcapheader = new byte[24];
            fs.Read(pcapheader, 0, pcapheader.Length);
            pcapHeader pcaph = new pcapHeader();
            pcaph.getValues(pcapheader);
            byte[] packetheader = new byte[16];
            fs.Read(packetheader, 0, packetheader.Length);
            PacketHeader ph = new PacketHeader();
            ph.GetValue(packetheader);
            byte[] payload = new byte[ph.packerlen];
            fs.Read(payload, 0, payload.Length);
            Ethernet ethernet = new Ethernet(payload);



            //StreamReader sr = new StreamReader(filePath, System.Text.Encoding.Default);
        }
    }

    struct pcapHeader
    {
        public string magic;
        public int major;
        public int minor;
        public int LinkType;
        public bool IsReverse;
        public int getValues(byte[] frame)
        {

            byte[] magic_temp = new byte[4];
            Array.Copy(frame, 0, magic_temp, 0, magic_temp.Length);
            //magic = System.Text.Encoding.ASCII.GetString(magic_temp);
            magic=BitConverter.ToString(magic_temp).Replace("-", string.Empty).ToLower();

            byte[] major_temp = new byte[2];
            Array.Copy(frame, 4, major_temp, 0, major_temp.Length);
            byte[] minor_temp = new byte[2];
            Array.Copy(frame, 6, minor_temp, 0, minor_temp.Length);
            byte[] linktype_temp = new byte[4];
            Array.Copy(frame, 20, linktype_temp, 0, linktype_temp.Length);
            if (magic == "a1b2c3d4")
            {
                IsReverse = false;
            }
            else
                IsReverse = true;

            if(IsReverse)
            {
                Array.Reverse(major_temp);
                Array.Reverse(minor_temp);
                Array.Reverse(linktype_temp);
            }
            string temp = BitConverter.ToString(major_temp).Replace("-", string.Empty).ToLower();
            major = int.Parse(temp);
            temp= BitConverter.ToString(minor_temp).Replace("-", string.Empty).ToLower();
            minor = int.Parse(temp);
            temp = BitConverter.ToString(linktype_temp).Replace("-", string.Empty).ToLower();
            LinkType = int.Parse(temp);

            return 0;
        }
    }


    public enum LinkTypeSTD:int
    {
        BSD=0,
        Ethernet=1,
        TokenPing=6,
        ARCnet=7,
        SLIP=8,
        PPP=9,
        FDDI=10,
        LLCSNAP=100,
        rawIP=101,
        BSD_OS_SLIP=102,
        BSD_OS_PPP=103,
        Cisco_HDLC=104,
        C802_11=105,
        OpenBSDloopback=108,
        special_Linuxcooked=113,
        LocalTalk=114
    }

    public struct PacketHeader
    {
        public DateTime dateTime;
        public int packerlen;

        public void GetValue(byte[] frame)
        {
            byte[] time_temp = new byte[4];
            Array.Copy(frame, 0, time_temp, 0, time_temp.Length);
            //string UnixTime = BitConverter.ToString(time_temp).Replace("-", string.Empty).ToLower();
            int u = BitConverter.ToInt32(time_temp,0);
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            dateTime = startTime.AddSeconds(double.Parse(u.ToString()));
            byte[] len = new byte[4];
            Array.Copy(frame, 8, len, 0, len.Length);
            len.Reverse();
            packerlen = BitConverter.ToInt32(len, 0);
        }
    }

}
