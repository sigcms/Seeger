<%@ Page Title="{ FileMgnt.UploadFile }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="Seeger.Web.UI.Admin.Files.Upload" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<table class="formtable">
    <tr>
        <th><%= T("FileMgnt.Path") %></th>
        <td><%= Path %></td>
    </tr>
    <tr>
        <th><label class="required"><%= T("FileMgnt.File") %></label></th>
        <td>
            <asp:FileUpload runat="server" ID="FileUpload" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="FileUpload" ErrorMessage="*" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:LinkButton runat="server" ID="UploadButton" Text="<%$ T: Common.Upload %>" CssClass="button primary"
                 OnClick="UploadButton_Click" />
            <a href="javascript:location.href='List.aspx?path=<%= Path %>'" class="button secondary"><%= T("Common.Cancel") %></a>
        </td>
    </tr>
</table>

</asp:Content>
