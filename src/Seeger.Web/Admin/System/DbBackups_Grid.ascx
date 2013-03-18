<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DbBackups_Grid.ascx.cs" Inherits="Seeger.Web.UI.Admin._System.DbBackups_Grid" %>

<table class="datatable">
    <thead>
        <tr>
            <th><%= T("Filename") %></th>
            <th><%= T("Backup Time") %></th>
            <th><%= T("Operations") %></th>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater runat="server" ID="List">
            <ItemTemplate>
                <tr class="data-item">
                    <td><%# Eval("Name") %></td>
                    <td style="text-align:center"><%# ((DateTime)Eval("CreationTimeUtc")).ToLocalTime() %></td>
                    <td style="text-align:center">
                        <a href="DownloadBackup.ashx?file=<%# Eval("Name") %>"><%= T("Download") %></a>
                        <a href="#" class="grid-action" data-action="Delete" data-action-param-file="<%# Eval("Name") %>"><%= T("Common.Delete") %></a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>