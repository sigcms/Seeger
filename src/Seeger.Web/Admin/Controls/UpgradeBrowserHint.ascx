<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpgradeBrowserHint.ascx.cs" Inherits="Seeger.Web.UI.Admin.Controls.UpgradeBrowserHint" %>

<sig:MessagePanel runat="server" ID="Hint" CssClass="browser-hint" Visible="false">
<div>
    <asp:Literal runat="server" ID="HintMessage" />
</div>
<div class="browser-list" style="padding-top:5px;">
    <div class="browser-item">
        <a href="http://www.maxthon.cn/mx3/" target="_blank" title="<%= Localize("Dashboard.Maxthon3") %>">
            <img src="/Admin/Images/maxthon.png" alt="<%= Localize("Dashboard.Maxthon3") %>" />
        </a>
        <span>
            <a href="http://www.maxthon.cn/mx3/" target="_blank" title="<%= Localize("Dashboard.Maxthon3") %>">
                <%= Localize("Dashboard.Maxthon3")%>
            </a>
        </span>
    </div>
    <div class="browser-item">
        <a href="http://firefox.com.cn/" target="_blank" title="<%= Localize("Dashboard.Firefox") %>">
            <img src="/Admin/Images/firefox.png" alt="<%= Localize("Dashboard.Firefox") %>" />
        </a>
        <span>
            <a href="http://firefox.com.cn/" target="_blank" title="<%= Localize("Dashboard.Firefox") %>">
                <%= Localize("Dashboard.Firefox") %>
            </a>
        </span>
    </div>
    <div class="browser-item">
        <a href="http://www.google.com/chrome?hl=zh-cn" target="_blank" title="<%= Localize("Dashboard.GoogleChrome") %>">
            <img src="/Admin/Images/chrome.png" alt="<%= Localize("Dashboard.GoogleChrome") %>" />
        </a>
        <span>
            <a href="http://www.google.com/chrome?hl=zh-cn" target="_blank" title="<%= Localize("Dashboard.GoogleChrome") %>">
                <%= Localize("Dashboard.GoogleChrome")%>
            </a>
        </span>
    </div>
    <div class="browser-item">
        <a href="http://windows.microsoft.com/zh-CN/internet-explorer/products/ie-9/home" target="_blank" title="IE9">
            <img src="/Admin/Images/ie.png" alt="IE9" />
        </a>
        <span>
            <a href="http://windows.microsoft.com/zh-CN/internet-explorer/products/ie-9/home" target="_blank" title="IE9">
                IE9
            </a>
        </span>
    </div>
    <div style="clear:both"></div>
</div>
</sig:MessagePanel>