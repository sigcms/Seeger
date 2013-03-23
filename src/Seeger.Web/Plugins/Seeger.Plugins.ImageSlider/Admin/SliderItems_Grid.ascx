<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SliderItems_Grid.ascx.cs" Inherits="Seeger.Plugins.ImageSlider.Admin.SliderItems_Grid" %>

<table class="datatable">
    <thead>
        <tr>
            <th style="width:110px"><%= T("Image") %></th>
            <th><%= T("Caption") %></th>
            <th><%= T("NavigateUrl") %></th>
            <th><%= T("DisplayOrder") %></th>
            <th><%= T("Operations") %></th>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater runat="server" ID="List">
            <ItemTemplate>
                <tr class="data-item">
                    <td>
                        <img src="<%# Eval("ImageUrl") %>" style="max-height:50px;max-width:110px" />
                    </td>
                    <td>
                        <%# Eval("Caption") %>
                    </td>
                    <td>
                        <%# Eval("NavigateUrl") %>
                    </td>
                    <td style="text-align:center">
                        <%# Eval("DisplayOrder") %>
                    </td>
                    <td style="text-align:center">
                        <% if (CanEdit) { %>
                        <a href="SliderItemEdit.aspx?sliderId=<%= SliderId %>&itemId=<%# Eval("Id") %>"><%= T("Edit") %></a>
                        <% } %>
                        <% if (CanDelete) { %>
                        <a href="#" data-action="Delete" data-action-param-sliderId="<%= SliderId %>" data-action-param-itemId="<%# Eval("Id") %>"><%# T("Delete") %></a>
                        <% } %>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>