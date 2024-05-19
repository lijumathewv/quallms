using System.Text.Json;
using System.Text;

namespace QualLMS.WebApp.Data
{
    public class Client (string? BaseURL)
    {
        public async Task<string> PostAPI(string url,  string param)
        {
            string returnModel = "";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL!);

                HttpResponseMessage response;

                if (string.IsNullOrEmpty(param))
                {
                    response = await client.PostAsync(url, null);
                }
                else
                {
                    HttpContent content = new StringContent(param, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(url, content);
                }

                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync();

                returnModel = result.Result.ToString();

            }

            return returnModel;
        }
    }
}
