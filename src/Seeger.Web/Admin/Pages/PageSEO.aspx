<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Shared/Popup.Master" AutoEventWireup="true" CodeBehind="PageSEO.aspx.cs" Inherits="Seeger.Web.UI.Admin.Pages.PageSEO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        function onSaved() {
            window.parent.Sig.Window.close('Dialog');
            window.parent.Sig.Message.show('<%= T("Message.SaveSuccess") %>');
        }
    
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<table class="formtable">
    <asp:PlaceHolder runat="server" ID="LanguageHolder" Visible="false">
        <tr>
            <th><%= T("Globalization.LanguageName") %></th>
            <td>
                <asp:Literal runat="server" ID="LanguageName" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <th><%= T("Page.Title") %></th>
        <td>
            <asp:TextBox runat="server" ID="PageTitle" />
        </td>
    </tr>
    <tr>
        <th><%= T("Page.MetaKeywords") %></th>
        <td>
            <asp:TextBox runat="server" ID="PageMetaKeywords" />
        </td>
    </tr>
    <tr>
        <th><%= T("Page.MetaDescription") %></th>
        <td>
            <asp:TextBox runat="server" ID="PageMetaDescription" TextMode="MultiLine" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:LinkButton runat="server" ID="SaveButton" Text='<%$ T: Common.Save %>' CssClass="button primary" OnClick="SaveButton_Click" />
            <a href="javascript:window.parent.Sig.Window.close('Dialog');" class="button secondary"><%= T("Common.Cancel") %></a>
        </td>
    </tr>
</table>

</asp:Content>
