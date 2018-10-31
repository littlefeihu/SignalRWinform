using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;

namespace WebSocketWinform
{
    public partial class Form1 : Form
    {
        //static string url = "ws://172.17.1.211:9000/lis/api/intelligentservice/websocket/notice/0008849d-3171-e9b8-791f-735928742dc6?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImdoMDEyIiwicHdkIjoiOTZlNzkyMTg5NjVlYjcyYzkyYTU0OWRkNWEzMzAxMTIiLCJvcmdpZCI6IjQyNTA1MTc1MTAxIiwiaWF0IjoxNTQwOTQ2ODE2MjYwfQ.6qFVUuTnsXyGb4dJIkKphY0vBbxqUqUFUY6Ln39zufM&clientinfo=%7b%22COMPUTERNAME%22%3a%22DESKTOP-TEST%22%2c%22IPADDRESS%22%3a%22172.17.17.195%22%2c%22MACADDRESS%22%3a%228C%3a16%3a45%3a29%3aD8%3a6E%22%2c%22CPU%22%3a%22BFEBFBFF000906E9%22%7d";

        static string url = "ws://localhost:8000/Laputa";

        static WebSocket ws = new WebSocket(url);
        public Form1()
        {
            InitializeComponent();

            ws.OnMessage += (sender, e) =>
            {

                this.Invoke((Action)(() =>
                {
                    Console.WriteLine("Receive Message: " + e.Data);
                    toolStripStatusLabel1.Text = "Receive Message: " + e.Data;
                }
   ));
            };
            ws.OnOpen += (sender, e) =>
            {
                this.Invoke((Action)(() =>
                {
                    Console.WriteLine("Connect Opened");
                    toolStripStatusLabel1.Text = "Connect Opened";
                }
               ));



            };
            ws.OnError += (sender, e) =>
            {
                this.Invoke((Action)(() =>
                {
                    Console.WriteLine("Error Happened: " + e.Message);
                    toolStripStatusLabel1.Text = "Error Happened: " + e.Message;
                }
));

            };
            ws.OnClose += (sender, e) =>
            {
                this.Invoke((Action)(() =>
                {

                    Console.WriteLine("Connect Closed: " + e.Reason);
                    toolStripStatusLabel1.Text = "Connect Closed: " + e.Reason;
                }
                    ));
            };


        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ws.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ws.Send(textBox1.Text);
        }
    }
}
