using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using AWS.D365.Helper;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;

namespace DynamicsWrapper.Helper
{
    public class FunctionHelper
    {

        private DynamcisHelper _dynamics365Connection;
        private void CreateCase(ILambdaContext context, string caseTitle)
        {
            _dynamics365Connection = new DynamcisHelper();
            string serviceUrl = "https://boundless-dev.crm11.dynamics.com";
            var httpClient = _dynamics365Connection.GetHttpClient(serviceUrl);

            // var operationUri = "incident";
            JObject contact = new JObject();

            string baseUri = $"{serviceUrl}/api/data/V9.0/incidents";
            contact.Add("title", "Case regading device 1 -" + caseTitle);
            contact.Add("c9_MembershipNumber@odata.bind", "/accounts(750ab432-9d3e-ea11-a812-000d3a7ed588)");
            contact.Add("customerid_contact@odata.bind", "contacts(a6c75e2f-9d3e-ea11-a812-000d3a7ed677)");

            HttpRequestMessage createrequest1 = new HttpRequestMessage(HttpMethod.Post, baseUri);
            createrequest1.Content = new StringContent(contact.ToString());
            context.Logger.LogLine(baseUri);
            context.Logger.LogLine(contact.ToString());
            createrequest1.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            try
            {
                HttpResponseMessage response = httpClient.SendAsync(createrequest1).Result;
                if (response.IsSuccessStatusCode)
                {


                }
            }
            catch (Exception ex)
            {
                context.Logger.LogLine(ex.Message);
            }
            context.Logger.LogLine("all done");
        }
    }
}
