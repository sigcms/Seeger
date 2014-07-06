<%@ Page Title="Sliders" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="Sliders.aspx.cs" Inherits="Seeger.Plugins.ImageSlider.Admin.Sliders" %>

<asp:Content runat="server" ContentPlaceHolderID="MainHolder">

    <div class="page-header">
        <h1><%= T("Sliders") %></h1>
    </div>

    <div class="ajax-grid"></div>

</asp:Content>