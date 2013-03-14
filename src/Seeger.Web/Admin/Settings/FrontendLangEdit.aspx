<%@ Page Title="{ Globalization.EditFrontendLanguage }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master"
    AutoEventWireup="true" CodeBehind="FrontendLangEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Settings.FrontendLangEdit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <table class="formtable">
        <tr>
            <th><%= T("Globalization.LanguageName") %></th>
            <td>
                <asp:DropDownList runat="server" ID="LanguageList" DataTextField="DisplayName" DataValueField="Name">
                </asp:DropDownList>
                <asp:CustomValidator runat="server" ID="LanguageDuplicateValidator" ErrorMessage="<%$ T: Globalization.ErrMsg.LanguageAlreadyAdded %>"
                     OnServerValidate="LanguageDuplicateValidator_ServerValidate" ControlToValidate="LanguageList" />
            </td>
        </tr>
        <tr>
            <th><label class="required"><%= T("Globalization.LanguageDisplayName") %></label></th>
            <td>
                <asp:TextBox runat="server" ID="DisplayName" MaxLength="50" />
                <asp:RequiredFieldValidator runat="server" ID="DisplayNameRequiredValidator" ErrorMessage="*" ControlToValidate="DisplayName" />
            </td>
        </tr>
        <tr>
            <th><label class="has-help-hint" title="<%= T("Globalization.Help_BindedDomain") %>"><%= T("Globalization.BindedDomain")%></label></th>
            <td>
                <asp:TextBox runat="server" ID="Domain" MaxLength="50" />
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:LinkButton runat="server" ID="SubmitButton" Text="<%$ T: Common.Save %>" CssClass="button primary" OnClick="SubmitButton_Click" />
                <a href="FrontendLangList.aspx" class="button secondary"><%= T("Common.Cancel") %></a>
            </td>
        </tr>
    </table>

</asp:Content>
