using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebRTC.Filters {
    public class SessionFilter : ActionFilterAttribute, IActionFilter, IResultFilter {

        public override void OnActionExecuting(ActionExecutingContext filtercontext) {
            
            if (HttpContext.Current.Session["AccountId"] == null) 
                filtercontext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Account" }, { "Action", "Login" } });

            base.OnActionExecuting(filtercontext);
        }
    }
}