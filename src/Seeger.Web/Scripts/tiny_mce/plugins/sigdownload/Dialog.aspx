<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dialog.aspx.cs" Inherits="Seeger.Web.UI.Scripts.tiny_mce.plugins.sigdownload.Dialog" %>

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
        width: 235px;
    }
    .upload-panel
    {
        margin-top: 5px;
    }
    .path-info
    {
        padding-bottom: 3px;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="browser-panel">
            <fieldset class="select-file-panel">
                <legend><%= Localize("SigDownload.SelectFile") %></legend>
                <div class="path-info">
                    <%= Localize("SigDownload.CurrentPath")%>: <span id="current-path"></span>
                </div>
                <div id="file-browser"></div>
            </fieldset>
        </div>
        <div class="settings-panel">
            <fieldset class="properties-panel">
                <legend><%= Localize("SigDownload.Properties")%></legend>
                <table>
                    <tr>
                        <td class="column1"><label for="link-text"><%= Localize("SigDownload.LinkText")%></label></td>
                        <td>
                            <input id="link-text" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="column1"><label for="link-title"><%= Localize("SigDownload.LinkTitle") %></label></td>
                        <td>
                            <input id="link-title" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="column1"><label><%= Localize("SigDownload.OpenType") %></label></td>
                        <td>
                            <select id="link-target">
                                <option value=""><%= Localize("SigDownload.OpenType.Default") %></option>
                                <option value="_self"><%= Localize("SigDownload.OpenType.CurrentWindow") %></option>
                                <option value="_blank"><%= Localize("SigDownload.OpenType.NewWindow") %></option>
                            </select>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset class="upload-panel">
                <legend><%= Localize("SigDownload.Upload") %></legend>
                <div style="padding-left:55px">
                    <iframe id="uploadFrame" frameborder="0" src="<%= "/Admin/Shared/Uploader.aspx" %>" width="110" height="45" scrolling="no"></iframe>
                </div>
            </fieldset>
        </div>
        <div style="clear:both"></div>
    </div>

	<div class="mceActionPanel">
		<input type="button" id="insert" value='<%= Localize("Common.Insert") %>' />
		<input type="button" id="cancel" value='<%= Localize("Common.Cancel") %>' onclick="tinyMCEPopup.close();" />
    </div>

    <script type="text/javascript">

        var filePath = null;
        var filebrowser = null;
        var $currentPath = $("#current-path");

        $(function () {
            filebrowser = $("#file-browser").filebrowser({
                root: '/Files',
                serverHandler: '/Admin/Files/ListFiles.ashx',
                fileClick: function (fullPath, fileName) {
                    selectFile(fullPath, fileName);
                    return false;
                },
                directoryLoaded: function (dir) {
                    filePath = null;
                    $currentPath.html(dir);
                    $("#uploadFrame").attr("src", "/Admin/Shared/Uploader.aspx?dir=" + dir);
                }
            });

            $("#insert").click(function () {
                if (!filePath) {
                    alert('<%= Localize("SigDownload.PleaseSelectFile") %>');
                } else {
                    DownloadDialog.insert($("#link-text").val(), $("#link-title").val(), filePath, $("#link-target").val());
                }

                return false;
            });
        });

        function selectFile(fullPath, fileName) {
            filePath = fullPath;
            $("#link-text").val(fileName);
        }

        function uploader_onUploaded(dir, fileName) {
            var fullName = dir + "/" + fileName;
            filebrowser.addFile(fullName, true);
            selectFile(fullName, fileName);

            alert('<%= Localize("SigDownload.UploadSuccess") %>');
        }
    
    </script>
    </form>
</body>
</html>
