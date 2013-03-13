<%@ Page Title="{ Globalization.EditFrontendLanguage }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master"
    AutoEventWireup="true" CodeBehind="FrontendLangEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Settings.FrontendLangEdit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <table class="formtable">
        <tr>
            <th><%= Localize("Globalization.LanguageName") %></th>
            <td>
                <asp:DropDownList runat="server" ID="LanguageList" DataTextField="DisplayName" DataValueField="Name">
                </asp:DropDownList>
                <asp:CustomValidator runat="server" ID="LanguageDuplicateValidator" ErrorMessage="<%$ Resources: Globalization.ErrMsg.LanguageAlreadyAdded %>"
                     OnServerValidate="LanguageDuplicateValidator_ServerValidate" ControlToValidate="LanguageList" />
            </td>
        </tr>
        <tr>
            <th><label class="required"><%= Localize("Globalization.LanguageDisplayName") %></label></th>
            <td>
                <asp:TextBox runat="server" ID="DisplayName" MaxLength="50" />
                <asp:RequiredFieldValidator runat="server" ID="DisplayNameRequiredValidator" ErrorMessage="*" ControlToValidate="DisplayName" />
            </td>
        </tr>
        <tr>
            <th><label class="has-help-hint" title="<%= Localize("Globalization.Help_BindedDomain") %>"><%= Localize("Globalization.BindedDomain")%></label></th>
            <td>
                <asp:TextBox runat="server" ID="Domain" MaxLength="50" />
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:LinkButton runat="server" ID="SubmitButton" Text="<%$ Resources: Common.Save %>" CssClass="button primary" OnClick="SubmitButton_Click" />
                <a href="FrontendLangList.aspx" class="button secondary"><%= Localize("Common.Cancel") %></a>
            </td>
        </tr>
    </table>

</asp:Content>
