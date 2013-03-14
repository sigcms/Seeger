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
                    <th style="width:150px"><%= T("Licensing.Id") %></th>
                    <td>
                        <%= License.Id.ToString() %>
                    </td>
                </tr>
                <tr>
                    <th><%= T("Licensing.UserName") %></th>
                    <td>
                        <%= License.UserName %>
                    </td>
               </tr>
               <tr>
                    <th><%= T("Licensing.FittingCmsVersion") %></th>
                    <td>
                        <%= T("Seeger.ShortName") + " " + License.CmsVersion.ToString(2) + " " + T("Licensing.Edition_" + License.CmsEdition.Name) %>
                    </td>
                </tr>
                <tr>
                    <th><%= T("Licensing.IsTrial") %></th>
                    <td>
                        <%= License.IsTrial ? T("Common.Yes") : T("Common.No") %>
                    </td>
                </tr>
                <tr>
                    <th>
                        <%= T("Licensing.Multilingual") %>
                    </th>
                    <td>
                        <%= License.IsFeatureAvailable(Seeger.Licensing.Features.Multilingual) ? T("Common.Support") : T("Common.NotSupport") %>
                    </td>
                </tr>
                <tr>
                    <th><%= T("Licensing.SupportedDomains") %></th>
                    <td>
                        <%= String.Join(", ", License.SupportedDomains) %>
                    </td>
                </tr>
                <tr>
                    <th><%= T("Licensing.ExpirationDate") %></th>
                    <td>
                        <%= License.NeverExpire ? T("Licensing.NeverExpire") : License.ExpirationDate.ToString(System.Globalization.CultureInfo.CurrentCulture) %>
                    </td>
                </tr>
            </table>

            <div style="padding-top:8px;">
                <a href="ValidateSystem.aspx" class="button primary"><%= T("Licensing.ChangeLicense") %></a>
                <a href="<%= SeegerUrls.Purchase %>" target="_blank" class="button secondary"><%= T("Licensing.PurchaseLicense") %></a>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterHolder" runat="server">
</asp:Content>
