using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SocketIOClient.Sample
{
    class Program
    {
        public static SocketIO client = null;


        public static void Main(string[] args) {
            Task.Run(async () => {
                await Run();
            });
            Console.ReadKey(true);
        }
        public static async Task Run()
        {
            var url = "http://robot6.funnyai.com:8000";
            client = new SocketIO(url)
            {
                // if server need some parameters, you can add to here
                Parameters = new Dictionary<string, string>
                {
                    { "uid", "" },
                    { "token", "" }
                }
            };

            //client.OnClosed += Client_OnClosed;
            client.OnConnected +=Client_OnConnectedAsync;

            // Listen server events
            client.On("chat_event", res =>
            {
                Console.WriteLine(res.Text);
                // Next, you might parse the data in this way.
                //var obj = JsonConvert.DeserializeObject<T>(res.Text);
                //// Or, read some fields
                //var jobj = JObject.Parse(res.Text);
                //int code = jobj.Value<int>("code");
                //bool hasMore = jobj["data"].Value<bool>("hasMore");
                //var data = jobj["data"].ToObject<ResponseData>();
                // ...
            });

            //// Connect to the server
            await client.ConnectAsync();
            //await client.ConnectAsync();

            //// Emit test event, send string.
            //await client.EmitAsync("test", "EmitTest");

            //// Emit test event, send object.
            //await client.EmitAsync("test", new { code = 200 });

            // ...

        }

        private static void Client_OnConnectedAsync() {
            Console.WriteLine("Connected to server");
            Task.Run(async () => {
            await client.EmitAsync("chat_event", 
                new { from="a",to="", message = "test .net" });// "{\"from\":\"a\",\"to\":\"*\",\"msg\"}");

            });
        }


        private static  async void Client_OnClosed() {
            //if (reason == ServerCloseReason.ClosedByServer) {
            //    // ...
            //} else if (reason == ServerCloseReason.Aborted) {
            //    for (int i = 0; i < 3; i++) {
            //        try {
            //            await client.ConnectAsync();
            //            break;
            //        } catch (WebSocketException ex) {
            //            // show tips
            //            Console.WriteLine(ex.Message);
            //            await Task.Delay(2000);
            //        }
            //    }
            //    // show tips
            //    Console.WriteLine("Tried to reconnect 3 times, unable to connect to the server");
            //}
        }


        static async Task Test()
        {
            var client = new SocketIO("http://robot6.funnyai.com:8000");
            client.OnClosed += async reason =>
            {
                //await Task.Delay(60000);
                //await client.ConnectAsync();
                //await client.EmitAsync("test", "test");
                if (reason == ServerCloseReason.ClosedByServer)
                {
                    // ...
                }
                else if (reason == ServerCloseReason.Aborted)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            await client.ConnectAsync();
                            break;
                        }
                        catch (WebSocketException ex)
                        {
                            // show tips
                            Console.WriteLine(ex.Message);
                            await Task.Delay(2000);
                        }
                    }
                    // show tips
                    Console.WriteLine("Tried to reconnect 3 times, unable to connect to the server");
                }
            };
            await client.ConnectAsync();
            await Task.Delay(10000);
            await client.EmitAsync("close", "close");
        }

        static void Test1(CancellationTokenSource tokenSource)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    if (tokenSource.IsCancellationRequested)
                    {
                        break;
                    }
                    await Task.Delay(1000);
                    Console.WriteLine(DateTime.Now);
                }
            }, tokenSource.Token);
        }
    }
}
