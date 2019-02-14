using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace HelloWorld
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            if (name == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                name = data?.name;
            }

            
            XmlDocument XMLEventList = new XmlDocument();
            XMLEventList.LoadXml("https://campusevents.uncc.edu/feed/cci-student-xml");

            string json = JsonConvert.SerializeXmlNode(XMLEventList);
       

            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please enter your name in the request parameter.")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name + "\n\n Events on UNCC's Campus right now!: \n" + json);

            
           
        }
    }
}
