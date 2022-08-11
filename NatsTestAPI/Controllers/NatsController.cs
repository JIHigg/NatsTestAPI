using Microsoft.AspNetCore.Mvc;
using NatsTestAPI.Services;
using NatsTestCore.Objects;
using System.Collections.Generic;

namespace NatsTestAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class NatsController : ControllerBase
    {
        private ChatService _chatService;

        public NatsController(ChatService chatService)
        {
            _chatService = chatService;
        }

        /// <summary>
        /// Controller implementation returning IEnumerable of Messages
        /// </summary>
        /// <returns></returns>
        [HttpGet("messages")]
        public IEnumerable<Message> GetMessages()
        {
            return _chatService.GetMessages();
        }

        /// <summary>
        /// Post message to NATSServer
        /// </summary>
        /// <param name="message"></param>
        [HttpPost("messages")]
        public void PostMessage([FromBody]Message message)
        {
            _chatService.SendMessage(message);
        }

        //OLD CODE 
        /*private static IConnection _connect = EnsureConnection();
        string defaultSubscription = "nats.demo.pubsub";
        
        /// <summary>
        /// Post request to Publish message to the Subscription channel
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] string message)
        {
            //using(_connect = EnsureConnection())
            //{
            //}
                try
                {
                    _connect.Publish(defaultSubscription, Encoding.UTF8.GetBytes(message));
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
        }

        /// <summary>
        /// Get Request to retrieve messages from the Subscription channel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMessages()
        {
            try 
            {
                List<string> newMessages = new List<string>();
                EventHandler<MsgHandlerEventArgs> msg = (sender, args) =>
                {
                    string data = Encoding.UTF8.GetString(args.Message.Data);
                    newMessages.Add(data);
                };

                var data = Json(newMessages);
                return (Ok(data));

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates new IConnection instance with NATS server
        /// </summary>
        /// <returns></returns>
        private static IConnection EnsureConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();

            Options ops = ConnectionFactory.GetDefaultOptions();
            ops.Url = "nats://localhost:4222"; //TODO change url to correct server

            return factory.CreateConnection(ops); //Sometimes throws 'Timeout' error?
        }*/
    }
}
