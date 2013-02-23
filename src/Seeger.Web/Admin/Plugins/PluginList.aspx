<%@ Page Title="{ Menu.PluginMgnt }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="PluginList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Plugins.PluginList" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

    <asp:Repeater runat="server" ID="List" OnItemDataBound="List_ItemDataBound">
        <ItemTemplate>
            <div class="plugin-item" plugin-name="<%# Eval("Name") %>">
                <h3 class="plugin-name"><%# Eval("DisplayName") %><em>(<%# Eval("Name") %>)</em></h3>
                <div class="plugin-desc">
                    <%# Eval("Description") %>
                </div>
                <div>
                    <asp:PlaceHolder runat="server" ID="InstallHolder" Visible="false">
                        <button class="btn-install button primary"><%= Localize("Common.Install") %></button>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="UninstallHolder" Visible="false">
                        <button class="btn-uninstall button secondary"><%= Localize("Common.Uninstall") %></button>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="EnableHolder" Visible="false">
                        <button class="btn-enable button secondary"><%= Localize("Common.Enable") %></button>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="DisableHolder" Visible="false">
                        <button class="btn-disable button secondary"><%= Localize("Common.Disable") %></button>
                    </asp:PlaceHolder>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <script type="text/javascript">
        $(function () {
            $('.btn-install').click(function () {
                var name = $(this).closest('.plugin-item').attr('plugin-name');
                PageMethods.Install(name, function () {
                    location.href = location.href;
                }, function (e) {
                    alert(e.get_message());
                });
                return false;
            });
            $('.btn-uninstall').click(function () {
                if (confirm('Are you sure to uninstall this plugin?')) {
                    var name = $(this).closest('.plugin-item').attr('plugin-name');
                    PageMethods.Uninstall(name, function () {
                        location.href = location.href;
                    }, function (e) {
                        alert(e.get_message());
                    });
                }
                return false;
            });
            $('.btn-enable').click(function () {
                var name = $(this).closest('.plugin-item').attr('plugin-name');
                PageMethods.Enable(name, function () {
                    location.href = location.href;
                }, function (e) {
                    alert(e.get_message());
                });
                return false;
            });
            $('.btn-disable').click(function () {
                var name = $(this).closest('.plugin-item').attr('plugin-name');
                PageMethods.Disable(name, function () {
                    location.href = location.href;
                }, function (e) {
                    alert(e.get_message());
                });
                return false;
            });
        });
    </script>

</asp:Content>
