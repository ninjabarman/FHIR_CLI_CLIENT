using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace FHIR_Test_Client.Services
{
    public class ObservationService
    {
        private FhirClient _client;

        public ObservationService(FhirClient client) { _client = client; }

        public IEnumerable<Observation> GetAllObservations()
        {
            var observations = new List<Observation>();
            int i = 1;
            bool observationsExist = true;
            while (observationsExist)
            {
                try
                {
                    string url = WebConfigurationManager.AppSettings["SparkServerURI"] + "/Observation/" + i.ToString();
                    var observation = _client.Read<Observation>(url);
                    observations.Add(observation);
                    i++;
                }
                catch (Exception ex)
                {
                    observationsExist = false;
                }
            }
            return observations;
        }

        public IEnumerable<Observation> GetObservationsByName(string text)
        {
            var searchParameters = new SearchParams();
            searchParameters.Add("name", text);

            var results = _client.Search(searchParameters, ResourceType.Observation.ToString());
            return results.Entry.Select(s => (Observation)s.Resource);
        }

    }
}