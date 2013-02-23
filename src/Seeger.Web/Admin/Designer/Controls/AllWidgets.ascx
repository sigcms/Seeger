<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllWidgets.ascx.cs" Inherits="Seeger.Web.UI.Admin.Designer.Controls.AllWidgets" %>

<%@ Register src="WidgetList.ascx" tagname="WidgetList" tagprefix="uc" %>

<ul id="sig-widget-groups" class="sig-widget-groups" style="zoom:1">
    <asp:Repeater runat="server" ID="CategoryRepeater">
        <ItemTemplate>
            <li><a href='#sig-widget-group-<%# Eval("Key") %>'><%# Eval("Name") %></a></li>
        </ItemTemplate>
    </asp:Repeater>
</ul>

<div style="clear:both;padding-top:8px;">
    <asp:Repeater runat="server" ID="CategoryWidgetsRepeater" OnItemDataBound="CategoryWidgetsRepeater_ItemDataBound">
        <ItemTemplate>
            <div id='sig-widget-group-<%# Eval("Key") %>' style="display:none;zoom:1;">
                <uc:WidgetList ID="WidgetList" runat="server" AutoBind="false" />
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

<script type="text/javascript">$("#sig-widget-groups").looklesstab();</script>