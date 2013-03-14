<%@ Page Title="" Language="C#" EnableViewState="false" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs" Inherits="Seeger.Web.UI.Admin.Dashboard" %>
    
<%@ Register TagPrefix="uc" TagName="InvalidLicenseHint" Src="Licensing/InvalidLicenseHint.ascx" %>
<%@ Register TagPrefix="uc" TagName="CurrentDomainHint" Src="Licensing/CurrentDomainHint.ascx" %>
<%@ Register TagPrefix="uc" TagName="DomainBindingHint" Src="Licensing/DomainBindingHint.ascx" %>
<%@ Register TagPrefix="uc" TagName="UpdateCheck" Src="Controls/UpdateCheck.ascx" %>
<%@ Register TagPrefix="uc" TagName="SeegerNews" Src="Controls/SeegerNews.ascx" %>
<%@ Register TagPrefix="uc" TagName="UpgradeBrowserHint" Src="Controls/UpgradeBrowserHint.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <script type="text/javascript" src="Scripts/product-service.js"></script>
    <div class="dashboard-panel">
        <div class="greeting"><%= Greeting %></div>
        <div class="panel-body">
            <uc:InvalidLicenseHint runat="server" ID="InvalidLicenseHint" />
            <uc:CurrentDomainHint runat="server" ID="CurrentDomainHint" />
            <uc:DomainBindingHint runat="server" ID="DomainBindingHint" />
            <uc:UpgradeBrowserHint runat="server" ID="UpgradeBrowserHint" />
            <uc:UpdateCheck runat="server" ID="UpdateCheck" />
            <table class="quick-links">
                <tr>
                    <td><a href="<%= SeegerUrls.Homepage %>" target="_blank"><%= T("Seeger.Homepage") %></a></td>
                    <td><a href="<%= SeegerUrls.Purchase %>" target="_blank"><%= T("Seeger.Purchase") %></a></td>
                    <td><a href="<%= SeegerUrls.Suggest %>" target="_blank"><%= T("Seeger.Suggest") %></a></td>
                    <td><a href="<%= SeegerUrls.ReportBug %>" target="_blank"><%= T("Seeger.ReportBug") %></a></td>
                </tr>
                <tr>
                    <td><a href="<%= SeegerUrls.Help %>" target="_blank"><%= T("Seeger.Help") %></a></td>
                    <td><a href="<%= SeegerUrls.TechSupport %>" target="_blank"><%= T("Seeger.TechSupport") %></a></td>
                    <td><a href="<%= SeegerUrls.FAQ %>" target="_blank"><%= T("Seeger.FAQ") %></a></td>
                    <td><a href="<%= SeegerUrls.Contact %>" target="_blank"><%= T("Seeger.Contact") %></a></td>
                </tr>
            </table>
            <uc:SeegerNews runat="server" ID="SeegerNews" />
        </div>

    </div>
</asp:Content>
