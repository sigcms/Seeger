<%@ Page Title="{ User.MyProfile }" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Seeger.Web.UI.Admin.My.Profile" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="page-header">
    <h1><%= T("User.MyProfile") %></h1>
</div>

<table class="formtable">
    <tr>
        <th><%= T("User.UserName") %></th>
        <td>
            <asp:Literal runat="server" ID="UserName" />
        </td>
    </tr>
    <tr>
        <th><%= T("User.Nick") %></th>
        <td>
            <asp:TextBox runat="server" MaxLength="50" ID="Nick" />
            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="Nick" runat="server" />
        </td>
    </tr>
    <tr>
        <th><%= T("User.Email") %></th>
        <td>
            <asp:TextBox runat="server" MaxLength="250" ID="Email" />
            <asp:RequiredFieldValidator runat="server" ID="EmailRequiredValidator" ControlToValidate="Email" ErrorMessage="*" />
        </td>
    </tr>
    <tr>
        <th><%= T("User.Password") %></th>
        <td>
            <asp:TextBox runat="server" AutoCompleteType="None" MaxLength="20" ID="Password" TextMode="Password" />
        </td>
    </tr>
    <tr>
        <th><%= T("User.PasswordConfirm") %></th>
        <td>
            <asp:TextBox runat="server" AutoCompleteType="None" MaxLength="20" ID="PasswordConfirm" TextMode="Password" />
            <asp:CompareValidator runat="server" ID="PasswordConfirmValidator" ControlToValidate="Password"
                 ControlToCompare="PasswordConfirm" Operator="Equal" ErrorMessage="<%$ T: User.TwoPasswordNotSame %>" />
        </td>
    </tr>
    <tr>
        <th><%= T("User.Skin") %></th>
        <td>
            <asp:DropDownList runat="server" ID="SkinList">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th><%= T("User.Language") %></th>
        <td>
            <asp:DropDownList runat="server" ID="LanguageList" DataTextField="DisplayName" DataValueField="Name">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:LinkButton runat="server" ID="SubmitButton" CssClass="button primary" Text="<%$ T: Common.Save %>" OnClick="SubmitButton_Click" />

            <a href="javascript:history.back();" class="button secondary"><%= T("Common.Back") %></a>
        </td>
    </tr>
</table>

</asp:Content>
