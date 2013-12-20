<%@ Page Title="Qiniu Cloud Settings" Language="C#" MasterPageFile="Admin.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="Seeger.Plugins.QiniuCloud.Admin.Settings" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

    <table class="formtable">
        <tr>
            <th>Bucket:</th>
            <td>
                <asp:TextBox runat="server" ID="QiniuBucket" MaxLength="50" />
                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="QiniuBucket" runat="server" />
            </td>
        </tr>
        <tr>
            <th>Domain:</th>
            <td>
                <asp:TextBox runat="server" ID="Domain" MaxLength="50" />
                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="Domain" runat="server" />
            </td>
        </tr>
        <tr>
            <th>Access Key:</th>
            <td>
                <asp:TextBox runat="server" ID="AccessKey" MaxLength="100" />
                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="AccessKey" runat="server" />
            </td>
        </tr>
        <tr>
            <th>Security Key:</th>
            <td>
                <asp:TextBox runat="server" ID="SecurityKey" MaxLength="100" />
                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="SecurityKey" runat="server" />
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:Button runat="server" ID="SaveButton" CssClass="button primary" Text="Save" OnClick="SaveButton_Click" />
                <a href="/Admin/Files/Buckets.aspx" class="button secondary">Cancel</a>
            </td>
        </tr>
    </table>

</asp:Content>
