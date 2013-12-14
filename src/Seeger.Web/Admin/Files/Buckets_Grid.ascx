<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Buckets_Grid.ascx.cs" Inherits="Seeger.Web.UI.Admin.Files.Buckets_Grid" %>

<table class="datatable">
    <thead>
        <tr>
            <th>Name</th>
            <th>Default</th>
            <th style="width:200px">Actions</th>
        </tr>
    </thead>
    <tbody>
        <% foreach (var meta in Metas) { %>
            <tr class="data-item" data-bucketid="<%= meta.BucketId %>">
                <td><%= meta.DisplayName %></td>
                <td><%= meta.IsDefault ? "Yes" : "No" %></td>
                <td style="text-align:center">
                    <a href="#" data-action="SetDefault" data-action-param-bucketid="<%= meta.BucketId %>">Set Default</a>
                    <a href="<%= GetConfigurationUrl(meta.BucketId) %>">Configure</a>
                </td>
            </tr>
        <% } %>
    </tbody>
</table>