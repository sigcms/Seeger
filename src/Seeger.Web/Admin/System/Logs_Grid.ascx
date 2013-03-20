<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Logs_Grid.ascx.cs" Inherits="Seeger.Web.UI.Admin._System.Logs_Grid" %>
<%@ Import Namespace="Seeger.Logging" %>

<table class="datatable">
    <thead>
        <tr>
            <th style="width:160px"><%= T("DateTime") %></th>
            <th><%= T("Message") %></th>
            <th style="width:120px"><%= T("Operator") %></th>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater runat="server" ID="List">
            <ItemTemplate>
                <tr class="data-item">
                    <td style="text-align:center"><%# ((DateTime)Eval("UtcTimestamp")).ToLocalTime() %></td>
                    <td><div style="color:<%# (LogLevel)Eval("Level") >= LogLevel.Error ? "red" : "" %>"><%# TransformMessage(Eval("Message") as string) %></div></td>
                    <td style="text-align:center">
                        <%# GetOperatorHtml(Container.DataItem) %>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>

<sig:Pager runat="server" ID="Pager" CssClass="pager" />
