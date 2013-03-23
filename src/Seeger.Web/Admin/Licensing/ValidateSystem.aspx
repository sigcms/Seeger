<%@ Page Title="{ Licensing.ValidateSystem }" Language="C#" ValidateRequest="false" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.master" CodeBehind="ValidateSystem.aspx.cs" Inherits="Seeger.Web.UI.Admin.Licensing.ValidateSystem" %>

<asp:Content runat="server" ContentPlaceHolderID="MainHolder">
    <div class="activate-cms-form">
        <label><%= T("Licensing.PleaseEnterLicenseKey") %>:</label>
        <asp:TextBox runat="server" ID="LicenseKey" Width="350" Height="100" TextMode="MultiLine" />
        <label style="padding-top:15px"><%= T("Licensing.OrUploadLicenseFile") %></label>
        <asp:FileUpload runat="server" ID="LicenseUpload" />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="LicenseUpload"
             ValidationExpression="^.+\.licx$" ErrorMessage="<%$ T: Licensing.LicenseFileTypeNotValid %>" />
        <div style="padding-top:30px">
            <asp:LinkButton runat="server" ID="ActivateButton" CssClass="button primary" Text="<%$ T: Licensing.ActivateNow %>"
                 OnClick="ActivateButton_Click" />

            <a href="<%= SeegerUrls.Purchase %>" target="_blank" class="button secondary"><%= T("Licensing.PurchaseLicense") %></a>
        </div>
    </div>
</asp:Content>
