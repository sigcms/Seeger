<%@ Page Title="{ Globalization.FrontendLanguageList }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="FrontendLangList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Settings.FrontendLangList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="mgnt-toolbar">
    <button type="button" onclick="location.href='FrontendLangEdit.aspx'"><%= T("Common.Add") %></button>
</div>

<div class="ajax-grid">
    <div class="grid-panel"></div>
</div>

</asp:Content>
