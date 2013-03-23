<%@ Page Title="{ User.List }" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Security.UserList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="mgnt-toolbar">
    <sig:AdminButton runat="server" OnClientClick="location.href='UserEdit.aspx';return false;" Text="<%$ T: User.Add %>" Function="UserMgnt" Operation="Add" />
</div>

<div class="ajax-grid">
    <div class="grid-panel"></div>
</div>

</asp:Content>
