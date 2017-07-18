using CommonLib.Adapter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class Form1 : Form
    {
        IList<Tuple<string, string>> device;
        SocketAdapter adapter = null;
        private string clientName { get; set; }
        public Form1()
        {
            InitializeComponent();
            clientName = prompt.ShowDialog("Please enter client Name", "Info");
            this.Text = clientName;
            device = SocketAdapter.PingAvailableServerSockets();
            adapter = new SocketAdapter(12345);
            adapter.Connect();
            adapter.OnResponseReceived += (response) =>
            {
                textBox1.Text += response + Environment.NewLine;
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (device != null && device.Count > 0)
            {
                try
                {
                    string command = textBox2.Text;
                    adapter.SendData(clientName+" -> "+command);
                    textBox2.Text = "";
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                }
            }
        }
    }
}
