using Amazon.Lambda.Core;
using Amazon.SecretsManager.Model;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using System.IO;
using Amazon;
using Newtonsoft.Json;
using AWS.D365.Helper.Model;

namespace AWS.D365.Helper
{
    public class DynamcisHelper
    {
        HttpMessageHandler messageHandler;

        public HttpClient GetHttpClient(string serviceUrl)
        {
            messageHandler = new OAuthMessageHandler(new HttpClientHandler());
            //  context.Logger.LogLine(m);
            HttpClient httpClient = new HttpClient(messageHandler)
            {
                // context.Logger.LogLine(httpClient.BaseAddress.ToString());
                BaseAddress = new Uri(string.Format("{0}/api/data/V9.0/", serviceUrl)),

                Timeout = new TimeSpan(0, 2, 0)  //2 minutes
            };
            //context.Logger.LogLine(httpClient.BaseAddress.ToString());
            return httpClient;

        }
    }


    class OAuthMessageHandler : DelegatingHandler
    {
        private AuthenticationHeaderValue authHeader;

        public string token;
        public OAuthMessageHandler(HttpMessageHandler innerHandler)
          : base(innerHandler)
        {


            string secretName = "DynamicsWrapper";
            string region = "eu-west-2";
            string secret = "";

            MemoryStream memoryStream = new MemoryStream();

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest();
            request.SecretId = secretName;
            request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

            GetSecretValueResponse response = null;

            // In this sample we only handle the specific exceptions for the 'GetSecretValue' API.
            // See https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
            // We rethrow the exception by default.

            try
            {
                response = client.GetSecretValueAsync(request).Result;
            }
            catch (Exception ex)
            {

            }
            Secrets secCredentials = new Secrets();
            secCredentials  = JsonConvert.DeserializeObject<Secrets>(response.SecretString);


            var clientId = secCredentials.clientId;
            var clientSecret = secCredentials.clientSecret;
            var aadInstance = secCredentials.aadInstance;
            var tenantID = secCredentials.tenantID;
            var serviceUrl = secCredentials.serviceUrl;

            var clientcred = new ClientCredential(clientId, clientSecret);


            var authenticationContext = new AuthenticationContext($"{aadInstance}{tenantID}");
            AuthenticationResult authenticationResult;

            authenticationResult = authenticationContext.AcquireTokenAsync(serviceUrl, clientcred).Result;


            authHeader = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
            token = authenticationResult.AccessToken;
            // MemoryCache.Default.Add("crm_token", authenticationResult.AccessToken, authenticationResult.ExpiresOn);
        }

        protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Headers.Authorization = authHeader;
            return base.SendAsync(request, cancellationToken);
        }
    }
}
