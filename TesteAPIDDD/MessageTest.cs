using Entities.Entities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TesteAPIDDD
{
    [TestClass]
    public class MessageTest
    {
        public static string Token { get; set; }

        [TestMethod]
        public void Add()
        {
            /*
             *Esse teste cria uma mensagem com a data e horario de teste no banco de dados.
             *Lembrar de implemetar um metodo para remover a mensagem.
             *
             *Caso usar lembrar de apagar manualmente.
             */
            var message = new
            {
                title = $"Data de teste: ${DateTime.Now.ToString("dd/MM/yyyy HH:mm")}"
            };

            var result = ApiPost("https://localhost:7131/api/AddMessage", message).Result;

            var MessagesList = JsonConvert.DeserializeObject<Message[]>(result).ToList();

            Assert.IsFalse(MessagesList.Any());
        }

        //[TestMethod]
        //public void Delete()
        //{
        //    var message = new
        //    {
        //        title = $"Teste de deleção: ${DateTime.Now.ToString("dd/MM/yyyy HH:mm")}"
        //    };

        //    var postMessageResult = ApiPost("https://localhost:7131/api/AddMessage", message).Result;

        //    var MessagesList = JsonConvert.DeserializeObject<Message[]>(postMessageResult).ToList();

        //    var messegeToDelete = MessagesList.Where(x => x.Title == message.title);

        //    var result = ApiPost("https://localhost:7131/api/DeleteMessage", message).Result;
        //    var notifiesList = JsonConvert.DeserializeObject<Message[]>(result).ToList();

        //    Assert.IsFalse(notifiesList.Any());
        //}

        [TestMethod]
        public void List()
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
                string Login = "Local001@host.com";
                string Password = "Local001@host.com";

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