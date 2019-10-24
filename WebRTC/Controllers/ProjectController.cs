using Swashbuckle.Swagger.Annotations;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebRTC.Controllers.Base;
using WebRTC.Models;
using WebRTC.Service;

namespace WebRTC.Controllers {
    [RoutePrefix("api/project")]
    public class ProjectController : BaseController {

        private readonly Swap _swap;
        public ProjectController(Swap swap) {
            _swap = swap;
        }

        /// <summary>
        /// RETORNA UMA LISTA [PROJECT]
        /// </summary>
        /// <remarks>Use esta chamada para obter a listagem de todos os projetos</remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("list"), HttpGet]
        public async Task<HttpResponseMessage> List() {
            try {
                return await Response(_swap.ListPorjects());
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// RETORNA UM OBJETO [PROJECT]
        /// </summary>
        /// <remarks>Use esta chamada para obter um projeto pela sua chave de identificação</remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("byId/{id}"), HttpGet]
        public async Task<HttpResponseMessage> ById(string id) {
            try {
                return await Response(_swap.GetProjectById(id));
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// RETORNA UM OBJETO [PROJECT]
        /// </summary>
        /// <remarks>Use esta chamada para obter um projeto pela sua chave de sessão</remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("bySessionId/{id}"), HttpGet]
        public async Task<HttpResponseMessage> BySessionId(string id) {
            try {
                return await Response(_swap.GetProjectBySessionId(id));
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// CRIA UM OBJETO [PROJECT]
        /// </summary>
        /// <remarks>
        /// Use esta chamada para criar um projeto novo na memória
        /// Tenha em mãos os dados de integração do tokbox
        /// </remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("create"), HttpPost]
        public async Task<HttpResponseMessage> AddProject([FromBody]ProjectRequest request) {
            try {
                _swap.AddProject(request.Key, request.Secret, request.Name);
                return await Response();
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// DELETA UM OBJETO [PROJECT]
        /// </summary>
        /// <remarks>
        /// Use esta chamada para deletar um projeto da memória
        /// </remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("delete/{id}"), HttpPost]
        public async Task<HttpResponseMessage> DeleteById(string id) {
            try {
                _swap.RemoveProject(id);
                return await Response();
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }
    }
}