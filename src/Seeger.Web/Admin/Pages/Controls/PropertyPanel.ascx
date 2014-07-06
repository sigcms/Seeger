<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PropertyPanel.ascx.cs" Inherits="Seeger.Web.UI.Admin.Pages.Controls.PropertyPanel" %>

<script type="text/x-jquery-tmpl" id="pagePropertiesTemplate">
    <table class="pageinfo-table">
        <tr>
            <th><%= T("Page.Path") %></th>
            <td>${PagePath}</td>
        </tr>
        <tr>
            <th><%= T("Page.Layout") %></th>
            <td>
                <img src='${LayoutPreviewImage}' title='${Layout}' alt='${Layout}' />
            </td>
        </tr>
        <tr>
            <th><%= T("Page.Skin") %></th>
            <td id="skin-cell"></td>
        </tr>
        <tr>
            <th><%= T("Page.CreatedTime") %></th>
            <td>${CreatedTime}</td>
        </tr>
        <tr>
            <th><%= T("Page.ModifiedTime") %></th>
            <td>${LastModifiedTime}</td>
        </tr>
    </table>
</script>

<div class="panel panel-default" id="pagelist-infopanel">
    <div class="panel-heading">
        <div class="panel-title"><%= T("Page.CurrentPage") %></div>
    </div>
    <div class="panel-body">
        <div id="content-no-selection">
            <sig:MessagePanel runat="server" MessageType="Info">
                <%= T("Page.SelectPageHint") %>
            </sig:MessagePanel>
        </div>
        <div id="content-page-selected" style="display: none;">
            <div id="page-toolbar">
                <sig:AdminPlaceHolder runat="server" PermissionGroup="PageMgnt" Permission="Design">
                    <a href="#design" <%= Multilingual ? "content='#design-dropdown'" : "" %> class="button primary">
                        <%= T("Page.Design") %>
                    </a>
                </sig:AdminPlaceHolder>
                <a href="#view" <%= Multilingual ? "content='#view-dropdown'" : "" %> class="icon-link view"><%= T("Page.View") %><%= Multilingual ? "<span class='more'></span>" : "" %></a>
                <sig:AdminPlaceHolder runat="server" PermissionGroup="PageMgnt" Permission="Edit">
                    <a href="#edit" class="icon-link edit"><%= T("Common.Edit") %></a>
                </sig:AdminPlaceHolder>
                <sig:AdminPlaceHolder runat="server" PermissionGroup="PageMgnt" Permission="Add">
                    <a href="#addchild" class="icon-link add"><%= T("Page.AddChild") %></a>
                </sig:AdminPlaceHolder>
                <sig:AdminPlaceHolder runat="server" PermissionGroup="PageMgnt" Permission="SEOSetting">
                    <a href="#seo" <%= Multilingual ? "content='#seo-dropdown'" : "" %> class="icon-link seo">SEO<%= Multilingual ? "<span class='more'></span>" : "" %></a>
                </sig:AdminPlaceHolder>
                <sig:AdminPlaceHolder runat="server" PermissionGroup="PageMgnt" Permission="Delete">
                    <a href="#delete" id="delete-button" class="icon-link delete"><%= T("Common.Delete") %></a>
                </sig:AdminPlaceHolder>
            </div>

            <div id="page-properties"></div>
        </div>
    </div>
</div>

<%= RenderCultureDropdown("view-dropdown") %>
<%= RenderCultureDropdown("design-dropdown") %>
<%= RenderCultureDropdown("seo-dropdown") %>