using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MSGraph
{
    internal class Program
    {
        static void Main(string[] args)
        {
           
            SendMail().GetAwaiter().GetResult();
            Console.WriteLine("Mail Send");
            Console.ReadLine();
        }

        static private async Task SendMail()
        {
            var aadInstance = "https://login.microsoftonline.com/{0}";
            var clientId = "<client id>";
            var tenantId = "<aad tenant id>";
            var clientSecret = "<client secret";
            var fromAddress = "<mail from>";
            var toAddress = "<mail to>";


            ClientCredential cred = new ClientCredential(clientId, clientSecret);
            AuthenticationContext authContext = new AuthenticationContext(String.Format(aadInstance, tenantId));

            AuthenticationResult authResult = await authContext.AcquireTokenAsync("https://graph.microsoft.com/", cred);

            var graphServiceClient = new GraphServiceClient(
                new DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);

                    return Task.CompletedTask;
                }));


            var message = new Message
            {
                Subject = "Can I send mail on behalf of" + fromAddress,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = "Yes you can!"
                },
                ToRecipients = new List<Recipient>()
                {
                    new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = toAddress
                        }
                    }
                }
            };

            
            
            await graphServiceClient.Users[fromAddress]
                      .SendMail(message, true)
                      .Request()
                      .PostAsync();

        }
    }
}
