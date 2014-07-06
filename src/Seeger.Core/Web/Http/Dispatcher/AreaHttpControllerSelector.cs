using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace Seeger.Web.Http.Dispatcher
{
    public class AreaHttpControllerSelector : IHttpControllerSelector
    {
        private const string AreaKey = "area";
        private const string ControllerKey = "controller";

        private readonly HttpConfiguration _configuration;
        private readonly Lazy<Dictionary<string, HttpControllerDescriptor>> _controllers;
        private readonly HashSet<string> _duplicates;

        public AreaHttpControllerSelector(HttpConfiguration config)
        {
            _configuration = config;
            _duplicates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _controllers = new Lazy<Dictionary<string, HttpControllerDescriptor>>(InitializeControllerDictionary);
        }

        private Dictionary<string, HttpControllerDescriptor> InitializeControllerDictionary()
        {
            var dictionary = new Dictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);

            IAssembliesResolver assembliesResolver = _configuration.Services.GetAssembliesResolver();
            IHttpControllerTypeResolver controllersResolver = _configuration.Services.GetHttpControllerTypeResolver();

            ICollection<Type> controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);

            foreach (Type t in controllerTypes)
            {
                var areaAttr = t.GetCustomAttributes(typeof(AreaAttribute), true).FirstOrDefault() as AreaAttribute;
                var areaName = areaAttr == null ? null : areaAttr.Name;
                var controllerName = t.Name.Remove(t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length);

                var key = GetControllerCacheKey(areaName, controllerName);

                // Check for duplicate keys.
                if (dictionary.Keys.Contains(key))
                {
                    _duplicates.Add(key);
                }
                else
                {
                    dictionary[key] = new HttpControllerDescriptor(_configuration, t.Name, t);
                }
            }

            // Remove any duplicates from the dictionary, because these create ambiguous matches. 
            foreach (string s in _duplicates)
            {
                dictionary.Remove(s);
            }
            return dictionary;
        }

        static T TryGetDictionaryValue<T>(IDictionary<string, object> dic, string key)
        {
            if (dic != null)
            {
                object result = null;
                if (dic.TryGetValue(key, out result))
                {
                    return (T)result;
                }
            }

            return default(T);
        }

        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            IHttpRouteData routeData = request.GetRouteData();
            if (routeData == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var controllerName = TryGetDictionaryValue<string>(routeData.Values, ControllerKey);
            if (controllerName == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var areaName = TryGetDictionaryValue<string>(routeData.Route.DataTokens, AreaKey);

            // Find a matching controller.
            var key = GetControllerCacheKey(areaName, controllerName);

            HttpControllerDescriptor controllerDescriptor;
            if (_controllers.Value.TryGetValue(key, out controllerDescriptor))
            {
                return controllerDescriptor;
            }
            else if (_duplicates.Contains(key))
            {
                throw new HttpResponseException(
                    request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Multiple controllers were found that match this request."));
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // If controller is put under an area, we use key format {area}.{controller}, otherwise we use {controller}
        static string GetControllerCacheKey(string areaName, string controllerName)
        {
            if (!String.IsNullOrEmpty(areaName))
            {
                return areaName + "." + controllerName;
            }

            return controllerName;
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return _controllers.Value;
        }
    }
}
