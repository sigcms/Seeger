var DownloadDialog = {
    init: function (ed) {
        tinyMCEPopup.resizeToInnerSize();
    },

    insert: function (linkText, linkTitle, href, target) {
        var ed = tinyMCEPopup.editor, dom = ed.dom;

        tinyMCEPopup.execCommand('mceInsertContent', false, createLinkHtml(linkText, linkTitle, href, target));

        tinyMCEPopup.close();

        function createLinkHtml(linkText, linkTitle, href, target) {
            if (!linkText) {
                linkText = href;
            }
            if (!linkTitle) {
                linkTitle = linkText;
            }

            var html = "<a href='" + href + "' title='" + linkTitle + "'";
            if (target) {
                html += " target='" + target + "'";
            }

            html += ">" + linkText + "</a>";

            return html;
        }
    }
};

tinyMCEPopup.onInit.add(DownloadDialog.init, DownloadDialog);
