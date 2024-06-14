using System.Text;

namespace QualLMS.WebAppMvc.Models
{
    public class Client(string? BaseURL)
    {
        public async Task<string> PostAPI(string url, string param)
        {
            string returnModel = "";

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };

            using (HttpClient client = new HttpClient(handler))
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
