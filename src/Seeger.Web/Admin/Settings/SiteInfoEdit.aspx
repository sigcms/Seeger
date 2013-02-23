<%@ Page Title="{ Menu.SiteInfo }" ValidateRequest="false" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="SiteInfoEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Settings.SiteInfoEdit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="mgnt-toolbar">
    <asp:PlaceHolder runat="server" ID="LanguageHodler" Visible="false">
        <span style="float:right;">
            <%= Localize("Globalization.SelectLanguage") %>:
            <asp:DropDownList runat="server" ID="LanguageList" AutoPostBack="true"
                              DataTextField="DisplayName" DataValueField="Name" AppendDataBoundItems="true"
                              OnSelectedIndexChanged="LanguageList_SelectedIndexChanged">
                <asp:ListItem Text="<%$ Resources: Common.Default %>" Value="" />
            </asp:DropDownList>
        </span>
    </asp:PlaceHolder>
</div>

<table class="formtable">
    <tr class="separator">
        <th><%= Localize("SiteInfo.BasicSetting") %></th>
        <td></td>
    </tr>
    <tr>
        <th><%= Localize("SiteInfo.SiteTitle") %></th>
        <td>
            <asp:TextBox runat="server" ID="SiteTitle" MaxLength="100" />
        </td>
    </tr>
    <tr>
        <th><%= Localize("SiteInfo.SiteSubtitle") %></th>
        <td>
            <asp:TextBox runat="server" ID="SiteSubtitle" MaxLength="300" />
        </td>
    </tr>
    <tr>
        <th><%= Localize("SiteInfo.Logo") %></th>
        <td>
            <asp:PlaceHolder runat="server" ID="LogoPreviewHolder" Visible="false">
                <div class="img-preview-panel">
                    <asp:Image runat="server" ID="LogoPreview" Width="100" />
                    <a id="delete-logo-button" href="javascript:void(0);" class="caution"><%= Localize("SiteInfo.DeleteLogo") %></a>
                    <asp:CheckBox runat="server" ID="DeleteLogo" style="display:none" />
                </div>
            </asp:PlaceHolder>
            <div>
                <asp:FileUpload runat="server" ID="LogoUpload" />
                <asp:CustomValidator runat="server" ID="LogoValidator" ErrorMessage="<%$ Resources: Message.OnlyAllowUploadImage %>"
                     OnServerValidate="LogoValidator_ServerValidate" />
            </div>
        </td>
    </tr>
    <tr>
        <th><%= Localize("SiteInfo.Copyright") %></th>
        <td>
            <asp:TextBox runat="server" ID="Copyright" MaxLength="100" />
        </td>
    </tr>
    <tr>
        <th><%= Localize("SiteInfo.MiiBeiAnNumber") %></th>
        <td>
            <asp:TextBox runat="server" ID="MiiBeiAnNumber" MaxLength="50" />
        </td>
    </tr>
    <tr class="separator">
        <th><%= Localize("SiteInfo.SEOSetting") %></th>
        <td></td>
    </tr>
    <tr>
        <th><%= Localize("Page.Title") %></th>
        <td>
            <asp:TextBox runat="server" ID="PageTitle" />
        </td>
    </tr>
    <tr>
        <th><%= Localize("Page.MetaKeywords") %></th>
        <td>
            <asp:TextBox runat="server" ID="PageMetaKeywords" />
        </td>
    </tr>
    <tr>
        <th><%= Localize("Page.MetaDescription") %></th>
        <td>
            <asp:TextBox runat="server" ID="PageMetaDescription" TextMode="MultiLine" />
        </td>
    </tr>
    <sig:AdminPlaceHolder runat="server" PermissionGroup="SiteSetting" Permission="SiteInfo">
        <tr>
            <th></th>
            <td>
                <asp:LinkButton runat="server" ID="SubmitButton" Text='<%$ Resources: Common.Save %>' CssClass="button primary" OnClick="SubmitButton_Click" />
            </td>
        </tr>
    </sig:AdminPlaceHolder>
</table>

    <script type="text/javascript">

        $("#delete-logo-button").click(function () {
            var $parent = $(this).parent();
            $parent.find("input[type=checkbox]").attr("checked", "checked");
            $parent.hide();
        });
    
    </script>

</asp:Content>
