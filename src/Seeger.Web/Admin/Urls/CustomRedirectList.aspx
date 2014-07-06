<%@ Page Title="{ CustomRedirect.List }" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="CustomRedirectList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Urls.CustomRedirectList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="page-header">
    <h1>
        <sig:AdminPlaceHolder runat="server" PermissionGroup="CustomRedirect" Permission="Add">
            <a href="CustomRedirectEdit.aspx" class="btn btn-success"><i class="fa fa-2x fa-plus"></i></a>
        </sig:AdminPlaceHolder>
        <span><%= T("CustomRedirect.List") %></span>
    </h1>
</div>

<div class="ajax-grid">
    <div class="grid-panel"></div>
</div>

</asp:Content>
