<%@ Page Title="Manage items" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SliderItems.aspx.cs" Inherits="Seeger.Plugins.ImageSlider.Admin.SliderItems" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

    <div class="mgnt-toolbar">
        <% if (CanAdd) { %>
        <button type="button" onclick="location.href='SliderItemEdit.aspx?sliderId=<%= SliderId %>'"><%= T("Add") %></button>
        <% } %>
        <button type="button" onclick="location.href='Sliders.aspx'"><%= T("Back") %></button>
    </div>
    <div class="ajax-grid"></div>

</asp:Content>
