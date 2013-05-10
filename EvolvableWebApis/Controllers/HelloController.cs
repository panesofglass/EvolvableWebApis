using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EvolvableWebApis.Models;

namespace EvolvableWebApis.Controllers
{
    public class HelloController : ApiController
    {
        private static readonly Dictionary<string, string> _helloMessages = new Dictionary<string, string>();

        public string Get()
        {
            return "Hello, world!";
        }

        public string Get(string id)
        {
            if (!_helloMessages.ContainsKey(id))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Could not find the requested message."));

            return _helloMessages[id];
        }

        public HttpResponseMessage Post(Hello message)
        {
            if (message == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You must supply a hello message."));

            string id = Guid.NewGuid().ToString();

            _helloMessages.Add(id, message.Message);
            var link = Url.Link("DefaultApi", new { controller = "Hello", id = id });
            var response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(link);
            return response;
        }

        public HttpResponseMessage Put(string id, Hello message)
        {
            if (message == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You must supply a hello message."));

            if (string.IsNullOrEmpty(id))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You must specify an id."));

            _helloMessages[id] = message.Message;

            HttpResponseMessage response = _helloMessages.ContainsKey(id) ?
                Request.CreateResponse(HttpStatusCode.Created) :
                Request.CreateResponse();

            var link = Url.Link("DefaultApi", new { controller = "Hello", id = id });
            response.Headers.Location = new Uri(link);
            return response;
        }
    }
}
