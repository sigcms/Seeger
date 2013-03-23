<%@ Page Title="{ Role.Edit }" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="RoleEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Security.RoleEdit" %>
<%@ Register TagName="PrivilegeView" TagPrefix="uc" Src="PrivilegeView.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<table class="formtable">
    <tr>
        <th><label class="required"><%= T("Role.Name") %></label></th>
        <td>
            <asp:TextBox runat="server" ID="Name" MaxLength="50" />
            <asp:RequiredFieldValidator runat="server" ID="NameRequiredValidator" ErrorMessage="*" ControlToValidate="Name" />
        </td>
    </tr>
    <tr>
        <th><%= T("Role.Privileges") %></th>
        <td>
            <uc:PrivilegeView runat="server" ID="Privileges" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:LinkButton runat="server" ID="SubmitButton" Text="<%$ T: Common.Save %>" CssClass="button primary" OnClick="SubmitButton_Click" />
            <a href="RoleList.aspx" class="button secondary"><%= T("Common.Back") %></a>
        </td>
    </tr>
</table>

</asp:Content>
