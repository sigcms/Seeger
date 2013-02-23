<%@ Page Title="{ Licensing.LicenseInfo }" ValidateRequest="false" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true"
    CodeBehind="LicenseInfo.aspx.cs" Inherits="Seeger.Web.UI.Admin.Licensing.LicenseInfo" %>

<%@ Register TagPrefix="uc" TagName="InvalidLicenseHint" Src="InvalidLicenseHint.ascx" %>
<%@ Register TagPrefix="uc" TagName="CurrentDomainHint" Src="CurrentDomainHint.ascx" %>
<%@ Register TagPrefix="uc" TagName="DomainBindingHint" Src="DomainBindingHint.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

    <uc:InvalidLicenseHint runat="server" ID="InvalidLicenseHint" />
    <uc:CurrentDomainHint runat="server" ID="CurrentDomainHint" />
    <uc:DomainBindingHint runat="server" ID="DomainBindingHint" />

    <asp:PlaceHolder runat="server" ID="LicenseInfoHolder">
        <div class="license-info">
            <table class="datatable vertical-header">
                <tr>
                    <th style="width:150px"><%= Localize("Licensing.Id") %></th>
                    <td>
                        <%= License.Id.ToString() %>
                    </td>
                </tr>
                <tr>
                    <th><%= Localize("Licensing.UserName") %></th>
                    <td>
                        <%= License.UserName %>
                    </td>
               </tr>
               <tr>
                    <th><%= Localize("Licensing.FittingCmsVersion") %></th>
                    <td>
                        <%= Localize("Seeger.ShortName") + " " + License.CmsVersion.ToString(2) + " " + Localize("Licensing.Edition_" + License.CmsEdition.Name) %>
                    </td>
                </tr>
                <tr>
                    <th><%= Localize("Licensing.IsTrial") %></th>
                    <td>
                        <%= License.IsTrial ? Localize("Common.Yes") : Localize("Common.No") %>
                    </td>
                </tr>
                <tr>
                    <th>
                        <%= Localize("Licensing.Multilingual") %>
                    </th>
                    <td>
                        <%= License.IsFeatureAvailable(Seeger.Licensing.Features.Multilingual) ? Localize("Common.Support") : Localize("Common.NotSupport") %>
                    </td>
                </tr>
                <tr>
                    <th><%= Localize("Licensing.SupportedDomains") %></th>
                    <td>
                        <%= String.Join(", ", License.SupportedDomains) %>
                    </td>
                </tr>
                <tr>
                    <th><%= Localize("Licensing.ExpirationDate") %></th>
                    <td>
                        <%= License.NeverExpire ? Localize("Licensing.NeverExpire") : License.ExpirationDate.ToString(System.Globalization.CultureInfo.CurrentCulture) %>
                    </td>
                </tr>
            </table>

            <div style="padding-top:8px;">
                <a href="ValidateSystem.aspx" class="button primary"><%= Localize("Licensing.ChangeLicense") %></a>
                <a href="<%= SeegerUrls.Purchase %>" target="_blank" class="button secondary"><%= Localize("Licensing.PurchaseLicense") %></a>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterHolder" runat="server">
</asp:Content>
