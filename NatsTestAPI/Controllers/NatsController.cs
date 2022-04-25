using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NATS.Client;
using System.Text;

namespace NatsTestAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class NatsController : Controller
    {
        private static IConnection _connect;
        string defaultSubscription = "nats.demo.pubsub";

        /// <summary>
        /// Post request to Publish message to the Subscription channel
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        //[Route("/post")]
        public IActionResult Post([FromBody] string message)
        {
            using(_connect = EnsureConnection())
            {
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
        }

    }
}
