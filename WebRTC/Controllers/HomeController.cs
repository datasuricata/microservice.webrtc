using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebRTC.Controllers.Base;
using WebRTC.Filters;
using WebRTC.Models;

namespace WebRTC.Controllers {
    [SessionFilter]
    public class HomeController : CoreController {
        #region - general -

        public async Task<ActionResult> Index() {
            try {
                ViewBag.Dropdown = await Get<List<SelectListItem>>("component/projects");

                if (Session["Project"] != null)
                    return View(await Get<List<Stream>>($"stream/listByProjectId/{Session["Project"]}"));

                return View(new List<Stream>());
            }
            catch (Exception ex) { Notify(ex); }
            return View();
        }
        public async Task<ActionResult> Publish(string id) {
            try {
                return View(await Get<ServerResponse>($"stream/publish/{id}"));
            }
            catch (Exception ex) { Notify(ex); return RedirectToAction(nameof(Index)); }
        }
        public async Task<ActionResult> Subscribe(string id) {
            try {
                return View(await Get<ServerResponse>($"stream/subscribe/{id}"));
            }
            catch (Exception ex) { Notify(ex); return RedirectToAction(nameof(Index)); }
        }
        public async Task<ActionResult> Links() {
            try {
                ViewBag.Endpoint = ConfigurationManager.AppSettings["ApiEndpoint"];
                return View(await Get<List<Stream>>("stream/listActives"));
            }
            catch (Exception ex) {
                Notify(ex);
                return View(new List<Stream>());
            }
        }

        #endregion

        #region - stream -

        public async Task<ActionResult> ListStreams() {
            try {
                return View(await Get<List<Stream>>("stream/list"));
            }
            catch (Exception ex) { Notify(ex); return View(new List<Stream>()); }
        }
        [HttpPost]
        public async Task<ActionResult> Stream(string name, string projectId) {
            try {
                var request = new StreamRequest {
                    Name = name,
                    ProjectId = projectId,
                };
                await Post<dynamic>("stream/create", request);
            }
            catch (Exception ex) { Notify(ex); }
            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult> DeleteStream(string id) {
            try {
                await PostAnonymous<dynamic>($"stream/delete/{id}", null);
            }
            catch (Exception ex) { Notify(ex); }
            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult> Broadcast(string id, bool broadcast) {
            try {
                if (broadcast)
                    await Get<dynamic>($"stream/stopSession/{id}");
                else
                    await Get<dynamic>($"stream/startSession/{id}");
            }
            catch (Exception ex) { Notify(ex); }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region - project -

        public async Task<ActionResult> ListProjects() {
            try {
                return View(await Get<List<Project>>("project/list"));
            }
            catch (Exception ex) { Notify(ex); return View(new List<Project>()); }
        }
        [HttpPost]
        public async Task<ActionResult> Project(string name, int key, string secret) {
            try {
                var request = new ProjectRequest {
                    Key = key,
                    Name = name,
                    Secret = secret,
                };
                await Post<dynamic>("project/create", request);
            }
            catch (Exception ex) { Notify(ex); }
            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult> DeleteProject(string id) {
            try {
                await PostAnonymous<dynamic>($"project/delete/{id}", null);
            }
            catch (Exception ex) { Notify(ex); }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}