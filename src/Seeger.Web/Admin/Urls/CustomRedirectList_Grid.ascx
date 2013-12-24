<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomRedirectList_Grid.ascx.cs" Inherits="Seeger.Web.UI.Admin.Urls.CustomRedirectList_Grid" %>

<table class="datatable">
    <thead>
        <tr>
            <th><%= T("CustomRedirect.From") %></th>
            <th><%= T("CustomRedirect.To") %></th>
            <th><%= T("CustomRedirect.MatchByRegex") %></th>
            <th><%= T("CustomRedirect.RedirectMode") %></th>
            <th><%= T("CustomRedirect.IsEnabled") %></th>
            <% if (HasPermission("CustomRedirect", "Edit")) { %>
            <th><%= T("Common.Edit") %></th>
            <% } %>
            <% if (HasPermission("CustomRedirect", "Delete")) { %>
            <th><%= T("Common.Delete") %></th>
            <% } %>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater runat="server" ID="List">
            <ItemTemplate>
                <tr class="data-item">
                    <td><%# Eval("From") %></td>
                    <td><%# Eval("To") %></td>
                    <td style="text-align:center"><%# (bool)Eval("MatchByRegex") ? "✓" : "X" %></td>
                    <td style="text-align:center"><%# T("RedirectMode." + Eval("RedirectMode")) %></td>
                    <td style="text-align:center"><%# (bool)Eval("IsEnabled") ? "✓" : "X" %></td>
                    <% if (HasPermission("CustomRedirect", "Edit")) { %>
                    <td style="text-align:center">
                        <a href="CustomRedirectEdit.aspx?id=<%# Eval("Id") %>"><%= T("Common.Edit") %></a>
                    </td>
                    <% } %>
                    <% if (HasPermission("CustomRedirect", "Delete")) { %>
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