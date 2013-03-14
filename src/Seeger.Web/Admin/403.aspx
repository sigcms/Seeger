<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="403.aspx.cs" Inherits="Seeger.Web.UI.Admin._403" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <style type="text/css">
    .message.access-denied
    {
        width: 400px;
        margin: auto;
        margin-top: 100px;
        text-align: center;
        padding: 30px;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="message error access-denied">
    <%= T("Message.AccessDenied") %>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterHolder" runat="server">
</asp:Content>
