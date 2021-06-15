using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace ConsoleApp1
{
    public class Program
    {
        private static readonly string ClientId = "{enter your app client id here}";
        private static readonly string TenantId = "{enter your Tenant id here}";

        private static readonly IEnumerable<string> Scopes = new List<string>
        {
            "User.Read",
            "api://{target API clinet id}/user_impersonation"
        };

        static async Task Main(string[] args)
        {
            var options = new PublicClientApplicationOptions
                {
                    AzureCloudInstance = AzureCloudInstance.AzurePublic,
                    AadAuthorityAudience = AadAuthorityAudience.AzureAdMyOrg,
                    ClientId = ClientId,
                    RedirectUri = "http://localhost",
                    TenantId = TenantId
                   
                };

            var ApplicationBase = PublicClientApplicationBuilder.CreateWithApplicationOptions(options)
                                                .WithRedirectUri("http://localhost").Build();

            AuthenticationResult result = await ApplicationBase.AcquireTokenInteractive(Scopes).ExecuteAsync();


            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add(
                "Authorization", "Bearer " + result.AccessToken);
            
            Uri url = new Uri("https://{appname}.azurewebsites.net/api/Function1?name=Martina");

            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine(body);
        }
    }
}
