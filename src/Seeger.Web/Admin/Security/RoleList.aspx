<%@ Page Title="{ Role.List }" EnableViewState="false" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="RoleList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Security.RoleList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="mgnt-toolbar">
    <sig:AdminButton runat="server" OnClientClick="location.href='RoleEdit.aspx';return false;" Text="<%$ T: Role.Add %>"
         Function="RoleMgnt" Operation="Add" />
</div>

<div class="ajax-grid">
    <div class="grid-panel"></div>
</div>

</asp:Content>
