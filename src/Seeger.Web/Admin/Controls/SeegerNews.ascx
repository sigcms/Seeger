<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SeegerNews.ascx.cs" Inherits="Seeger.Web.UI.Admin.Controls.SeegerNews" %>

<div id="seeger-news" style="display:none"></div>

<script type="text/javascript">
    jQuery(function () {
        var culture = '<%= System.Globalization.CultureInfo.CurrentCulture.Name %>';
        var Messages = {
            SeegerNews: '<%= Localize("Dashboard.SeegerNews") %>'
        };

        ProductService.getSeegerNews(culture, function (newsList) {
            if (newsList.length > 0) {
                var html = "<ul>";

                for (var i = 0, len = newsList.length; i < len; ++i) {
                    html += "<li><a href='" + newsList[i].url + "' title='" + newsList[i].title + "' target='_blank'>" + newsList[i].title + "</a></li>";
                }

                html += "</ul>";

                $("#seeger-news").html(html).show();
            }
        });
    });
</script>