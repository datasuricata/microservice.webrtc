using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebRTC.Controllers.Base;
using WebRTC.Filters;
using WebRTC.Models;

namespace WebRTC.Controllers {
    public class LiveController : CoreController {
        #region - general -

        [SessionFilter]
        public async Task<ActionResult> Publish(string id) {
            try {
                return View(await Get<ServerResponse>($"stream/publish/{id}"));
            }
            catch (Exception ex) {
                Notify(ex);
                return RedirectToAction("Index", "Home");
            }
        }
        public async Task<ActionResult> Subscribe(string id) {
            try {
                return View(await Get<ServerResponse>($"stream/subscribe/{id}"));
            }
            catch (Exception ex) {
                Notify(ex);
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}