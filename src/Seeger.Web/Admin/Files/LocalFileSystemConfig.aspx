<%@ Page Title="Local Bucket Configuration" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="LocalFileSystemConfig.aspx.cs" Inherits="Seeger.Web.UI.Admin.Files.LocalFileSystemConfig" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

    <div class="page-header">
        <h1>Local Bucket Configuration</h1>
    </div>

    <table class="formtable">
        <tr>
            <th>Base Path:</th>
            <td>
                /Files<asp:TextBox runat="server" ID="BaseVirtualPath" MaxLength="50" />
                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="BaseVirtualPath" runat="server" />
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:Button runat="server" CssClass="button primary" ID="SaveButton" Text="Save" OnClick="SaveButton_Click" />
                <a href="Buckets.aspx" class="button secondary">Cancel</a>
            </td>
        </tr>
    </table>

</asp:Content>
