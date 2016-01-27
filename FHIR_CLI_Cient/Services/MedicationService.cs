using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;

namespace FHIR_Test_Client.Services
{
    public class MedicationService
    {
        private FhirClient _client;
        
        public MedicationService(FhirClient client) { _client = client; }

        public IEnumerable<Medication> GetAllMedications()
        {
            var medications = new List<Medication>();
            int i = 1;
            bool medicationsExist = true;
            while (medicationsExist)
            {
                try
                {
                    string url = WebConfigurationManager.AppSettings["SparkServerURI"] + "/Medicaton/" + i.ToString();
                    var medication = _client.Read<Medication>(url);
                    medications.Add(medication);
                    i++;
                }
                catch (Exception ex)
                {
                    medicationsExist = false;
                }
            }
            return medications;
        }

        IEnumerable<Medication> GetMedicationsByName(string name)
        {
            var searchParameters = new SearchParams();
            searchParameters.Add("name", name);

            var result = _client.Search(searchParameters, ResourceType.Medication.ToString());
            return result.Entry.Select(s => (Medication)s.Resource);
        }

    }
}