<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleList_Grid.ascx.cs" Inherits="Seeger.Web.UI.Admin.Security.RoleList_Grid" %>

<table class="datatable">
    <thead>
        <tr>
            <th><%= Localize("Role.Name") %></th>
            <th class="edit-column"><%= Localize("Common.Edit") %></th>
            <th class="delete-column"><%= Localize("Common.Delete") %></th>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater runat="server" ID="List">
            <ItemTemplate>
                <tr class="data-item">
                    <td><%# Eval("Name") %></td>
                    <td style="text-align:center">
                        <sig:AdminPlaceHolder runat="server" PermissionGroup="Role" Permission="Edit">
                            <a href="RoleEdit.aspx?id=<%# Eval("Id") %>"><%= Localize("Common.Edit") %></a>
                        </sig:AdminPlaceHolder>
                    </td>
                    <td style="text-align:center">
                        <sig:AdminPlaceHolder runat="server" PermissionGroup="Role" Permission="Delete">
                            <a href="#" class="grid-action" data-action="Delete" data-action-param-id="<%# Eval("Id") %>"><%= Localize("Common.Delete") %></a>
                        </sig:AdminPlaceHolder>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>

<sig:Pager runat="server" ID="Pager" CssClass="pager" />
