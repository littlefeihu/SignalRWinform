using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        public string ChoosenName { get; set; }
        private IHubProxy HubProxy { get; set; }
        string ServerURI = "http://localhost:8080/signalr";
        private HubConnection Connection { get; set; }
        public Form1()
        {
            var server = ConfigurationManager.AppSettings["server"];
            var port = ConfigurationManager.AppSettings["port"];

            ServerURI = $"{server}:{(port ?? "80")}";
            InitializeComponent();
            btn_send.Text = "Join Chat";

            ConnectAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ChoosenName))
            {
                string name = "";
                var chooseName = new NameChooser(ref name);
                chooseName.ShowDialog();
                this.ChoosenName = chooseName.ChoosenName;
                btn_send.Text = "Connecting...";
                Connect(this.ChoosenName);
                return;
            }
            HubProxy.Invoke("AddMessage", this.ChoosenName, textBox1.Text);
            textBox1.Text = String.Empty;
            textBox1.Focus();
        }
        private async void ConnectAsync()
        {
            Connection = new HubConnection(ServerURI);
            Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("MyHub");
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread
            HubProxy.On<string, string>("AddMessage", (name, message) =>
                this.Invoke((Action)(() =>
                    richTextBox1.AppendText(String.Format("{0}: {1}" + Environment.NewLine, name, message))
                ))
            );
            HubProxy.On("Started", () =>
                this.Invoke((Action)(() =>
                    richTextBox1.AppendText(String.Format("Exam Started" + Environment.NewLine))
                ))
            );

            HubProxy.On<string>("NewClient", (name) =>
                this.Invoke((Action)(() =>
                    richTextBox1.AppendText(String.Format("Client "+name+" Connected" + Environment.NewLine))
                ))
            );
            HubProxy.On("FailedToStart", () =>
                this.Invoke((Action)(() =>
                    richTextBox1.AppendText(String.Format("Exam Failed To Start!" + Environment.NewLine))
                ))
            );
            HubProxy.On("connected", () =>
                this.Invoke((Action)(() => {
                    richTextBox1.AppendText(String.Format("Connected as " + ChoosenName + "!" + Environment.NewLine));
                    btn_send.Enabled = true;
                    btn_send.Text = "Send";
                }
                ))
            );
            HubProxy.On("rejected", () =>
                this.Invoke((Action)(() =>
                    {
                        richTextBox1.AppendText(String.Format("Cannot connect with name of " + ChoosenName + "!" + Environment.NewLine));
                        this.ChoosenName = null;
                        btn_send.Enabled = true;
                        btn_send.Text = "Join Chat";
                    }
                ))
            );
            HubProxy.On<int>("TimeLeft", (timePassed) =>

                this.Invoke((Action)(() =>
                {
                    TimeSpan sinavSüresi = TimeSpan.FromMinutes(30);
                    var span = sinavSüresi.Subtract(TimeSpan.FromSeconds(timePassed));
                    lbl_time_left.Text = string.Format("{0}:{1}:{2}", span.Hours.ToString("0#"), span.Minutes.ToString("0#"), span.Seconds.ToString("0#"));
                })

            ));

            try
            {
                await Connection.Start();

            }
            catch (HttpRequestException)
            {
                btn_send.Enabled = false;
                btn_start.Enabled = false;
                richTextBox1.AppendText("Connected failed to " + ServerURI + Environment.NewLine);
                return;
            }

            richTextBox1.AppendText("Connected to server at " + ServerURI + Environment.NewLine);
        }
        public void Connect(string name)
        {
            btn_send.Enabled = false;
            HubProxy.Invoke("Connect", name);
        }
        private void CalculateTimeLeft(int obj)
        {

        }

        private void Connection_Closed()
        {
            richTextBox1.AppendText("Connection closed" + Environment.NewLine);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string key = null;
            var secretModal = new SecretModal(ref key);
            secretModal.ShowDialog();

            HubProxy.Invoke("Start", SecretModal.Key);

        }
    }
}
