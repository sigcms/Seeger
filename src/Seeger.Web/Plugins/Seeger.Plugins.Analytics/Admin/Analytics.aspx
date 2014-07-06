<%@ Page Title="{ Menu.Analytics }" Language="C#" ValidateRequest="false" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Analytics.aspx.cs" Inherits="Seeger.Plugins.Analytics.Admin.Analytics" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="page-header">
    <h1><%= T("Menu.Analytics") %></h1>
</div>

<table class="formtable">
    <tr>
        <th><%= T("Mgnt.AnalyticsCode") %></th>
        <td>
            <asp:TextBox runat="server" ID="Code" TextMode="MultiLine" Width="400" Height="100" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:CheckBox runat="server" ID="EnableAnalyticsCode" Text="<%$ T: Mgnt.EnableAnalyticsCode %>" CssClass="aspnet-checkbox" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:LinkButton runat="server" ID="SaveButton" CssClass="button primary" Text="<%$ T: Common.Save %>" OnClick="SaveButton_Click" />
        </td>
    </tr>
</table>

</asp:Content>
