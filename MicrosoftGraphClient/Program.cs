using System;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MicrosoftGraphClient
{
    class Program
    {
        private const string _clientId = "";
        private const string _tenantId = "";
        static async Task Main(string[] args)
        {
            IPublicClientApplication app;
            app = PublicClientApplicationBuilder
                .Create(_clientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
                .Build();
            
            List<string> scopes = new List<string>
            {
                "user.read"
            };

            DeviceCodeProvider provider = new DeviceCodeProvider(app, scopes);

            GraphServiceClient client = new GraphServiceClient(provider);

            User myProfile = await client.Me.Request().GetAsync();

            await Console.Out.WriteLineAsync($"Name:\t{myProfile.DisplayName}"); 
        }
    }
}
