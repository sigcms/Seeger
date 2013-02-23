<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Uploader.aspx.cs" Inherits="Seeger.Web.UI.Admin.Shared.Uploader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <sig:ScriptReference runat="server" Path="/Scripts/jquery/jquery.min.js" />
    <style type="text/css">
    body
    {
        padding: 0;
    }
    .file-upload
    {
        position: absolute;
        opacity: 0;
        filter: alpha(opacity=0);
    }
    #container 
    {
        overflow: hidden;
    }
    </style>
    <script type="text/javascript">
        function onUploaded(dir, fileName) {
            if (window.parent && window.parent.uploader_onUploaded) {
                window.parent.uploader_onUploaded(dir, fileName);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <button id="fake" type="button"><%= Localize("Common.Upload") %></button>
        <asp:FileUpload runat="server" ID="FileUpload" CssClass="file-upload" />

        <script type="text/javascript">
            var $uploader = $("#<%= FileUpload.ClientID %>");
            var uploaderInputWidth = 155;

            // Fake upload button
            var offset = $("#fake").offset();
            $uploader.css({
                left: offset.left - uploaderInputWidth,
                top: offset.top
            });

            // Upload
            var imageOnly = <%= ImageOnly.ToString().ToLower() %>;

            var regex = null;
            if (imageOnly) {
                regex = new RegExp('<%= ImageFileNamePattern %>', 'i');
            }

            $uploader.change(function () {
                var fileName = this.value;
                if (regex != null && !regex.test(fileName)) {
                    alert('<%= Localize("FileMgnt.FileTypeNotSupported") %>');
                } else {
                    $("#fake").html('<%= Localize("Common.Uploading") %>...');
                    $("#<%= form1.ClientID %>").submit();
                }
            });

            function getExtension(fileName) {
                var index = fileName.lastIndexOf('\\');
                if (index > 0) {
                    fileName = fileName.sustr(index + 1);
                }

                index = fileName.lastIndexOf('.');
                
                return fileName.substr(index);
            }

        </script>
    </div>
    </form>
</body>
</html>
