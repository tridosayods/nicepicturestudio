﻿using System.Web.Mvc;
using System.Web.Routing;

namespace NicePictureStudio
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CustomerRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Customers", action = "DetailsCustomerFromService", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "CRMRoute",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "CRMTemplates", action = "CRMApprisal", id = UrlParameter.Optional }
           );

        }
    }
}