<%@ Page Title="{ Setting.FrontendSetting }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="FrontendSettingsEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Settings.FrontendSettingsEdit" %>

<%@ Register src="../Shared/Controls/PageDropDownList.ascx" tagname="PageDropDownList" tagprefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <table class="formtable">
        <sig:AdminPlaceHolder runat="server" Feature="Multilingual">
            <tr>
                <th>
                    <%= Localize("Setting.Multilingual") %><br />
                </th>
                <td>
                    <asp:CheckBox ID="Multilingual" runat="server" />
                    <span class="input-hint">
                        <%= Localize("Common.YouMayNeedTo") %><a href="FrontendLangList.aspx"><%= Localize("Setting.ManageFrontendLanguages") %></a>
                    </span>
                </td>
            </tr>
            <tr>
                <th><%= Localize("Setting.DefaultLanguage") %></th>
                <td>
                     <asp:DropDownList runat="server" ID="FrontendLang" DataTextField="DisplayName" DataValueField="Name" AppendDataBoundItems="true">
                     </asp:DropDownList>
               </td>
            </tr>
       </sig:AdminPlaceHolder>
        <tr>
            <th></th>
            <td></td>
        </tr>
        <sig:AdminPlaceHolder runat="server" PermissionGroup="FrontendSetting" Permission="Edit">
            <tr>
                <th></th>
                <td>
                    <asp:LinkButton Text="<%$ Resources: Common.Save %>" ID="SaveButton" CssClass="button primary" OnClick="SaveButton_Click" runat="server" />
                </td>
            </tr>
        </sig:AdminPlaceHolder>
    </table>

</asp:Content>
