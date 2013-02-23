<%@ Page Title="{ Role.Edit }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="RoleEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Security.RoleEdit" %>
<%@ Register TagName="PrivilegeView" TagPrefix="uc" Src="PrivilegeView.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<table class="formtable">
    <tr>
        <th><label class="required"><%= Localize("Role.Name") %></label></th>
        <td>
            <asp:TextBox runat="server" ID="Name" MaxLength="50" />
            <asp:RequiredFieldValidator runat="server" ID="NameRequiredValidator" ErrorMessage="*" ControlToValidate="Name" />
        </td>
    </tr>
    <tr>
        <th><%= Localize("Role.Privileges") %></th>
        <td>
            <uc:PrivilegeView runat="server" ID="Privileges" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:LinkButton runat="server" ID="SubmitButton" Text="<%$ Resources: Common.Save %>" CssClass="button primary" />
            <a href="RoleList.aspx" class="button secondary"><%= Localize("Common.Back") %></a>
        </td>
    </tr>
</table>

</asp:Content>
