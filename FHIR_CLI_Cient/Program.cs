using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System.Web;
using System.Net;
using Newtonsoft.Json;

namespace FHIR_CLI_Client
{
    class Program
    {
        /// <summary>
        /// Creates a string representing an instance of DiagnosticOrder which has been serialized to json
        /// </summary>
        /// <returns>string</returns>
        private static string GenerateDiagnosticOrder(string subject, int i, int j)
        {
            var order = new DiagnosticOrder();
            order.Subject = new ResourceReference();
            order.Subject.Reference = subject;
            string time = JsonConvert.SerializeObject(new Dictionary<string, int>() { { "start", i }, { "end", j } });
            order.FhirComments = new List<string>() { time };
            return FhirSerializer.SerializeResourceToJson(order);
        }
        static void Main(string[] args)
        {
            string content = GenerateDiagnosticOrder("spark1934", 1, 1);
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            WebRequest request = WebRequest.Create("http://localhost:49438/api/v1/values");
            request.Method = "POST";
            request.ContentType = "text/json";
            request.ContentLength = bytes.Length;
            var stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            var st = new System.IO.StreamReader(request.GetResponse().GetResponseStream());
            Console.Write(st.ReadToEnd());
            Console.ReadLine();
        }
    }
}
