using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;
namespace FHIR_CLI_Client.Services
{
    public class AllergyService
    {
        private FhirClient _client;

        public AllergyService(FhirClient client) { _client = client; }

        public IEnumerable<AllergyIntolerance> GetAllAllergies()
        {
            var patients = new List<AllergyIntolerance>();
            int i = 1;
            bool allergiesExist = true;
            while (allergiesExist)
            {
                try
                {
                    string url = ConfigurationManager.AppSettings["SparkServerURI"] + "/AllergyIntolerance/" + i.ToString();
                    var patient = _client.Read<AllergyIntolerance>(url);
                    patients.Add(patient);
                    i++;
                }
                catch (Exception ex)
                {
                    allergiesExist = false;
                }
            }
            return patients;
        }
        public IEnumerable<AllergyIntolerance> GetAllergiesByName(string text)
        {
            var searchParameters = new SearchParams();
            searchParameters.Add("name", text);

            var result = _client.Search(searchParameters, ResourceType.AllergyIntolerance.ToString());
            return result.Entry.Select(s => (AllergyIntolerance)s.Resource);
        }
    }
}