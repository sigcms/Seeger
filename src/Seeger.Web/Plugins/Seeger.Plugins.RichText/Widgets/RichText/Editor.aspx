<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Editor.aspx.cs" Inherits="Seeger.Plugins.RichText.Widgets.RichText.Editor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <sig:ScriptReference runat="server" Path="/Scripts/jquery/jquery.min.js" />
    <sig:ScriptReference runat="server" Path="/Scripts/jquery/jquery-ui.min.js" />
    <sig:ScriptReference runat="server" Path="/Scripts/sig.common.js" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" EnablePageMethods="true" />
    <div class="mgnt" style="padding:0">
        <div class="existing-contents-panel" style="display:none;padding-left:7px">
            <select class="existing-contents-dropdown">
                <option value="">-- <%= T("Select from existing contents") %> --</option>
            </select>
            <button type="button" class="btn-add-existing" data-add-mode="copy"><%= T("Copy this content") %></button>
            <button type="button" class="btn-add-existing" data-add-mode="shared"><%= T("Use this content") %></button>
        </div>
        <table class="formtable">
            <tr>
                <td>
                    <input type="text" id="ContentTitle" placeholder="<%= T("Enter title") %>" style="width:745px" />
                </td>
            </tr>
            <tr>
                <td>
                    <sig:TinyMCE runat="server" ID="ContentBody" Height="350" UseStaticID="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <a href="javascript:Editor.submit();" class="button primary"><%= T("Common.OK") %></a>
                    <a href="javascript:editorContext.cancel();" class="button secondary"><%= T("Common.Cancel") %></a>
                </td>
            </tr>
        </table>
    </div>
    
    <script type="text/javascript">
        window.aspNetAuth = '<%= Request.GetAuthCookieValue() %>';
    </script>
    <script type="text/javascript" src="/Scripts/sig.core.js"></script>
    <script type="text/javascript" src="/Scripts/Resources.ashx"></script>
    <script type="text/javascript" src="Scripts/editor.js"></script>

    </form>
</body>
</html>
