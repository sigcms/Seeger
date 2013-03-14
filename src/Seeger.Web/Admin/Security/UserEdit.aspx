<%@ Page Title="{ User.Edit }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Security.UserEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<table class="formtable">
    <tr>
        <th><label class="required"><%= T("User.UserName") %></label></th>
        <td>
            <asp:TextBox runat="server" MaxLength="50" ID="UserName" />
            <asp:RequiredFieldValidator runat="server" ID="UserNameRequired" ControlToValidate="UserName" ErrorMessage="*" Display="Dynamic" />
            <asp:CustomValidator runat="server" ID="UserNameDuplicateValidator" ControlToValidate="UserName"
                 ErrorMessage="<%$ T: User.UserNameIsUsed %>"
                 OnServerValidate="UserNameDuplicateValidator_ServerValidate" />
        </td>
    </tr>
    <tr>
        <th><label class="required"><%= T("User.Nick") %></label></th>
        <td>
            <asp:TextBox runat="server" ID="Nick" MaxLength="50" autocomplete="off" />
            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="Nick" runat="server" />
        </td>
    </tr>
    <tr>
        <th><%= T("User.Email") %></th>
        <td>
            <asp:TextBox runat="server" MaxLength="250" ID="Email" autocomplete="off"  />
        </td>
    </tr>
    <tr>
        <th><%= T("User.Password") %></th>
        <td>
            <asp:TextBox runat="server" MaxLength="20" ID="Password" TextMode="Password" autocomplelte="off" />
            <asp:RequiredFieldValidator runat="server" ID="PasswordRequired" ControlToValidate="Password" ErrorMessage="*" />
        </td>
    </tr>
    <tr>
        <th><%= T("User.Role") %></th>
        <td>
            <asp:CheckBoxList runat="server" CssClass="aspnet-checkboxlist" ID="RoleList" RepeatDirection="Horizontal" DataTextField="Name" DataValueField="Id">
            </asp:CheckBoxList>
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:LinkButton runat="server" ID="SubmitButton" Text="<%$ T: Common.Save %>" CssClass="button primary" OnClick="SubmitButton_Click" />
        </td>
    </tr>
</table>

</asp:Content>
