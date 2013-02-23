<%@ Page Title="{ Menu.Analytics }" Language="C#" ValidateRequest="false" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="Analytics.aspx.cs" Inherits="Seeger.Plugins.Analytics.Admin.Analytics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<table class="formtable">
    <tr>
        <th><%= Localize("Mgnt.AnalyticsCode") %></th>
        <td>
            <asp:TextBox runat="server" ID="Code" TextMode="MultiLine" Width="400" Height="100" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:CheckBox runat="server" ID="EnableAnalyticsCode" Text="<%$ Resources: Mgnt.EnableAnalyticsCode %>" CssClass="aspnet-checkbox" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:LinkButton runat="server" ID="SaveButton" CssClass="button primary" Text="<%$ Resources: Common.Save %>" OnClick="SaveButton_Click" />
        </td>
    </tr>
</table>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterHolder" runat="server">
</asp:Content>
