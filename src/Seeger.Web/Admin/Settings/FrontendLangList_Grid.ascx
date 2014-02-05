<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrontendLangList_Grid.ascx.cs" Inherits="Seeger.Web.UI.Admin.Settings.FrontendLangList_Grid" %>

<table class="datatable">
    <thead>
        <tr>
            <th><%= T("Globalization.LanguageName") %></th>
            <th><%= T("Globalization.LanguageDisplayName") %></th>
            <th><%= T("Globalization.BindedDomain") %></th>
            <th><%= T("Common.Edit") %></th>
            <th><%= T("Common.Delete") %></th>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater runat="server" ID="List">
            <ItemTemplate>
                <tr class="data-item">
                    <td><%# Eval("Name") %></td>
                    <td><%# Eval("DisplayName") %></td>
                    <td><%# Eval("BindedDomain") %></td>
                    <td>
                        <a href="FrontendLangEdit.aspx?name=<%# Eval("Name") %>"><%= T("Common.Edit") %></a>
                    </td>
                    <td>
                        <a href="#" class="grid-action" data-action="Delete" data-action-param-name="<%# Eval("Name") %>"><%= T("Common.Delete") %></a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>

<sig:Pager runat="server" ID="Pager" CssClass="pager" />