<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserList_Grid.ascx.cs" Inherits="Seeger.Web.UI.Admin.Security.UserList_Grid" %>

<table class="datatable">
    <thead>
        <tr>
            <th><%= Localize("User.UserName") %></th>
            <th><%= Localize("User.Nick") %></th>
            <th><%= Localize("User.Email") %></th>
            <% if (HasPermission("UserMgnt", "Edit")) { %>
            <th><%= Localize("Common.Edit") %></th>
            <% } %>
            <% if (HasPermission("UserMgnt", "Delete")) { %>
            <th><%= Localize("Common.Delete") %></th>
            <% } %>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater runat="server" ID="List">
            <ItemTemplate>
                <tr class="data-item">
                    <td><%# Eval("UserName") %></td>
                    <td><%# Eval("Nick") %></td>
                    <td><%# Eval("Email") %></td>
                    <% if (HasPermission("UserMgnt", "Edit")) { %>
                    <td style="text-align:center">
                        <a href="UserEdit.aspx?id=<%# Eval("Id") %>"><%= Localize("Common.Edit") %></a>
                    </td>
                    <% } %>
                    <% if (HasPermission("UserMgnt", "Delete")) { %>
                    <td style="text-align:center">
                        <a href="#" class="grid-action" data-action="Delete" data-action-param-id="<%# Eval("Id") %>"><%= Localize("Common.Delete") %></a>
                    </td>
                    <% } %>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>

<sig:Pager runat="server" ID="Pager" CssClass="pager" />