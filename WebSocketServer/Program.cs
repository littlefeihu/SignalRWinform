using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Example
{
    public class Laputa : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            var msg = "server arrived";

            Send(msg);
        }

        protected override void OnOpen()
        {
            Console.WriteLine("Client Connected");
            base.OnOpen();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("Client Closed");
            base.OnClose(e);
        }
        protected override void OnError(ErrorEventArgs e)
        {
            Console.WriteLine("Client Errored");
            base.OnError(e);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var wssv = new WebSocketServer("ws://localhost:8000");
            try
            {
                wssv.AddWebSocketService<Laputa>("/Laputa");

                wssv.Start();
                Console.WriteLine("Server started");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Happened:" + ex.ToString());
            }
            finally
            {
                wssv.Stop();

            }
        }
    }
}