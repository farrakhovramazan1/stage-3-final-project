using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Sockets;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
namespace WindowsFormsApp2
{
    
    public partial class Form1 : Form
    {
        Int64 counter;
        Object obj;
        List<PcStatus> list;
        public Form1()
        {
            obj = new Object();
            PcStatus.form = this;
            list = new List<PcStatus>();
            counter = 0;
            InitializeComponent();
        }
        public void lowerCounterAndCheck()
        {
            lock (obj)
            {
                counter--;
                progressBar1.Value++;
            }
            if(counter==0)
            {
                String res = "";
                foreach(PcStatus st in list)
                {
                    if(st.status)
                        res += st.ip + " " + st.mac + " " + st.name + " " + (st.status ? "Успешно " + st.time : "Нет соединения")+"\n";
                }
                label1.Text = res;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (counter != 0)
                return;
            int s1 = int.Parse(textBox1.Text), s2 = int.Parse(textBox2.Text), s3 = int.Parse(textBox3.Text), s4 = int.Parse(textBox4.Text);
            int d1 = int.Parse(textBox8.Text), d2 = int.Parse(textBox7.Text), d3 = int.Parse(textBox6.Text), d4 = int.Parse(textBox5.Text);
            counter = -(s1 - d1) * 256 * 256 * 256 - (s2 - d2) * 256 * 256 - (s3 - d3) * 256 - (s4 - d4);
            progressBar1.Maximum = (int)counter;
            progressBar1.Value = 0;
            Thread t = new Thread(threadFunc);
            t.Start();
            //GetInform("192.168.0.105");
        }
        private void threadFunc()
        {
            int s1 = int.Parse(textBox1.Text), s2 = int.Parse(textBox2.Text), s3 = int.Parse(textBox3.Text), s4 = int.Parse(textBox4.Text);
            int d1 = int.Parse(textBox8.Text), d2 = int.Parse(textBox7.Text), d3 = int.Parse(textBox6.Text), d4 = int.Parse(textBox5.Text);
            list = new List<PcStatus>();
            while (s1 != d1 || s2 != d2 || s3 != d3 || s4 != d4)
            {
                PcStatus pcStatus = new PcStatus(s1 + "." + s2 + "." + s3 + "." + s4);
                GetInform(pcStatus);
                list.Add(pcStatus);
                s4++;
                if (s4 >= 256)
                {
                    s4 = 0;
                    s3 += 1;
                }
                if (s3 >= 256)
                {
                    s3 = 0;
                    s2 += 1;
                }
                if (s2 >= 256)
                {
                    s2 = 0;
                    s1 += 1;
                }
            }
        }
        IPHostEntry entry;
        private void GetInform(PcStatus pcStatus)
        {
            try
            {
                //Проверяем существует ли IP
                Ping ping = new Ping();
                ping.PingCompleted += new PingCompletedEventHandler(pcStatus.pcCallback);
                ping.SendAsync(pcStatus.ip, 5000);
            }
            catch(Exception e) { label1.Text = e.ToString(); };

        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

      
    }
}
