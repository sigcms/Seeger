<%@ Page Title="Add/Edit Bucket" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BucketEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Files.BucketEdit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

    <table class="formtable">
        <tr>
            <th>Display Name:</th>
            <td>
                <asp:TextBox runat="server" ID="DisplayName" MaxLength="50" />
                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="DisplayName" runat="server" />
            </td>
        </tr>
        <tr>
            <th>File System:</th>
            <td>
                <asp:DropDownList runat="server" ID="FileSystem" />
                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="FileSystem" runat="server" />
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:Button runat="server" ID="NextButton" CssClass="button primary" Text="Next" OnClick="NextButton_Click" />
                <a href="Buckets.aspx" class="button secondary">Cancel</a>
            </td>
        </tr>
    </table>

</asp:Content>
