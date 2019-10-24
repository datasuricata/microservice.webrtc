using Swashbuckle.Application;
using System;
using System.Linq;
using System.Web.Http;
using WebActivatorEx;
using WebRTC;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebRTC {
    public class SwaggerConfig {
        public static void Register() {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c => {
                    c.SingleApiVersion("v1", "Microservice Web RTC");

                    c.IgnoreObsoleteActions();
                    c.IncludeXmlComments($@"{AppDomain.CurrentDomain.BaseDirectory}\bin\Swagger.xml");
                    c.IgnoreObsoleteProperties();

                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                })
                .EnableSwaggerUi(c => {
                    c.DocumentTitle("WebAPI RTC - Documentação");
                    c.DocExpansion(DocExpansion.List);
                });
        }
    }
}