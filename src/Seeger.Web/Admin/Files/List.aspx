<%@ Page Title="{ Menu.FileMgnt }" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Seeger.Web.UI.Admin.Files.List" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<asp:ScriptManagerProxy runat="server">
    <Services>
        <asp:ServiceReference Path="Services.asmx" />
    </Services>
</asp:ScriptManagerProxy>

<div class="mgnt-toolbar">
    <span class="file-current-path">
        <%= T("FileMgnt.CurrentPath") %>: <%= CurrentPath %>
    </span>
    <span class="file-actions">
        <asp:Button ID="UpButton" Enabled="false" runat="server" UseSubmitBehavior="false" Text="<%$ T: FileMgnt.GoUpperLevel %>" />
        <sig:AdminButton runat="server" OnClientClick="$('#create-folder-form').dialog('open');return false;" Text="<%$ T: FileMgnt.CreateFolder %>"
             Function="FileMgnt" Operation="AddFolder" />
        <sig:AdminButton runat="server" ID="UploadFileButton" Text="<%$ T: FileMgnt.UploadFile %>"
             Function="FileMgnt" Operation="UploadFile" />
        <button type="button" onclick="location.href=location.href;"><%= T("Common.Reload") %></button>
    </span>
</div>

<div class="ajax-grid">
    <div class="grid-panel"></div>
</div>

<table id="create-folder-form" class="formtable" style="display:none;">
    <tr>
        <th style="width:auto;"><label class="required"><%= T("FileMgnt.FolderName") %></label></th>
        <td>
            <input type="text" maxlength="50" id="folder-name" class="required" />
        </td>
    </tr>
    <tr>
        <th style="width:auto;"></th>
        <td>
            <button id="create-folder-button" class="button primary"><%= T("Common.Create") %></button>
            <button id="cancel-create-folder-button" class="button secondary"><%= T("Common.Cancel") %></button>
        </td>
    </tr>
</table>

<table id="rename-form" class="formtable" style="display:none">
    <tr>
        <th style="width:auto"><label class="required"><%= T("FileMgnt.NewName") %></label></th>
        <td>
            <input type="text" maxlength="50" id="new-name" class="required" />
            <span id="file-extension"></span>
        </td>
    </tr>
    <tr>
        <th style="width:auto"></th>
        <td>
            <button id="rename-button" class="button primary"><%= T("Common.Rename") %></button>
            <button id="cancel-rename-button" class="button secondary"><%= T("Common.Cancel") %></button>
        </td>
    </tr>
</table>

    <script type="text/javascript">
        var currentPath = '<%= CurrentPath %>';

        var ErrorMessages = {
            "folder-name": '<%= T("FileMgnt.FolderNameIsRequired") %>',
            "new-name": '<%= T("FileMgnt.NewNameIsRequired") %>'
        };
        
        $(function () {
            // Create Folder
            $("#create-folder-form").dialog({
                title: '<%= T("FileMgnt.CreateFolder") %>',
                autoOpen: false,
                modal: true,
                resizable: false
            });

            $("#create-folder-button").click(function () {
                if (Sig.Validator.validate("#create-folder-form") == false) {
                    return;
                }

                var $folderName = $("#folder-name");
                var foldername = $.trim($folderName.val());

                Seeger.Web.UI.Admin.Files.Services.CreateFolder(currentPath, foldername, function (result) {
                    $("#create-folder-form").dialog("close");

                    if (result.Success) {
                        Sig.Message.show(result.Message);

                        setTimeout(function () {
                            location.href = '?path=' + currentPath;
                        }, 1000);
                    } else {
                        Sig.Message.error(result.Message);

                        $folderName.val('');
                    }
                });
            });
            $("#cancel-create-folder-button").click(function () {
                $("#folder-name").val('');
                $('#create-folder-form').dialog('close');
            });

            // Rename
            $("#rename-form").dialog({
                title: '<%= T("Common.Rename") %>',
                autoOpen: false,
                modal: true,
                resizable: false,
                minWidth: 310
            });
            $("#cancel-rename-button").click(function () {
                $("#new-name").val('');
                $("#rename-form").dialog("close");
            });
        });

        function openRenameDialog(dataRow) {
            var $link = dataRow.find("td:first .icon-link");
            var isDirectory = $link.attr("isdirectory") == "1";
            var oldName = $.trim($link.text());
            var extension = "";

            if (!isDirectory) {
                extension = oldName.substr(oldName.lastIndexOf('.'));
                oldName = oldName.substr(0, oldName.length - extension.length);
            }

            $("#new-name").val(oldName);
            $("#file-extension").html(extension);

            $("#rename-button").unbind("click").click(function () {
                if (Sig.Validator.validate("#rename-form") == false) {
                    return;
                }

                var $newName = $("#new-name");
                var newName = $.trim($newName.val());

                if (newName == oldName) {
                    $("#rename-form").dialog("close");
                } else {
                    Seeger.Web.UI.Admin.Files.Services.Rename(isDirectory, currentPath, oldName + extension, newName + extension, function (result) {
                        $("#rename-form").dialog("close");

                        if (result.Success) {
                            $link.html(newName + extension);
                            Sig.Message.show(result.Message);
                        } else {
                            Sig.Message.error(result.Message);
                        }
                    });
                }
            });

            $("#rename-form").dialog("open");
        }
    </script>

</asp:Content>
