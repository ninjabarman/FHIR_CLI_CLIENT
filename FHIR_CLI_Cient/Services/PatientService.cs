using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;
using Hl7.Fhir.Support;

namespace FHIR_Test_Client.Services
{
    public class PatientService
    {
        private FhirClient _client;

        public PatientService(FhirClient client) { _client = client; }

        public IEnumerable<Patient> GetAllPatients()
        {
            var patients = new List<Patient>();
            int i = 1;
            bool patientsExist = true;
            while(patientsExist)
            {
                try
                {
                    string url = WebConfigurationManager.AppSettings["SparkServerURI"] + "/Patient/" + i.ToString();
                    var patient = _client.Read<Patient>(url);
                    patients.Add(patient);
                    i++;
                }
                catch (Exception ex)
                {
                    patientsExist = false;
                }
            }
            return patients;
        }

        public IEnumerable<Patient> GetPatientsByName(string firstName, string lastName)
        {
            var searchParameters = new SearchParams();
            searchParameters.Add("Given", firstName);
            searchParameters.Add("Family", lastName);
            
            var matches = new List<Patient>();
            var result = _client.Search(searchParameters, ResourceType.Patient.ToString());
            return result.Entry.Select(x => (Patient) x.Resource);
        }

        //STUB PATIENTS;
        #region
        public IEnumerable<Patient> GenerateStubExamples()
        {
            //stub1
            var stub1 = new Patient();
            var name1 = new HumanName();
            name1.Family = new List<string> {"Odenkirk"};
            name1.Given = new List<string> { "Bob" };

            stub1.Name = new List<HumanName> { name1 };
            stub1.Id = "b3tter_c@ll_$@ul";
            stub1.Gender = AdministrativeGender.Male;

            //stub2
            var stub2 = new Patient();
            var name2 = new HumanName();
            name2.Family = new List<string> { "Saggot" };
            name2.Given = new List<string> { "Bob" };

            stub2.Name = new List<HumanName> { name2 };
            stub2.Id = "full_house789";
            stub2.Gender = AdministrativeGender.Male;

            //stub3
            var stub3 = new Patient();
            var name3 = new HumanName();
            name3.Family = new List<string> { "Jones" };
            name3.Given = new List<string> { "Jessica" };

            stub3.Name = new List<HumanName> { name3 };
            stub3.Id = "m@r3lou$";
            stub3.Gender = AdministrativeGender.Female;

            return new List<Patient> { stub1, stub2, stub3};
        }

        public bool AddStubsToServer()
        {
            bool ret = true;
            var patients = GenerateStubExamples();
            foreach (var patient in patients)
            {
                try
                {
                    var patientEntry = _client.Create(patient);
                }
                catch(Exception ex)
                {
                    ret = false;
                }
            }
            return ret; 
        }

        public bool VerifyStubs(IEnumerable<Patient> stubs)
        {
            bool ret = true;
            foreach(var stub in stubs)
            {
                var result = _client.SearchById<Patient>(stub.Id);
                if (!result.Matches(stub))
                    ret = false;
            }
            return ret;
        }

        #endregion
    }
}