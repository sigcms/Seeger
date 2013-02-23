<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Shared/Popup.master" AutoEventWireup="true"
    CodeBehind="PageEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Pages.PageEdit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <table class="formtable">
        <asp:PlaceHolder runat="server" ID="ParentPageHolder" Visible="false">
            <tr>
                <th><%= Localize("Page.ParentPage") %></th>
                <td>
                    <asp:Literal runat="server" ID="ParentPageName" />
                </td>
            </tr>
        </asp:PlaceHolder>
        <tr>
            <th><label class="required"><%= Localize("Page.DisplayName") %></label></th>
            <td>
                <asp:TextBox runat="server" ID="Name" />
                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="Name" runat="server" />
                <asp:PlaceHolder runat="server" ID="VisibleInMenuHolder">
                    <span>
                        <asp:CheckBox runat="server" ID="VisibleInMenu" CssClass="aspnet-checkbox" Checked="true" Text="<%$ Resources: Page.VisibleInMenu %>" />
                    </span>
                </asp:PlaceHolder>
            </td>
        </tr>
        <sig:DevPlaceHolder runat="server">
            <tr>
                <th><%= Localize("Page.UniqueName") %></th>
                <td>
                    <asp:TextBox runat="server" ID="PageUniqueName" MaxLength="50" />
                </td>
            </tr>
             <tr>
                <th></th>
                <td>
                    <asp:CheckBox runat="server" ID="Deletable" Text="<%$ Resources: Page.IsDeletable %>" Checked="true" />
                </td>
            </tr>
        </sig:DevPlaceHolder>
        <tr>
            <th><label class="required"><%= Localize("Page.Url") %></label></th>
            <td>
                <asp:Literal runat="server" ID="BaseUrl" />
                <asp:TextBox runat="server" ID="UrlSegment" Width="60" />
                <%= FrontendSettings.PageExtension %>
                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="UrlSegment" Display="Dynamic"
                    runat="server" />
                <asp:CustomValidator ID="UrlSegmentDuplicateValidator" ErrorMessage="<%$ Resources: Page.ErrMsg.DuplicateUrlSegment %>"
                    ControlToValidate="UrlSegment" Display="Dynamic" runat="server" OnServerValidate="UrlSegmentDuplicateValidator_ServerValidate" />
            </td>
        </tr>
        <tr>
            <th><%= Localize("Page.BindedDomains") %></th>
            <td>
                <asp:TextBox runat="server" ID="BindedDomains" />
            </td>
        </tr>
        <tr>
            <th><%= Localize("Page.Template") %></th>
            <td>
                <asp:DropDownList runat="server" ID="TemplateList">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th><%= Localize("Page.Layout") %></th>
            <td>
                <div id="layout-list-container">
                    <asp:Repeater runat="server" ID="TemplateLayoutList"
                         OnItemDataBound="TemplateLayoutList_ItemDataBound">
                        <ItemTemplate>
                            <div class="image-item-selector" style="display:none;" template-name='<%# Eval("Name") %>'>
                                <asp:Repeater runat="server" ID="LayoutList">
                                    <ItemTemplate>
                                        <a class="image-item" layout-name='<%# Eval("FullName") %>' title='<%# Eval("DisplayName") %>' href='#'>
                                            <img src='<%# Eval("PreviewImageVirtualPath") %>' alt='<%# Eval("DisplayName") %>' />
                                            <span class="selected-tick"></span>
                                        </a>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <div class="clear"></div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <asp:TextBox runat="server" style="display:none" ID="CurrentLayout" />
            </td>
        </tr>
        <tr id="skin-row">
            <th><%= Localize("Page.Skin") %></th>
            <td>
                <div id="skin-list-container">
                    <asp:Repeater runat="server" ID="TemplateSkinList"
                         OnItemDataBound="TemplateSkinList_ItemDataBound">
                        <ItemTemplate>
                            <div class="image-item-selector" style="display:none" template-name='<%# Eval("Name") %>'>
                                <asp:Repeater runat="server" ID="SkinList">
                                    <ItemTemplate>
                                        <a class="image-item" skin-name='<%# Eval("FullName") %>' title='<%# Eval("DisplayName") %>' href='#'>
                                            <img src='<%# Eval("PreviewImageVirtualPath") %>' alt='<%# Eval("DisplayName") %>' />
                                            <span class="selected-tick"></span>
                                        </a>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <div class="clear"></div>
                            </div>                    
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <asp:TextBox runat="server" style="display:none" ID="CurrentSkin" />
            </td>
        </tr>
         <tr>
            <th></th>
            <td>
                <asp:CheckBox runat="server" ID="Published" Text="<%$ Resources: Page.Publish %>" Checked="true" />
            </td>
        </tr>
       <tr>
            <th></th>
            <td>
                <asp:LinkButton Text='<%$ Resources: Common.Save %>' runat="server" ID="SaveButton" CssClass="button primary" OnClick="SaveButton_Click" />
                <a href="javascript:window.parent.Sig.Window.close('Dialog');" class="button secondary"><%= Localize("Common.Cancel") %></a>
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        var Messages = {
            SaveSuccess: '<%= Localize("Message.SaveSuccess") %>'
        }

        var $currentLayout = $("#<%= CurrentLayout.ClientID %>");
        var $currentSkin = $("#<%= CurrentSkin.ClientID %>");
        var $templateList = $("#<%= TemplateList.ClientID %>");
        var $layoutListContainer = $("#layout-list-container");
        var $skinListContainer = $("#skin-list-container");

    </script>
    <script type="text/javascript" src="PageEdit.js"></script>

</asp:Content>
