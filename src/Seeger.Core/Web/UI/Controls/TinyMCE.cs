using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Globalization;
using Seeger.Config;

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
            string culture = CultureInfo.CurrentUICulture.Name;

            _settings = new NameValueCollection();
            _settings["script_url"] = "/Scripts/tiny_mce/tiny_mce.js";
            _settings["theme"] = "advanced";
            _settings["language"] = culture;
            _settings["plugins"] = "pagebreak,sigimage,sigdownload,contextmenu,paste,fullscreen,xhtmlxtras";
            _settings["theme_advanced_buttons1"] = "code,fullscreen,|,bold,italic,underline,strikethrough,sub,sup,forecolor,backcolor,|,link,unlink,sigimage,sigdownload,|,justifyleft,justifycenter,justifyright,justifyfull,fontselect,fontsizeselect,formatselect";
            _settings["theme_advanced_buttons2"] = String.Empty;
            _settings["theme_advanced_buttons3"] = String.Empty;
            _settings["theme_advanced_buttons4"] = String.Empty;
            _settings["theme_advanced_blockformats"] = "p,h1,h2,h3,h4,h5,h6";
            _settings["theme_advanced_toolbar_location"] = "top";
            _settings["theme_advanced_toolbar_align"] = "left";
            _settings["theme_advanced_statusbar_location"] = "bottom";
            _settings["theme_advanced_resizing"] = "true";

            var fontSetting = CmsConfiguration.Instance.TinyMceFonts.Find(culture);
            if (fontSetting != null)
            {
                _settings["theme_advanced_fonts"] = fontSetting.Fonts;
            }

            _settings["relative_urls"] = "false";
            _settings["convert_urls"] = "true";
            _settings["content_css"] = "/Scripts/tiny_mce/themes/advanced/skins/default/" + (culture == "en-US" ? "content.css" : "content." + culture + ".css");
            _settings["popup_css"] = "/Scripts/tiny_mce/themes/advanced/skins/default/" + (culture == "en-US" ? "dialog.css" : "dialog." + culture + ".css");
        }

        #region TinyMCE Init Configurations

        public string ScriptUrl
        {
            get
            {
                return _settings["script_url"] ?? String.Empty;
            }
            set
            {
                _settings["script_url"] = value;
                ViewState["ScriptUrl"] = value;
            }
        }

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

        public string ThemeAdvancedButtons1
        {
            get
            {
                return _settings["theme_advanced_buttons1"] ?? String.Empty;
            }
            set
            {
                _settings["theme_advanced_buttons1"] = value;
                ViewState["ThemeAdvancedButtons1"] = value;
            }
        }

        public string ThemeAdvancedButtons2
        {
            get
            {
                return _settings["theme_advanced_buttons2"] ?? String.Empty;
            }
            set
            {
                _settings["theme_advanced_buttons2"] = value;
                ViewState["ThemeAdvancedButtons2"] = value;
            }
        }

        public string ThemeAdvancedButtons3
        {
            get
            {
                return _settings["theme_advanced_buttons3"] ?? String.Empty;
            }
            set
            {
                _settings["theme_advanced_buttons3"] = value;
                ViewState["ThemeAdvancedButtons3"] = value;
            }
        }

        public string ThemeAdvancedButtons4
        {
            get
            {
                return _settings["theme_advanced_buttons4"] ?? String.Empty;
            }
            set
            {
                _settings["theme_advanced_buttons4"] = value;
                ViewState["ThemeAdvancedButtons4"] = value;
            }
        }

        public string ThemeAdvancedBlockFormats
        {
            get
            {
                return _settings["theme_advanced_blockformats"] ?? String.Empty;
            }
            set
            {
                _settings["theme_advanced_blockformats"] = value;
                ViewState["ThemeAdvancedBlockFormats"] = value;
            }
        }

        public VerticalAlign ThemeAdvancedToolbarLocation
        {
            get
            {
                return EnumHelper.Parse<VerticalAlign>(_settings["theme_advanced_toolbar_location"], true);
            }
            set
            {
                _settings["theme_advanced_toolbar_location"] = value.ToString().ToLower();
                ViewState["ThemeAdvancedToolbarLocation"] = value;
            }
        }

        public HorizontalAlign ThemeAdvancedToolbarAlign
        {
            get
            {
                return EnumHelper.Parse<HorizontalAlign>(_settings["theme_advanced_toolbar_align"], true);
            }
            set
            {
                _settings["theme_advanced_toolbar_align"] = value.ToString().ToLower();
                ViewState["ThemeAdvancedToolbarAlign"] = value;
            }
        }

        public VerticalAlign ThemeAdvancedStatusbarLocation
        {
            get
            {
                return EnumHelper.Parse<VerticalAlign>(_settings["theme_advanced_statusbar_location"], true);
            }
            set
            {
                _settings["theme_advanced_statusbar_location"] = value.ToString().ToLower();
                ViewState["ThemeAdvancedStatusbarLocation"] = value;
            }
        }

        public bool ThemeAdvancedResizing
        {
            get
            {
                return Convert.ToBoolean(_settings["theme_advanced_resizing"]);
            }
            set
            {
                _settings["theme_advanced_resizing"] = value.ToString().ToLower();
                ViewState["ThemeAdvancedResizing"] = value;
            }
        }

        public string ThemeAdvancedFonts
        {
            get
            {
                return _settings["theme_advanced_fonts"] ?? String.Empty;
            }
            set
            {
                _settings["theme_advanced_fonts"] = value;
                ViewState["ThemeAdvancedFonts"] = value;
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

        public string ContentCss
        {
            get
            {
                return _settings["content_css"] ?? String.Empty;
            }
            set
            {
                _settings["content_css"] = value;
                ViewState["ContentCss"] = value;
            }
        }

        public string PopupCss
        {
            get
            {
                return _settings["popup_css"] ?? String.Empty;
            }
            set
            {
                _settings["popup_css"] = value;
                ViewState["PopupCss"] = value;
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

            string jqueryTinyMceScriptUrl = this.JQueryTinyMCEScriptUrl;
            if (String.IsNullOrEmpty(jqueryTinyMceScriptUrl))
            {
                jqueryTinyMceScriptUrl = "/Scripts/tiny_mce/jquery.tinymce.js";
            }

            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "TinyMCE", jqueryTinyMceScriptUrl);

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
            writer.Write("<script type=\"text/javascript\">");
            writer.Write("jQuery(function() {");

            writer.Write("jQuery(\"#" + ClientID + "\").tinymce({");

            bool first = true;

            foreach (var key in _settings.AllKeys)
            {
                if (!first)
                {
                    writer.Write(",");
                }

                writer.Write(key);
                writer.Write(":");

                string value = _settings[key];
                if (value == "true" || value == "false")
                {
                    writer.Write(value);
                }
                else
                {
                    writer.Write(value.Quote("\""));
                }

                first = false;
            }

            writer.Write("});");

            writer.Write("});");
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