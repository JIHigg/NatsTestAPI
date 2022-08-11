using NATS.Client;
using System;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

namespace NatsTestConsole
{
    static class MessagerClient
    {
        private static string url = "https://localhost:44310/api/Nats";
        private static IConnection _connection; //For incoming messages -should be moved to api
        private static bool exit = false;

        public static void Run()
        {


            while (!exit)
            {
                using(_connection = CreateConnection())
                {
                    Console.Clear();
                    Console.WriteLine("You may send a message at any time, or Press '1' to retrieve your messages:");

                    //IncomingMessages();

                    string input = Console.ReadLine();

                    if(input != null)
                    {
                        if (input == "1")
                            GetMessages();
                        else
                        {
                            var task = Send(input);
                        }
                    }

                    //EventHandler<MsgHandlerEventArgs> h = (sender, args) =>
                    //{
                    //    Console.WriteLine(args.Message);
                    //};
                }
            }
        }

        /// <summary>
        /// Monitors and Displays incoming messages on subscription
        /// </summary>
        private static void IncomingMessages()
        {
            Task.Run(() =>
            {
                var sub = _connection.SubscribeSync("nats.demo.pubsub");
                while (!exit)
                {
                    var message = sub.NextMessage();
                    if(message != null)
                    {
                        string msg = Encoding.UTF8.GetString(message.Data);
                        Console.WriteLine(msg);

                    }
                }
            });
        }

        /// <summary>
        /// Creates IConnection to NATS for receiving messages
        /// </summary>
        /// <returns></returns>
        private static IConnection CreateConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();

            Options options = ConnectionFactory.GetDefaultOptions();
            options.Url = "nats://localhost:4222";

            return factory.CreateConnection(options);
        }

        /// <summary>
        /// Sends Get Request to API for Messages since started
        /// </summary>
        private async static void GetMessages()
        {
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(url);

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Send request to API REST service GetMessages using HttpClient
                var response = await client.GetAsync(url);

                //Check to ensure success
                if (response.IsSuccessStatusCode)
                {
                    //Storing response results
                    var messageResponse = response.Content.ReadAsStringAsync().Result;

                    //Deserializing results
                    List<OldMessage> messages = JsonSerializer.Deserialize<List<OldMessage>>(messageResponse);

                    foreach(var m in messages)
                    {
                        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} { m.Reply} + {m.Data}");
                    }

                }
            }
        }

        /// <summary>
        /// Sends POST request to API with string message
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async static Task Send(string input)
        {
            using (var client = new HttpClient())
            {
                //Serializing message text to JSON
                var json = JsonSerializer.Serialize(input);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                //Building request with content headers
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = data;

                //Building HttpClient
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                

                var result = await client.PostAsync(url,request.Content);

                if (result.IsSuccessStatusCode)
                    Console.WriteLine("Your message has been sent");
                else Console.WriteLine($"Something went wrong: {result.ReasonPhrase}");

            }
        }
    }
}
