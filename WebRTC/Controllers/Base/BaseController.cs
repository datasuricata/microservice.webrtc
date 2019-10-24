using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebRTC.Service.Validators;

namespace WebRTC.Controllers.Base {
    public class BaseController : ApiController {

        protected async Task<HttpResponseMessage> Response(object obj = null) {

            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }

        protected async Task<HttpResponseMessage> ExResponse(Exception ex) {
            if (ex.GetType() == typeof(Validator))
                return Request.CreateResponse(HttpStatusCode.BadRequest, new {
                    ex.Message,
                    Date = DateTime.Now,
                });
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new {
                    Message = "Ops, ocorreu um erro interno no servidor. Contate o suporte",
                    Exception = ex.ToString(),
                    Date = DateTime.Now,
                });
        }

        protected List<SelectListItem> ToDropDown<T>(IEnumerable<T> list, string text, string value) {
            var dropdown = list.Select(item => new SelectListItem {
                Text = item.GetType()
                    .GetProperty(text)
                    .GetValue(item, null) as string,

                Value = item.GetType()
                    .GetProperty(value)
                    .GetValue(item, null) as string
            }).ToList();

            return dropdown.OrderBy(x => x.Text).ToList();
        }

        protected string GetIpAddress() {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            return ip;
        }
    }
}