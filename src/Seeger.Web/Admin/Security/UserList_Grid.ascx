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
        <asp:Repeater runat="server" ID="List" OnItemDataBound="List_ItemDataBound">
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
                    <td style="text-align:center">
                        <asp:PlaceHolder runat="server" ID="DeleteHolder">
                            <a href="#" class="grid-action" data-action="Delete" data-action-param-id="<%# Eval("Id") %>"><%= T("Common.Delete") %></a>
                        </asp:PlaceHolder>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>

<sig:Pager runat="server" ID="Pager" CssClass="pager" />