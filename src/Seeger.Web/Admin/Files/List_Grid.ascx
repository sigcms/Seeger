<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="List_Grid.ascx.cs" Inherits="Seeger.Web.UI.Admin.Files.List_Grid" %>

<table class="datatable">
    <thead>
        <tr>
            <th></th>
            <th><%= T("FileMgnt.Path") %></th>
            <th><%= T("FileMgnt.CreationTime") %></th>
            <th><%= T("FileMgnt.LastWriteTime") %></th>
            <th><%= T("Common.Operations") %></th>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater runat="server" ID="List">
            <ItemTemplate>
                <tr class="data-item">
                    <td>
                        <a href="<%# (bool)Eval("IsDirectory") ? "?path=" + Eval("VirtualPath") : Eval("VirtualPath") as string %>" class="icon-link <%# !String.IsNullOrEmpty(Eval("Extension") as string) ? "icon-file icon-" + ((string)Eval("Extension")).Substring(1).ToLower() : "icon-folder" %>" isdirectory="<%# (bool)Eval("IsDirectory") ? "1" : "0" %>" <%# (bool)Eval("IsDirectory") ? "" : "target='_blank'" %>><%# Eval("Name") %></a>
                    </td>
                    <td>
                        <%# Eval("VirtualPath") %>
                    </td>
                    <td style="text-align:center">
                        <%# Eval("CreationTime") %>
                    </td>
                    <td style="text-align:center">
                        <%# Eval("LastWriteTime") %>
                    </td>
                    <td style="text-align:center">
                        <a href="#" onclick="openRenameDialog($(this).closest('tr'));return false;">[<%= T("Common.Rename") %>]</a>
                        <a href="#" class="caution grid-action" data-action="Delete" data-action-param-path="<%# UrlUtil.Combine(CurrentPath, Eval("Name") as string) %>">[<%= T("Common.Delete") %>]</a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>