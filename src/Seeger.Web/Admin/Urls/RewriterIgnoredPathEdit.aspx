<%@ Page Title="{ RewriterIgnoredPath.Edit }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="RewriterIgnoredPathEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Urls.ReservedPathEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="message info">
<%= Localize("RewriterIgnoredPath.HelpHint") %>
</div>

<table class="formtable">
    <tr>
        <th><%= Localize("RewriterIgnoredPath.Name")%></th>
        <td>
            <asp:TextBox runat="server" ID="Name" Text="<%$ Resources: Common.Unnamed %>" MaxLength="50" />
        </td>
    </tr>
    <tr>
        <th><label class="required"><%= Localize("RewriterIgnoredPath.Path")%></label></th>
        <td>
            <asp:TextBox runat="server" ID="Path" MaxLength="300" />
            <asp:RequiredFieldValidator runat="server" ID="PathRequiredValidator" ControlToValidate="Path" ErrorMessage="*" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:CheckBox runat="server" ID="MatchByRegex" Text="<%$ Resources: RewriterIgnoredPath.MatchByRegex %>" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:LinkButton runat="server" ID="SubmitButton" Text="<%$ Resources: Common.Save %>" CssClass="button primary" />

            <a href="RewriterIgnoredPathList.aspx" class="button secondary"><%= Localize("Common.Cancel") %></a>
        </td>
    </tr>
</table>

</asp:Content>
