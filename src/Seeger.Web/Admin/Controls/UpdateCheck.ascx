<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateCheck.ascx.cs" Inherits="Seeger.Web.UI.Admin.Controls.UpdateCheck" %>

<div id="new-version-hint" class="message info" style="display:none"></div>

<script type="text/javascript">

    jQuery(function () {
        var currentVersion = '<%= CurrentVersion %>';
        var culture = '<%= System.Globalization.CultureInfo.CurrentCulture.Name %>';

        var Messages = {
            NewVersionInfoFormat: '<%= T("Dashboard.NewVersionInfoFormat") %>',
            CheckNow: '<%= T("Dashboard.CheckNow") %>'
        };

        ProductService.checkUpdates(culture, function (result) {
            try {
                if (compareVersion(currentVersion, result.latestVersion) < 0) {
                    var html = Messages.NewVersionInfoFormat.replace("{VersionNumber}", result.latestVersion).replace("{PublishedDate}", result.publishedDate);
                    html += "<a href='" + result.url + "' target='_blank'>" + Messages.CheckNow + "</a>!";

                    $("#new-version-hint").html(html).show();
                }
            } catch (e) { }
        });

        function compareVersion(v1, v2) {
            var v1Parts = v1.split('.');
            var v2Parts = v2.split('.');

            for (var i = 0; i < 4; i++) {
                var part1 = parseInt(v1Parts[i], 10);
                var part2 = parseInt(v2Parts[i], 10);

                if (part1 < part2) {
                    return -1;
                } else if (part1 > part2) {
                    return 1;
                }
            }

            return 0;
        }
    });

</script>