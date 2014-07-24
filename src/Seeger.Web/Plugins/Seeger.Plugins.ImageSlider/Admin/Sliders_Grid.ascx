<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Sliders_Grid.ascx.cs" Inherits="Seeger.Plugins.ImageSlider.Admin.Sliders_Grid" %>

<table class="datatable">
    <thead>
        <tr>
            <th><%= T("Slider name") %></th>
            <th><%= T("Total items") %></th>
            <th><%= T("Created at") %></th>
            <th><%= T("Operations") %></th>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater runat="server" ID="List">
            <ItemTemplate>
                <tr class="data-item">
                    <td><%# Eval("Name") %></td>
                    <td style="text-align:center"><%# Eval("Items.Count") %></td>
                    <td style="text-align:center"><%# ((DateTime)Eval("UtcCreatedAt")).ToLocalTime() %></td>
                    <td style="text-align:center">
                        <a href="SliderItems.aspx?sliderId=<%# Eval("Id") %>"><%= T("Slider items") %></a>
                        <% if (CanDelete) { %>
                        <a href="#" data-action="Delete" data-action-param-id="<%# Eval("Id") %>"><%= T("Delete") %></a>
                        <% } %>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>

<sig:Pager runat="server" ID="Pager" CssClass="pager" />