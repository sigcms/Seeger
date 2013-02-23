using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Web.UI
{
    public class SeegerUrls
    {
        private static readonly string _baseUrl = "http://www.sigcms.com/";

        public static string Homepage
        {
            get { return _baseUrl; }
        }

        public static string Purchase
        {
            get { return _baseUrl + "purchase"; }
        }

        public static string Help
        {
            get { return _baseUrl + "help"; }
        }

        public static string Contact
        {
            get { return _baseUrl + "contact"; }
        }

        public static string TechSupport
        {
            get { return _baseUrl + "contact"; }
        }

        public static string FAQ
        {
            get { return _baseUrl + "faq"; }
        }

        public static string Suggest
        {
            get { return _baseUrl + "contact"; }
        }

        public static string ReportBug
        {
            get { return _baseUrl + "contact"; }
        }

        public static string About
        {
            get { return _baseUrl + "about"; }
        }
    }
}