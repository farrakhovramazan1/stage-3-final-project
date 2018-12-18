using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    class PcStatus
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int destIp, int srcIP, byte[] macAddr, ref uint physicalAddrLen);
        public static Form1 form;       
        public String ip;
        public bool status;
        public string name;
        public String mac;
        public double time;
        public PcStatus(String lol)
        {
            status = false;
            ip = lol;
            mac = "Не удалось определить mac";
            time = 0;
        }
        public delegate void InvokeDelegate();
        public void pcCallback(Object sender, PingCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null && e.Reply.Status==IPStatus.Success)
            {
                time = e.Reply.RoundtripTime;
                status = true;
                IPAddress a = IPAddress.Parse(ip);
                try
                {
                    IPHostEntry entry = Dns.GetHostByAddress(a);
                    name = Dns.GetHostEntry(IPAddress.Parse(ip)).HostName;
                }
                catch { name = "Не удалось определить DNS"; };
                


                byte[] macAddr = new byte[6];
                uint macAddrLen = (uint)macAddr.Length;

                if (SendARP(BitConverter.ToInt32(a.GetAddressBytes(), 0), 0, macAddr, ref macAddrLen) == 0)
                {
                    string[] str = new string[(int)macAddrLen];
                    for (int i = 0; i < macAddrLen; i++)
                        str[i] = macAddr[i].ToString("x2");

                    mac = string.Join(":", str);
                }
            }
            form.Invoke(new InvokeDelegate(form.lowerCounterAndCheck));
        }
    }
}
