using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Globalization;
using Seeger.Config;
using Newtonsoft.Json;

namespace Seeger.Web.UI
{
    public class TinyMCE : WebControl, IPostBackDataHandler
    {
        private NameValueCollection _settings;

        public TinyMCE()
        {
            AddDefaultSettings();
        }

        private void AddDefaultSettings()
        {
            _settings = new NameValueCollection();

            _settings["menubar"] = "false";
            _settings["plugins"] = "fullscreen code link paste textcolor sigimage sigdownload inserthtml";
            _settings["toolbar"] = "code fullscreen | styleselect | alignleft aligncenter alignright alignjustify | bold italic forecolor backcolor | bullist numlist | link inserthtml sigimage sigdownload";

            _settings["relative_urls"] = "false";
            _settings["convert_urls"] = "true";

            var adminSession = AdminSession.Current;
            if (adminSession != null)
            {
                AddCultureSensitiveDefaultSettings(adminSession.UICulture.Name);
            }
            else
            {
                AddCultureSensitiveDefaultSettings(CultureInfo.CurrentUICulture.Name);
            }
        }

        private void AddCultureSensitiveDefaultSettings(string culture)
        {
            _settings["language"] = culture;
        }

        #region TinyMCE Init Configurations

        public string Language
        {
            get
            {
                return _settings["language"] ?? String.Empty;
            }
            set
            {
                _settings["language"] = value;
                ViewState["Language"] = value;
            }
        }

        public string Plugins
        {
            get
            {
                return _settings["plugins"] ?? String.Empty;
            }
            set
            {
                _settings["plugins"] = value;
                ViewState["Plugins"] = value;
            }
        }

        public bool ConvertUrls
        {
            get
            {
                return Convert.ToBoolean(_settings["convert_urls"]);
            }
            set
            {
                _settings["convert_urls"] = value.ToString().ToLower();
                ViewState["ConvertUrls"] = value;
            }
        }

        public bool RelativeUrls
        {
            get
            {
                return Convert.ToBoolean(_settings["relative_urls"]);
            }
            set
            {
                _settings["relative_urls"] = value.ToString().ToLower();
                ViewState["RelativeUrls"] = value;
            }
        }

        #endregion

        public string Text
        {
            get { return ViewState["Text"] as String ?? String.Empty; }
            set { ViewState["Text"] = value; }
        }

        public string JQueryTinyMCEScriptUrl
        {
            get { return ViewState["JQueryTinyMCEScriptUrl"] as String ?? String.Empty; }
            set { ViewState["JQueryTinyMCEScriptUrl"] = value; }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Textarea;
            }
        }

        #region Render

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.ClientScript.RegisterClientScriptInclude("TinyMCE", "/Scripts/tinymce/tinymce.min.js");

            // 'name' attribute is required, otherwise the IPostBackDataHandler.LoadPostData() won't be invoked
            if (Attributes["name"] == null)
            {
                Attributes["name"] = UniqueID;
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write(Text);
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);

            RenderScripts(writer);
        }

        private void RenderScripts(HtmlTextWriter writer)
        {
            var selector = "#" + ClientID;
            var settings = new Dictionary<string, string>();

            settings.Add("selector", selector);

            foreach (var key in _settings.AllKeys)
            {
                settings.Add(key, _settings[key]);
            }

            if (settings.ContainsKey("language"))
            {
                settings["language"] = settings["language"].Replace('-', '_');
            }

            var json = JsonConvert.SerializeObject(settings);

            writer.Write("<script>");
            writer.Write("jQuery(function () { tinymce.init(" + json + "); });");
            writer.Write("</script>");
        }

        #endregion

        #region IPostBackDataHandler Memebers

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string value = postCollection[postDataKey];
            if (value != Text)
            {
                Text = value;
                return true;
            }

            return false;
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
        }

        #endregion
    }
}