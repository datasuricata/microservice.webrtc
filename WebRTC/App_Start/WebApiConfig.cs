using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using Unity;
using WebRTC.DI;

namespace WebRTC {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            
            var container = new UnityContainer();
            DependencyResolver.Resolve(container);
            config.DependencyResolver = new UnityResolver(container);


            var format = config.Formatters;
            format.Remove(format.XmlFormatter);

            var json = format.JsonFormatter.SerializerSettings;
            json.Formatting = Formatting.Indented;
            json.ContractResolver = new CamelCasePropertyNamesContractResolver();

            format.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            format.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
