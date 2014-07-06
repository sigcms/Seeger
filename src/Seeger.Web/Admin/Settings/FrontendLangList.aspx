<%@ Page Title="{ Globalization.FrontendLanguageList }" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="FrontendLangList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Settings.FrontendLangList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="page-header">
    <h1>
        <a href="FrontendLangEdit.aspx" class="btn btn-success" title="<%= T("Common.Add") %>"><i class="fa fa-2x fa-plus"></i></a>
        <span><%= T("Globalization.FrontendLanguageList") %></span>
    </h1>
</div>

<div class="ajax-grid">
    <div class="grid-panel"></div>
</div>

</asp:Content>
