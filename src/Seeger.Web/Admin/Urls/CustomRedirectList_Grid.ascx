<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomRedirectList_Grid.ascx.cs" Inherits="Seeger.Web.UI.Admin.Urls.CustomRedirectList_Grid" %>

<table class="datatable">
    <thead>
        <tr>
            <th><%= Localize("CustomRedirect.RedirectMode") %></th>
            <th><%= Localize("CustomRedirect.From") %></th>
            <th><%= Localize("CustomRedirect.To") %></th>
            <th><%= Localize("CustomRedirect.MatchByRegex") %></th>
            <% if (HasPermission("CustomRedirect", "Edit")) { %>
            <th><%= Localize("Common.Edit") %></th>
            <% } %>
            <% if (HasPermission("CustomRedirect", "Delete")) { %>
            <th><%= Localize("Common.Delete") %></th>
            <% } %>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater runat="server" ID="List">
            <ItemTemplate>
                <tr class="data-item">
                    <td><%# Localize("RedirectMode." + Eval("RedirectMode")) %></td>
                    <td><%# Eval("From") %></td>
                    <td><%# Eval("To") %></td>
                    <td><%# Eval("MatchByRegex") %></td>
                    <% if (HasPermission("CustomRedirect", "Edit")) { %>
                    <td>
                        <a href="CustomRedirectEdit.aspx?id=<%# Eval("Id") %>"><%= Localize("Common.Edit") %></a>
                    </td>
                    <% } %>
                    <% if (HasPermission("CustomRedirect", "Delete")) { %>
                    <td>
                        <a href="#" class="grid-action" data-action="Delete" data-action-param-id="<%# Eval("Id") %>"><%= Localize("Common.Delete") %></a>
                    </td>
                    <% } %>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>

<sig:Pager runat="server" ID="Pager" CssClass="pager" />