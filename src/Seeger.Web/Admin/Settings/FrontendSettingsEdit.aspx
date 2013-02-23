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
            <th><%= Localize("Setting.PageExtension") %></th>
            <td>
                <asp:DropDownList runat="server" ID="PageExtension" AppendDataBoundItems="true">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th></th>
            <td></td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:CheckBox runat="server" ID="CloseWebsite" CssClass="aspnet-checkbox" Text="<%$ Resources: Setting.CloseWebsite %>" />
            </td>
        </tr>
        <tr id="offline-url-row" style='<%= GetOfflineUrlRowStyle() %>'>
            <th></th>
            <td>
                <label style="display:block;padding-bottom:5px;"><%= Localize("Setting.SelectOfflinePage") %>:</label>
                <uc:PageDropDownList ID="PageList" runat="server" AutoBind="false" />
            </td>
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

    <script type="text/javascript">
        $("#<%= CloseWebsite.ClientID %>").click(function () {
            var $this = $(this);
            if ($this.is(":checked")) {
                $("#offline-url-row").show();
            } else {
                $("#offline-url-row").hide();
            }
        });
    </script>
</asp:Content>
