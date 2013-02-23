<%@ Page Title="{ Licensing.ValidateSystem }" Language="C#" ValidateRequest="false" AutoEventWireup="true" MasterPageFile="~/Admin/Shared/Management.Master" CodeBehind="ValidateSystem.aspx.cs" Inherits="Seeger.Web.UI.Admin.Licensing.ValidateSystem" %>

<asp:Content runat="server" ContentPlaceHolderID="MainHolder">
    <div class="activate-cms-form">
        <label><%= Localize("Licensing.PleaseEnterLicenseKey") %>:</label>
        <asp:TextBox runat="server" ID="LicenseKey" Width="350" Height="100" TextMode="MultiLine" />
        <label style="padding-top:15px"><%= Localize("Licensing.OrUploadLicenseFile") %></label>
        <asp:FileUpload runat="server" ID="LicenseUpload" />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="LicenseUpload"
             ValidationExpression="^.+\.licx$" ErrorMessage="<%$ Resources: Licensing.LicenseFileTypeNotValid %>" />
        <div style="padding-top:30px">
            <asp:LinkButton runat="server" ID="ActivateButton" CssClass="button primary" Text="<%$ Resources: Licensing.ActivateNow %>"
                 OnClick="ActivateButton_Click" />

            <a href="<%= SeegerUrls.Purchase %>" target="_blank" class="button secondary"><%= Localize("Licensing.PurchaseLicense") %></a>
        </div>
    </div>
</asp:Content>
