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
        <% foreach (var item in Items) {
               var textClass = "";
               if (item.Level == LogLevel.Error || item.Level == LogLevel.Fatal)
               {
                   textClass = "text-danger";
               }
               else if (item.Level == LogLevel.Warn)
               {
                   textClass = "text-warning";
               }

               var hasDetail = !String.IsNullOrEmpty(item.Exception);
        %>
            <tr class="data-item <%= textClass %>">
                <td style="text-align:center;vertical-align:top"><%= item.UtcTimestamp.ToLocalTime() %></td>
                <td>
                    <% if (hasDetail) { %>
                        <div style="cursor:pointer" data-toggle="view-log-detail"><%= TransformMessage(item.Message) %></div>
                        <div style="display:none;margin-top:10px" class="log-detail">
                            <pre><code><%= item.Exception %></code></pre>
                        </div>
                    <% } else { %>
                        <div><%= TransformMessage(item.Message) %></div>
                    <% } %>
                </td>
                <td style="text-align:center;vertical-align:top">
                    <%= GetOperatorHtml(item) %>
                </td>
            </tr>
        <% } %>
    </tbody>
</table>

<sig:Pager runat="server" ID="Pager" CssClass="pager" />
