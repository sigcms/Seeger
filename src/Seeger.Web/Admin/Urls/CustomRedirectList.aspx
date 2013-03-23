<%@ Page Title="{ CustomRedirect.List }" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="CustomRedirectList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Urls.CustomRedirectList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="mgnt-toolbar">
    <sig:AdminButton runat="server" ID="AddButton" UseSubmitBehavior="false" 
         OnClientClick="location.href='CustomRedirectEdit.aspx';return false;" 
         Text="<%$ T: Common.Add %>"
         Function="CustomRedirect"
         Operation="Add" />
</div>

<div class="ajax-grid">
    <div class="grid-panel"></div>
</div>

</asp:Content>
