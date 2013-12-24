<%@ Page Title="{ CustomRedirect.Edit }" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="CustomRedirectEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Urls.CustomRedirectEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<table class="formtable">
    <tr>
        <th><%= T("CustomRedirect.RedirectMode")%></th>
        <td>
            <asp:DropDownList runat="server" ID="RedirectMode">
                <asp:ListItem Text="<%$ T: RedirectMode.Temporary %>" Value="Temporary" />
                <asp:ListItem Text="<%$ T: RedirectMode.Permanent %>" Value="Permanent" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th><label class="required"><%= T("CustomRedirect.From") %></label></th>
        <td>
            <asp:TextBox runat="server" ID="From" MaxLength="300" />
            <asp:RequiredFieldValidator runat="server" ID="FromRequiredValidator" ControlToValidate="From" ErrorMessage="*" />
        </td>
    </tr>
    <tr>
        <th><label class="required"><%= T("CustomRedirect.To") %></label></th>
        <td>
            <asp:TextBox runat="server" ID="To" MaxLength="300" />
            <asp:RequiredFieldValidator runat="server" ID="ToRequiredValidator" ControlToValidate="To" ErrorMessage="*" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:CheckBox runat="server" ID="MatchByRegex" Text="<%$ T: CustomRedirect.MatchByRegex %>" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:CheckBox runat="server" ID="IsEnabled" Checked="true" Text="<%$ T: CustomRedirect.IsEnabled %>" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:LinkButton runat="server" ID="SubmitButton" Text="<%$ T: Common.Save %>" CssClass="button primary" OnClick="SubmitButton_Click" />
            <a href="CustomRedirectList.aspx" class="button secondary"><%= T("Common.Cancel") %></a>
        </td>
    </tr>
</table>

</asp:Content>
