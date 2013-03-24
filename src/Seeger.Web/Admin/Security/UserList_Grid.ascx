<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserList_Grid.ascx.cs" Inherits="Seeger.Web.UI.Admin.Security.UserList_Grid" %>

<table class="datatable">
    <thead>
        <tr>
            <th><%= T("User.UserName") %></th>
            <th><%= T("User.Nick") %></th>
            <th><%= T("User.Email") %></th>
            <th><%= T("User.LastLoginTime") %></th>
            <th><%= T("User.LastLoginIP") %></th>
            <% if (HasPermission("UserMgnt", "Edit")) { %>
            <th><%= T("Common.Edit") %></th>
            <% } %>
            <% if (HasPermission("UserMgnt", "Delete")) { %>
            <th><%= T("Common.Delete") %></th>
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
                    <td style="text-align:center"><%# Eval("LastLoginTime") %></td>
                    <td style="text-align:center"><%# Eval("LastLoginIP") %></td>
                    <% if (HasPermission("UserMgnt", "Edit")) { %>
                    <td style="text-align:center">
                        <a href="UserEdit.aspx?id=<%# Eval("Id") %>"><%= T("Common.Edit") %></a>
                    </td>
                    <% } %>
                    <% if (HasPermission("UserMgnt", "Delete")) { %>
                    <td style="text-align:center">
                        <a href="#" class="grid-action" data-action="Delete" data-action-param-id="<%# Eval("Id") %>"><%= T("Common.Delete") %></a>
                    </td>
                    <% } %>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>

<sig:Pager runat="server" ID="Pager" CssClass="pager" />