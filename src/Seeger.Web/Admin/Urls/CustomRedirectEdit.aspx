<%@ Page Title="{ CustomRedirect.Edit }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="CustomRedirectEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Urls.CustomRedirectEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="message info">
<%= Localize("CustomRedirect.HelpHint") %>
</div>

<table class="formtable">
    <tr>
        <th><%= Localize("CustomRedirect.RedirectMode")%></th>
        <td>
            <asp:DropDownList runat="server" ID="RedirectMode">
                <asp:ListItem Text="<%$ Resources: RedirectMode.Temporary %>" Value="Temporary" />
                <asp:ListItem Text="<%$ Resources: RedirectMode.Permanent %>" Value="Permanent" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th><label class="required"><%= Localize("CustomRedirect.From") %></label></th>
        <td>
            <asp:TextBox runat="server" ID="From" MaxLength="300" />
            <asp:RequiredFieldValidator runat="server" ID="FromRequiredValidator" ControlToValidate="From" ErrorMessage="*" />
        </td>
    </tr>
    <tr>
        <th><label class="required"><%= Localize("CustomRedirect.To") %></label></th>
        <td>
            <asp:TextBox runat="server" ID="To" MaxLength="300" />
            <asp:RequiredFieldValidator runat="server" ID="ToRequiredValidator" ControlToValidate="To" ErrorMessage="*" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:CheckBox runat="server" ID="MatchByRegex" Checked="true" Text="<%$ Resources: CustomRedirect.MatchByRegex %>" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:LinkButton runat="server" ID="SubmitButton" Text="<%$ Resources: Common.Save %>" CssClass="button primary" />
            <a href="CustomRedirectList.aspx" class="button secondary"><%= Localize("Common.Cancel") %></a>
        </td>
    </tr>
</table>

</asp:Content>
