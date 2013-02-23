<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WidgetList.ascx.cs"
    Inherits="Seeger.Web.UI.Admin.Designer.Controls.WidgetList" %>

<asp:PlaceHolder runat="server" ID="ContainerHolder" Visible="false">

<asp:Repeater runat="server" ID="WidgetRepeater">
    <ItemTemplate>
        <div widget-name='<%# Eval("Name") %>' 
            plugin-name='<%# Eval("Plugin.Name") %>'
            auto-open='<%# IsEditorAutoOpen(Container.DataItem).ToString().ToLower() %>'
            class="sig-widget-item">
            <img class="sig-widget-icon" src='<%# GetIconUrl(Container.DataItem) %>' alt='<%# Eval("Name") %>' />
            <div class="sig-widget-desc">
                <%# Eval("DisplayName") %>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>

<div style="clear:both;"></div>

</asp:PlaceHolder>