using Chatter.UI.Models.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Chatter.UI.Pages.Message
{
    [Authorize]
    public class SendModel : PageModel
    {
        [BindProperty]
        public SendMessageModel Input { get; set; }

        public void OnGet()
        {

        }

        public async Task OnPostAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var accessToken = await GetToken();

                    client.SetBearerToken(accessToken);

                    Input.UserId = User.Claims.FirstOrDefault(x => x.Type.Equals("sub"))?.Value;
                    await client.PostAsJsonAsync("https://localhost:44314/api/message/send", Input);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async Task<string> GetToken()
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"client_id", "Chatter.UI" },
                    {"client_secret", "Secret" },
                    {"grant_type", "client_credentials" }
                });

                var response = await client.PostAsync("https://localhost:44331/connect/token", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                var deserializedResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                return deserializedResponse.access_token;
            }
        }
    }
}