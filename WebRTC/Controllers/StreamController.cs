using Swashbuckle.Swagger.Annotations;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebRTC.Controllers.Base;
using WebRTC.Models;
using WebRTC.Service;

namespace WebRTC.Controllers {
    [RoutePrefix("api/stream")]
    public class StreamController : BaseController {

        private readonly Swap _swap;
        public StreamController(Swap swap) {
            _swap = swap;
        }

        /// <summary>
        /// RETORNA UMA LISTA [STREAM]
        /// </summary>
        /// <remarks>Use esta chamada para obter a listagem de todos os stream que compões as sessões</remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("list"), HttpGet]
        public async Task<HttpResponseMessage> List() {
            try {
                return await Response(_swap.ListStreams());
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// RETORNA UMA LISTA [STREAM]
        /// </summary>
        /// <remarks>Use esta chamada para obter a listagem de todos os stream que compões as sessões ativas</remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("listActives"), HttpGet]
        public async Task<HttpResponseMessage> ListActives() {
            try {
                return await Response(_swap.ListStreamsActives());
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// RETORNA UMA LISTA [STREAM]
        /// </summary>
        /// <remarks>Use esta chamada para obter a listagem de todos os stream que compões as sessões pela chave de identificação do projeto</remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("listByProjectId/{id}"), HttpGet]
        public async Task<HttpResponseMessage> ListByProject(string id) {
            try {
                return await Response(_swap.ListStreamsByProjectId(id));
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// RETORNA UM OBJETO [STREAM]
        /// </summary>
        /// <remarks>Use esta chamada para obter um stream pela sua chave de identificação</remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("byId/{id}"), HttpGet]
        public async Task<HttpResponseMessage> ById(string id) {
            try {
                return await Response(_swap.GetStreamById(id));
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// HABILITA A SESSÃO [STREAM]
        /// </summary>
        /// <remarks>
        /// Use esta chamada para ativar a sessão do stream
        /// Podemos abrir os links de transmissão e visualização após a ativação com o broadcast ativo
        /// </remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("startSession/{id}"), HttpGet]
        public async Task<HttpResponseMessage> StartSession(string id) {
            try {
                _swap.StartSession(id, GetIpAddress());
                return await Response();
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// DESABILITA A SESSÃO [STREAM]
        /// </summary>
        /// <remarks>
        /// Use esta chamada para desabilitar a sessão do stream
        /// Limpa as informações de sessão da memória e desabilita o broadcast
        /// </remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("stopSession/{id}"), HttpGet]
        public async Task<HttpResponseMessage> StopSession(string id) {
            try {
                _swap.StopSession(id);
                return await Response();
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// CRIA UM TOKEN [PUBLISHER]
        /// </summary>
        /// <remarks>
        /// Use esta chamada para vincular um token de publicação 
        /// O token é vinculado a chave de identificação da sessão e o endereço ip de origem
        /// A publicação é derivador de quem vai transmitir o vídeo
        /// </remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("publish/{id}"), HttpGet]
        public async Task<HttpResponseMessage> Publish(string id) {
            try {
                var token = _swap.Publish(id);
                var session = _swap.GetStreamById(id);

                return await Response(new ServerResponse {
                    ApiKey = session.Project.Key,
                    SessionId = session.SessionReferenceId,
                    Token = token,
                    Info = $"{session.Project?.Name}-{session.Name}",
                });
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// CRIA UM TOKEN [SUBSCRIBER]
        /// </summary>
        /// <remarks>
        /// Use esta chamada para vincular um token de inscrição 
        /// O token é vinculado a chave de identificação da sessão e o endereço ip de origem
        /// A publicação é derivador de quem vai assitir o vídeo
        /// </remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("subscribe/{id}"), HttpGet]
        public async Task<HttpResponseMessage> Subscribe(string id) {
            try {
                var token = _swap.Subscribe(id);
                var session = _swap.GetStreamById(id);

                return await Response(new ServerResponse {
                    ApiKey = session.Project.Key,
                    SessionId = session.SessionReferenceId,
                    Token = token,
                    Info = $"{session.Project?.Name}-{session.Name}",
                });
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// CRIA UM OBJETO [STREAM]
        /// </summary>
        /// <remarks>
        /// Use esta chamada para criar um stream novo na memória
        /// Vincule ao menos uma chave de identificação de projeto
        /// </remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("create"), HttpPost]
        public async Task<HttpResponseMessage> AddStream([FromBody]StreamRequest request) {
            try {
                _swap.AddStream(request.ProjectId, request.Name);
                return await Response();
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// DELETA UM OBJETO [STREAM]
        /// </summary>
        /// <remarks>
        /// Use esta chamada para deletar um stream da memória
        /// </remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("delete/{id}"), HttpPost]
        public async Task<HttpResponseMessage> Delete(string id) {
            try {
                _swap.RemoveStream(id);
                return await Response();
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }
    }
}