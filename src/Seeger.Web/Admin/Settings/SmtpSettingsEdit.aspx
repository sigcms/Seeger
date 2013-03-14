<%@ Page Title="{ Function.EmailSetting }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="SmtpSettingsEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Settings.SmtpSettingsEdit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

<table class="formtable">
    <tr>
        <th><%= T("Email.Server")%></th>
        <td>
            <asp:TextBox runat="server" ID="SmtpServer" MaxLength="50" />
            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="SmtpServer"
                runat="server" />
        </td>
    </tr>
    <tr>
        <th>
            <%= T("Email.Port") %>
        </th>
        <td>
            <asp:TextBox runat="server" ID="Port" MaxLength="5" Width="50" />
            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="Port"
                runat="server" />
            <asp:CheckBox runat="server" ID="EnableSsl" Text="<%$ T: Email.EnableSsl %>" />
            <asp:RegularExpressionValidator ErrorMessage="<%$ T: Email.PortShouldBeNumber %>" ControlToValidate="Port"
                runat="server" Display="Dynamic" ValidationExpression="^\d+$" />
        </td>
    </tr>
    <tr>
        <th><%= T("Email.SenderName") %></th>
        <td>
            <asp:TextBox runat="server" ID="SenderName" MaxLength="50" />
            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="SenderName"
                runat="server" Display="Dynamic" />
        </td>
    </tr>
    <tr>
        <th><%= T("Email.SenderEmail") %></th>
        <td>
            <asp:TextBox runat="server" ID="SenderEmail" MaxLength="50" />
            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="SenderEmail"
                runat="server" Display="Dynamic" />
        </td>
    </tr>
    <tr>
        <th><%= T("Email.AccountName") %></th>
        <td>
            <asp:TextBox runat="server" ID="AccountName" MaxLength="50" autocomplete="off" />
            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="AccountName"
                runat="server" Display="Dynamic" />
        </td>
    </tr>
    <tr>
        <th><%= T("Email.AccountPassword") %></th>
        <td>
            <asp:TextBox runat="server" ID="AccountPassword" TextMode="Password" MaxLength="50" autocomplete="off" />
            <span class="input-hint"><%= T("Email.KeepBlankIfNoNeedToChange") %></span>
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:Button runat="server" ID="SaveButton" Text="<%$ T: Common.Save %>" CssClass="button primary"
                 OnClick="SaveButton_Click" />
        </td>
    </tr>
</table>

</asp:Content>
