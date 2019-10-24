using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebRTC.Service.Request;

namespace WebRTC.Controllers.Base {
    public class CoreController : Controller {

        //protected string RetriveToken() {
        //    return HttpContext.Session["Token"]?.ToString();
        //}

        protected async Task<T> Get<T>(string uri) {
            var request = new DataRequest<T>();
            var json = await request.Get(uri);
            return json;
        }

        protected async Task<T> Post<T>(string uri, object command) {
            var request = new DataRequest<T>();
            var json = await request.Post(uri, command);
            return json;
        }

        protected async Task PostAnonymous<T>(string uri, object command) {
            var request = new DataRequest<T>();
            await request.PostAnonymous(uri, command);
        }

        protected void Notify(Exception ex) {
            TempData["Error"] = ex.Message;
        }
    }
}