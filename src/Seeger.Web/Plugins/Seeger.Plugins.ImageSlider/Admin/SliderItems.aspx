<%@ Page Title="Slider items" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SliderItems.aspx.cs" Inherits="Seeger.Plugins.ImageSlider.Admin.SliderItems" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

    <div class="page-header">
        <h1>
            <span class="pull-right">
                <a href="Sliders.aspx" class="btn btn-default"><i class="fa fa-chevron-left"></i> <%= T("Back") %></a>
            </span>
            <% if (CanAdd) { %>
                <a href="SliderItemEdit.aspx?sliderId=<%= SliderId %>" class="btn btn-success" title="<%= T("Add") %>"><i class="fa fa-2x fa-plus"></i></a>
            <% } %>
            <span><%= T("Slider items") %></span>
        </h1>
    </div>

    <div class="ajax-grid"></div>

</asp:Content>
