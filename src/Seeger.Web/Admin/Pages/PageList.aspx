<%@ Page Title="{ Menu.PageMgnt }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master"
    AutoEventWireup="true" CodeBehind="PageList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Pages.PageList" %>

<%@ Register TagPrefix="uc" TagName="PropertyPanel" Src="Controls/PropertyPanel.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <link href="../../Scripts/telerik/themes/telerik.common.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/telerik/themes/telerik.default.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

    <asp:ScriptManagerProxy runat="server">
        <Services>
            <asp:ServiceReference Path="Services.asmx" />
        </Services>
    </asp:ScriptManagerProxy>

    <div class="mgnt-toolbar">
        <sig:AdminPlaceHolder runat="server" PermissionGroup="PageMgnt" Permission="Add">
            <button type="button" onclick="addChildPage(null);return false;"><%= T("Page.AddRootPage") %></button>
            <button id="add-child-button" type="button" onclick="addChildPage(selectedPage);return false;" disabled="disabled"><%= T("Page.AddChild") %></button>
        </sig:AdminPlaceHolder>
    </div>
    <div id="pagelist-container">
        <div id="no-page-hint" style="<%= GetNoPageHintPanelStyle() %>">
            <%= T("Page.NoPageNow") %>
            <sig:AdminPlaceHolder runat="server" PermissionGroup="PageMgnt" Permission="Add">
                , <a href='javascript:addChildPage(null);' title='<%= T("Page.ClickHereToAddPageRightNow") %>'><%= T("Page.AddPageRightNow") %></a>
            </sig:AdminPlaceHolder>
        </div>
        <div id="pagelist-treepanel">
            <div id="PageTree" class="t-widget t-treeview t-reset">
                <%= RenderPageTree() %>
            </div>
        </div>
        <uc:PropertyPanel runat="server" />
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="FooterHolder">
    <script type="text/javascript" src="../../Scripts/telerik/telerik.common.js"></script>
    <script type="text/javascript" src="../../Scripts/telerik/telerik.draganddrop.js"></script>
    <script type="text/javascript" src="../../Scripts/telerik/telerik.treeview.js"></script>
    <script type="text/javascript" src="../../Scripts/telerik/telerik.treeview.ext.js"></script>
    <script type="text/javascript" src="PageList.js"></script>
    <script type="text/javascript">
        settings.multilingual = <%= FrontendSettings.Multilingual.ToString().ToLower() %>;

        Messages.Processing = '<%= T("Message.Processing") %>';
        Messages.Loading = '<%= T("Message.Loading") %>';
        Messages.Success = '<%= T("Message.OperationSuccess") %>';
        Messages.None = '<%= T("Common.None") %>';
        Messages.DeleteSuccess = '<%= T("Message.DeleteSuccess") %>';
        Messages.DeletePageConfirm = '<%= T("Message.DeletePageConfirm") %>';
        Messages.DialogTitle.AddPage = '<%= T("Page.Add") %>';
        Messages.DialogTitle.EditPage = '<%= T("Page.Edit") %>';
    </script>
</asp:Content>
