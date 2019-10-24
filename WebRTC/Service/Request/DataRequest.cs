using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebRTC.Service.Validators;

namespace WebRTC.Service.Request {
    public class DataRequest<T> : BaseRequest {
        public DataRequest(){
        }

        private async Task HandleResponse(HttpResponseMessage response) {
            if (!response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<MessageValidator>(content);

                if (response.StatusCode == HttpStatusCode.BadRequest) {
                    Validator.When(result == null, "Ops! falha na requisição");
                    Validator.When(!string.IsNullOrEmpty(result.Message), result.Message);
                }

                if(response.StatusCode == HttpStatusCode.InternalServerError) {
                    throw new ArgumentException("Ops! parece que um percevejo invadiu nosso servidor, já estamos preparando o lança chamas.");
                }
            }
        }

        public async Task<T> Get(string endpoint, string token = "") {
            var response = await SendAsync(RequestMethod.Get, endpoint, null, token);
            var obj = await response.Content.ReadAsStringAsync();
            await HandleResponse(response);

            return JsonConvert.DeserializeObject<T>(obj);
        }

        public async Task<T> Post(string endpoint, object command, string token = "") {
            var response = await SendAsync(RequestMethod.Post, endpoint, command, token);
            var obj = await response.Content.ReadAsStringAsync();
            await HandleResponse(response);
            return JsonConvert.DeserializeObject<T>(obj);
        }

        public async Task PostAnonymous(string endpoint, object command, string token = "") {
            var response = await SendAsync(RequestMethod.Post, endpoint, command, token);
            var obj = await response.Content.ReadAsStringAsync();
            await HandleResponse(response);
        }
    }
}