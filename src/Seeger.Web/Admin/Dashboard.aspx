<%@ Page Title="" Language="C#" EnableViewState="false" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs" Inherits="Seeger.Web.UI.Admin.Dashboard" %>
    
<%@ Register TagPrefix="uc" TagName="UpgradeBrowserHint" Src="Controls/UpgradeBrowserHint.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <script type="text/javascript" src="Scripts/product-service.js"></script>
    <div class="dashboard-panel">
        <div class="greeting"><%= Greeting %></div>
        <div class="panel-body">
            <uc:UpgradeBrowserHint runat="server" ID="UpgradeBrowserHint" />
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
        </div>

    </div>

    <div>
        <input type="text" id="text1" data-folder="/Html5" data-autorename="true" />
    </div>

    <button type="button" id="btn-upload">Upload</button>

    <script>
        $(function () {
            sig.ui.UploadZone.init('#text1');
        });
    </script>

</asp:Content>
