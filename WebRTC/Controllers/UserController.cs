using Swashbuckle.Swagger.Annotations;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebRTC.Controllers.Base;
using WebRTC.Models;
using WebRTC.Service;
using WebRTC.Service.Request;

namespace WebRTC.Controllers {
    [RoutePrefix("api/user")]
    public class UserController : BaseController {

        private readonly Swap _swap;
        public UserController(Swap swap) {
            _swap = swap;
        }

        /// <summary>
        /// RETORNA UMA LISTA [ACCOUNT]
        /// </summary>
        /// <remarks>Use esta chamada para obter a listagem de todos os usuarios</remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("list"), HttpGet]
        public async Task<HttpResponseMessage> List() {
            try {
                return await Response(_swap.ListAccounts().Select(x => new {
                    x.Id,
                    x.CreatedAt,
                    x.Email,
                    x.Role,
                }));
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// RETORNA UM OBJETO [ACCOUNT]
        /// </summary>
        /// <remarks>Use esta chamada para obter um usuario pela sua chave de identificação</remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("byId/{id}"), HttpGet]
        public async Task<HttpResponseMessage> ById(string id) {
            try {
                var account = _swap.GetAccountById(id);
                return await Response(new { account.Id, account.CreatedAt, account.Email, account.Role });
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// RETORNA UM OBJETO [ACCOUNT]
        /// </summary>
        /// <remarks>Use esta chamada para obter um usuario pelo email</remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("byEmail/{email}"), HttpGet]
        public async Task<HttpResponseMessage> ByEmail(string email) {
            try {
                var account = _swap.GetAccountByEmail(email);
                return await Response(new { account.Id, account.CreatedAt, account.Email, account.Role });
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// AUTENTICA [ACCOUNT]
        /// </summary>
        /// <remarks>
        /// Use esta chamada para criar uma autenticação de usuario
        /// </remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("auth"), HttpPost]
        public async Task<HttpResponseMessage> Authorize([FromBody]AccountRequest request) {
            try {
                var account = _swap.Authorize(request.Email, request.Password);
                return await Response(new AccountResponse {
                    Id = account.Id,
                    Email = account.Email,
                    Role = account.Role,
                    Project = account.Project,
                });
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// CRIA UM OBJETO [ACCOUNT]
        /// </summary>
        /// <remarks>
        /// Use esta chamada para criar um usuario novo na memória
        /// </remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("create/{secret}"), HttpPost]
        public async Task<HttpResponseMessage> AddAccount(string secret, [FromBody]AccountRequest request) {
            try {
                var bind = ConfigurationManager.AppSettings["PassBind"];
                _swap.AddAccount(request.Email, request.Password, secret, bind);
                return await Response();
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// SEEDER [ACCOUNT]
        /// </summary>
        /// <remarks>
        /// Use esta chamada para criar os usuários padrões iniciais na memória
        /// </remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("seeder/{secret}"), HttpPost]
        public async Task<HttpResponseMessage> Seeder(string secret) {
            try {
                var bind = ConfigurationManager.AppSettings["PassBind"];
                _swap.Seeder(secret, bind);
                return await Response();
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// VINCULAR PROJETO [ACCOUNT]
        /// </summary>
        /// <remarks>
        /// Use esta chamada vincular uma chave de projeto a um usuário
        /// </remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("link/{secret}"), HttpPost]
        public async Task<HttpResponseMessage> LinkProject(string secret, [FromBody]LinkRequest request) {
            try {
                var bind = ConfigurationManager.AppSettings["PassBind"];
                _swap.LinkAccount(request.ProjectId, request.Email, secret, bind);
                return await Response();
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }

        /// <summary>
        /// CRIA UM OBJETO [ACCOUNT]
        /// </summary>
        /// <remarks>
        /// Use esta chamada para remover um usuario da memória
        /// </remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("delete/{id}"), HttpPost]
        public async Task<HttpResponseMessage> RemoveAccount(string id) {
            try {
                _swap.RemoveAccount(id);
                return await Response();
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }
    }
}