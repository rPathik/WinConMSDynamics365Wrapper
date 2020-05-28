using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using AWS.D365.Helper.Interfaces;
using AWS.D365.Helper.Model;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AWS.D365.Helper
{
    public class CaseRepository : ICaseRepository
    {
        private DynamcisHelper _dynamics365Connection;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<RootObject<CaseModel>> GetAllCases()
        {
            var serviceUrl = Environment.GetEnvironmentVariable("serviceUrl");

            _dynamics365Connection = new DynamcisHelper();
            var httpClient = _dynamics365Connection.GetHttpClient(serviceUrl);
            string operationUri = $"{serviceUrl}/api/data/V9.0/incidents?$select=title,ticketnumber&$top=10";
            // var operationUri = "incident";
            JObject contact = new JObject();

            var response = httpClient.GetAsync((operationUri)).Result;

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<RootObject<CaseModel>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                //throw _dynamics365Connection.GetException(response);
                throw new Exception("An error has occured");
            }

            // context.Logger.LogLine("all done");
        }

        /// <summary>
        /// GetCase
        /// </summary>
        /// <returns></returns>
        public async Task<RootObject<CaseModel>> GetCase(string Id)
        {
            var serviceUrl = Environment.GetEnvironmentVariable("serviceUrl");

            _dynamics365Connection = new DynamcisHelper();
            var httpClient = _dynamics365Connection.GetHttpClient(serviceUrl);
            string operationUri = $"{serviceUrl}/api/data/V9.0/incidents?$select=title,ticketnumber&$filter=ticketnumber eq '" + Id.Trim() +"'";
            // var operationUri = "incident";
            JObject contact = new JObject();

            var response = httpClient.GetAsync((operationUri)).Result;

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<RootObject<CaseModel>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                //throw _dynamics365Connection.GetException(response);
                throw new Exception("An error has occured");
            }

            // context.Logger.LogLine("all done");
        }

        
    }
}
