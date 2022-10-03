using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TesteAPIDDD
{
    [TestClass]
    public class UnitTest1
    {
        public static string Token { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            var result = ApiPost("https://localhost:7131/api/ListMessage").Result;

            var MessagesList = JsonConvert.DeserializeObject<Message[]>(result).ToList();

            Assert.IsTrue(MessagesList.Any());
        }

        public void GetToken()
        {
            string urlApiGetToken = "https://localhost:7131/api/CreateToken";

            using (var cliente = new HttpClient())
            {
                string Login = "local@host.com";
                string Password = "@LocalHost001";

                var data = new
                {
                    email = Login,
                    password = Password,
                    CPF = "string"
                };

                string JsonObject = JsonConvert.SerializeObject(data);
                var content = new StringContent(JsonObject, Encoding.UTF8, "application/json");

                var resultado = cliente.PostAsync(urlApiGetToken, content);
                resultado.Wait();

                if (resultado.Result.IsSuccessStatusCode)
                {
                    var tokenJson = resultado.Result.Content.ReadAsStringAsync();
                    Token = JsonConvert.DeserializeObject(tokenJson.Result).ToString();
                }
            }
        }

        public async Task<string> ApiPost(string url, object data = null)
        {
            string JsonObject = data != null ? JsonConvert.SerializeObject(data) : "";
            var content = new StringContent(JsonObject, Encoding.UTF8, "application/json");

            GetToken();
            if (!string.IsNullOrWhiteSpace(Token))
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                    var response = client.PostAsync(url, content);
                    response.Wait();

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var returnResponse = await response.Result.Content.ReadAsStringAsync();

                        return returnResponse;
                    }
                }
            }
            return null;
        }
    }
}