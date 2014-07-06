<%@ Page Title="{ Role.List }" EnableViewState="false" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="RoleList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Security.RoleList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="page-header">
    <h1>
        <sig:AdminPlaceHolder runat="server" PermissionGroup="RoleMgnt" Permission="Add">
            <a href="RoleEdit.aspx" class="btn btn-success" title="<%= T("Role.Add") %>"><i class="fa fa-2x fa-plus"></i></a>
        </sig:AdminPlaceHolder>
        <span><%= T("Role.List") %></span>      
    </h1>
</div>

<div class="ajax-grid">
    <div class="grid-panel"></div>
</div>

</asp:Content>
