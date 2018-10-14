using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Configuration;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var runAt = ConfigurationManager.AppSettings["runAt"];
            var port = ConfigurationManager.AppSettings["port"];
            var url = $"{runAt}:{(port??"80")}";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
 
    public class MyHub : Hub
    {
        static ConcurrentDictionary<string, string> connectedClients = new ConcurrentDictionary<string, string>();
        Timer timer;
        const string Secret = "ThisIsASampleSecretKeyToFilterRequest";
        DateTime startedOn;
        
        public void Start(string key)
        {
            if (key==Secret)
            {
                startedOn = DateTime.Now;
                timer = new Timer(1000);
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
                Clients.All.started();
            }
            else
            {
                Clients.Caller.failedToStart();
            }
        }

        public void Connect(string name)
        {
            if (connectedClients.Any(c=>c.Value.ToLower()==name.ToLower()))
            {
                string reason = $"the {name} is already taken, please choose another name";
                Clients.Caller.rejected(reason);
            }
            else
            {
                var connectionId = this.Context.ConnectionId;
                connectedClients.AddOrUpdate(connectionId,name,(key,value)=> { return value; });
                Clients.Caller.connected();
                Clients.Others.newClient(name);
                Console.WriteLine("new client connected: "+name);
                Console.WriteLine("Total Clients: "+connectedClients.Count());
            }
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.TimeLeft((int)e.SignalTime.Subtract(startedOn).TotalSeconds);
        }

        public void TimeLeft(int spanPassed)
        {
            Clients.All.timeLeft(spanPassed);
        }
        public void AddMessage(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }
    }
}
