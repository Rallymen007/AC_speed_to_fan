using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO.MemoryMappedFiles;

namespace SerialIO
{
    public partial class Form1 : Form
    {
        private SerialPort port;
        private MemoryMappedFile shm;
        private MemoryMappedViewAccessor accessor;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            port = new SerialPort("COM5", 9600);
            port.Open();
            port.Write("0");
            /*shm = MemoryMappedFile.OpenExisting("acpmf_physics");
            accessor = shm.CreateViewAccessor(0, 116);

            new Thread(new ThreadStart(refreshValue)).Start();*/
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            int w = trackBar1.Value;
            //byte[] b = BitConverter.GetBytes(w);
            //Array.Reverse(b);
            port.Write(w.ToString());
            label1.Text = trackBar1.Value.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            port.Close();
        }

        private void refreshValue()
        {
            try
            {
                while (true)
                {
                    float w = accessor.ReadSingle(28);
                    label2.Invoke((MethodInvoker)(() => label2.Text = w.ToString()));
                    w = (w > 255) ? 255 : w;
                    port.Write(BitConverter.GetBytes((Int32)Math.Round(w)), 0, 1);
                    Thread.Sleep(16);
                }
            }
            catch { }
        }
    }
}
