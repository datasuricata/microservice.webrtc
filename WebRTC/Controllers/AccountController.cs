using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebRTC.Controllers.Base;
using WebRTC.Helpers;
using WebRTC.Models;

namespace WebRTC.Controllers {
    public class AccountController : CoreController {
        public ActionResult Login() => View();

        [HttpPost]
        public async Task<ActionResult> Login(string email, string password) {
            try {

                var request = await Post<AccountResponse>("user/auth", 
                    new AccountRequest { Email = email, Password = password.Encrypt() });

                #region - session -

                var endpoint = ConfigurationManager.AppSettings["ApiEndpoint"];
                var version = ConfigurationManager.AppSettings["webpages:Version"];

                Session.Clear();
                Session.Timeout = 40;
                Session["Swagger"] = $"{endpoint}/swagger/ui/index";
                Session["Version"] = version;
                Session["Endpoint"] = endpoint;
                Session["AccountId"] = request.Id;
                Session["Project"] = request.Project;
                Session["Email"] = request.Email;
                Session["Role"] = request.Role.ToString();

                #endregion

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex) {
                Notify(ex);
                return RedirectToAction(nameof(Login));
            }
        }
    }
}