using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using FHIR_CLI_Client.Services;
using System.Web;
using System.Net;
namespace FHIR_CLI_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            #region
            /**
            var client = new FhirClient(ConfigurationManager.AppSettings["SparkServerURI"]);

            //set up the services
            var patientService = new PatientService(client);
            var allergyService = new AllergyService(client);
            var observationService = new ObservationService(client);
            var medicationService = new MedicationService(client);

            //Part1

            //Part 2: create three stubb objects
            patientService.AddStubsToServer();

            var stubs = patientService.GenerateStubExamples();
            bool successful = true;
            foreach(var stub in stubs)
            {
                var res = client.SearchById<Patient>(stub.Id).GetResources().Select(s => (Patient) s).ToList();
                if (!res.Contains(stub))
                    successful = false;
            }
            if (successful)
                Console.WriteLine("Stub examples created successfully on server");
            else
                Console.WriteLine("ERROR: Something went wrong.");
            **/
            #endregion
            var order = new DiagnosticOrder();
            order.Subject = new ResourceReference();
            order.Subject.Reference = "spark1934";
            string content = FhirSerializer.SerializeResourceToJson(order);
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
