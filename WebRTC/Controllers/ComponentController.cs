using Swashbuckle.Swagger.Annotations;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebRTC.Controllers.Base;
using WebRTC.Models;
using WebRTC.Service;

namespace WebRTC.Controllers {

    [RoutePrefix("api/component")]
    public class ComponentController : BaseController {

        private readonly Swap _swap;
        public ComponentController(Swap swap) {
            _swap = swap;
        }

        /// <summary>
        /// RETORNA UM COMPONENTE [PROJECT]
        /// </summary>
        /// <remarks>Use este metodo para criar um dropdown de todos os projetos</remarks>
        /// <response code="400">VALIDAÇÃO DE DOMINIO</response>
        /// <response code="500">ERRO INTERNO NO SERVIDOR</response>
        [SwaggerResponse(200, "OK")]
        [Route("projects"), HttpGet]
        public async Task<HttpResponseMessage> List() {
            try {
                return await Response(ToDropDown(_swap.ListPorjects(), nameof(Project.Name), nameof(Project.Id)));
            }
            catch (Exception ex) {
                return await ExResponse(ex);
            }
        }
    }
}