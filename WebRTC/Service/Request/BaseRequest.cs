using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebRTC.Service.Request {
    public class BaseRequest {
        protected BaseRequest() {
            _baseUri = ConfigurationManager.AppSettings["ApiEndpoint"];
        }

        #region [ properties ]

        private readonly string _baseUri;
        private readonly HttpClient _client = new HttpClient();

        #endregion

        #region [ send async ]

        protected async Task<HttpResponseMessage> SendAsync(RequestMethod typerequest, string requestUri, object parametros = null, string token = "") {
            _client.BaseAddress = new Uri($"{_baseUri}/api/");
            _client.Timeout = TimeSpan.FromMinutes(30);

            switch (typerequest) {
                case RequestMethod.Get: {
                        if (!string.IsNullOrEmpty(token))
                            return await Get(requestUri, token);
                        return await Get(requestUri);
                    }
                case RequestMethod.Post: {
                        if (!string.IsNullOrEmpty(token))
                            return await Post(requestUri, parametros, token);
                        return await Post(requestUri, parametros);
                    }
                case RequestMethod.Put: {
                        if (!string.IsNullOrEmpty(token))
                            return await Put(requestUri, parametros, token);
                        return await Put(requestUri, parametros);
                    }
            }
            return null;
        }

        #endregion

        #region [ get ]

        private async Task<HttpResponseMessage> Get(string requestUri, string token = "") {
            using (_client) {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(token))
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                return await _client.GetAsync(requestUri);
            }
        }

        #endregion

        #region [ post ]

        private async Task<HttpResponseMessage> Post(string requestUri, object parameters, string token = "") {
            using (_client) {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(token))
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var json = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                return await _client.PostAsync(requestUri, content);
            }
        }

        #endregion

        #region [ put ]

        private async Task<HttpResponseMessage> Put(string requestUri, object parameters, string token = "") {
            using (_client) {
                if (!string.IsNullOrEmpty(token))
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var json = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                return await _client.PutAsync(requestUri, content);
            }
        }

        #endregion
    }
}