<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dialog.aspx.cs" Inherits="Seeger.Web.UI.Scripts.tiny_mce.plugins.sigimage.Dialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<script type="text/javascript" src="../../tiny_mce_popup.js"></script>
    <script type="text/javascript" src="../../../../Scripts/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../../../Scripts/filebrowser/jquery.filebrowser.js"></script>
    <script type="text/javascript" src="script.js"></script>
    <link href="../../../../Scripts/filebrowser/jquery.filebrowser.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .browser-panel, .settings-panel
    {
        float: left;
    }
    .settings-panel
    {
        margin-left: 5px;
    }
    .settings-panel fieldset
    {
        width: 220px;
    }
    .preview-panel, .upload-panel
    {
        margin-top: 5px;
    }
    .path-info
    {
        padding-bottom: 3px;
    }
    #preview
    {
        border: #999 1px solid;
        width: 218px;
        height: 98px;
        overflow: auto;
    }
    #img-width, #img-height
    {
        text-align: center;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">

    <div>
        <div class="browser-panel">
            <fieldset class="select-file-panel">
                <legend><%= T("SigImage.SelectImage") %></legend>
                <div class="path-info">
                    <%= T("SigImage.CurrentPath") %>: <span id="current-path"></span>
                </div>
                <div id="file-browser"></div>
            </fieldset>
        </div>
        <div class="settings-panel">
            <fieldset class="properties-panel">
                <legend><%= T("SigImage.Properties") %></legend>
                <table>
                    <tr>
                        <td class="column1"><label for="img-title"><%= T("SigImage.Title") %>:</label></td>
                        <td>
                            <input id="img-title" type="text" maxlength="100" />
                        </td>
                    </tr>
                    <tr>
                        <td class="column1"><label for="img-width"><%= T("SigImage.Dimensions") %>:</label></td>
                        <td>
                            <input id="img-width" type="text" maxlength="4" size="4" />
                            x
                            <input id="img-height" type="text" maxlength="4" size="4" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset class="upload-panel">
                <legend><%= T("SigImage.Upload") %></legend>
                <div style="padding-left:55px">
                    <iframe id="uploadFrame" frameborder="0" src="/Admin/Shared/Uploader.aspx?imageOnly=1" width="110" height="45" scrolling="no"></iframe>
                </div>
            </fieldset>
            <fieldset class="preview-panel">
                <legend><%= T("SigImage.Preview") %></legend>
                <div id="preview"></div>
            </fieldset>
        </div>
        <div style="clear:both"></div>
    </div>

	<div class="mceActionPanel">
		<input type="button" id="insert" value='<%= T("Common.Insert") %>' />
		<input type="button" id="cancel" value='<%= T("Common.Cancel") %>' onclick="tinyMCEPopup.close();" />
    </div>

    <script type="text/javascript">

        var imageSrc = null;
        var filebrowser = null;
        var $currentPath = $("#current-path");

        $(function () {
            filebrowser = $("#file-browser").filebrowser({
                root: '/Files',
                serverHandler: '/Admin/Files/ListFiles.ashx?imageOnly=1',
                fileClick: function (fullFileName, fileName) {
                    selectFile(fullFileName);
                    return false;
                },
                directoryLoaded: function (dir) {
                    imageSrc = null;
                    $currentPath.html(dir);
                    $("#uploadFrame").attr("src", "/Admin/Shared/Uploader.aspx?imageOnly=1&dir=" + dir);
                }
            });

            $("#insert").click(function () {
                if (!imageSrc) {
                    alert('<%= T("SigImage.PleaseSelectImage") %>');
                } else {
                    var width = Math.floor(parseInt($.trim($("#img-width").val())));
                    var height = Math.floor(parseInt($.trim($("#img-height").val())));
                    ImageDialog.insert(imageSrc, $.trim($("#img-title").val()), width, height);
                }

                return false;
            });
        });

        function selectFile(src) {
            imageSrc = src;
            $("#preview").html("<img src='" + src + "' />");
        }

        function uploader_onUploaded(dir, fileName) {
            var fullName = dir + "/" + fileName;
            filebrowser.addFile(fullName, true);
            selectFile(fullName);

            alert('<%= T("SigImage.UploadSuccess") %>');
        }
    
    </script>
    </form>
</body>
</html>
