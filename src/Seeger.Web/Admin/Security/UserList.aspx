<%@ Page Title="{ User.List }" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Security.UserList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="page-header">
    <h1>
        <sig:AdminPlaceHolder runat="server" PermissionGroup="UserMgnt" Permission="Add">
            <a href="UserEdit.aspx" title="<%= T("User.Add") %>" class="btn btn-success"><i class="fa fa-2x fa-plus"></i></a>
        </sig:AdminPlaceHolder>
        <span><%= T("User.List") %></span>
    </h1>
</div>

<div class="ajax-grid">
    <div class="grid-panel"></div>
</div>

</asp:Content>
