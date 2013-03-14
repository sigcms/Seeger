<%@ Page Title="{ Seeger.Name }" Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Seeger.Web.UI.Admin.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<body>
    <form id="form1" runat="server">
    <div class="frame-container">
        <div id="frame-header">
            <a class="frame-logo" href="/" target="_blank"></a>
            <div class="frame-user-links">
                <span>
                    <a href="/" target="_blank"><%= T("Common.Homepage") %></a>&nbsp;
                    |
                    <a href="My/Profile.aspx" target="content-iframe"><%= T("User.MyProfile") %></a>&nbsp;
                    <sig:AdminPlaceHolder runat="server" RequireSuperAdmin="true">
                    |
                    <a href="Licensing/LicenseInfo.aspx" target="content-iframe"><%= T("Licensing.LicenseInfo") %></a>&nbsp;
                    </sig:AdminPlaceHolder>
                    |
                    <a href="Logout.aspx" title="<%= T("Security.Logout") %>" onclick='return confirm(&quot;<%= T("Message.LogoutConfirm") %>&quot;)'><%= T("Security.Logout") %></a>
                </span>
            </div>
        </div>
        <div id="frame-main">
            <div id="frame-menubar" class="frame-menubar">
                <ul class="shortcut-menu">
                    <li><a target="content-iframe" href="Dashboard.aspx"><%= T("Common.Dashboard") %></a></li>
                </ul>
                <%= RenderAdminMenu() %>
                <%= RenderModuleMenus() %>
            </div>

            <div id="frame-main-content">
                <iframe id="content-iframe" name="content-iframe" frameborder="0" src="Dashboard.aspx">
                </iframe>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="../Scripts/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery/jquery-ui.min.js"></script>
    <script type="text/javascript" src="Scripts/frame.js"></script>
    </form>
</body>
</html>
