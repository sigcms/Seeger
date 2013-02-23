<%@ Page Title="{ Globalization.FrontendLanguageList }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="FrontendLangList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Settings.FrontendLangList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="mgnt-toolbar">
    <button type="button" onclick="location.href='FrontendLangEdit.aspx'"><%= Localize("Common.Add") %></button>
</div>

<sig:GridView runat="server" ID="Grid" AllowPaging="false">
    <Columns>
        <asp:BoundField HeaderText="<%$ Resources: Globalization.LanguageName %>" DataField="Name" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField HeaderText="<%$ Resources: Globalization.LanguageDisplayName %>" DataField="DisplayName" ItemStyle-HorizontalAlign="Center" />
        <asp:TemplateField HeaderText="<%$ Resources: Globalization.BindedDomain %>" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <%# GetBindedDomainCellHtml(Container.DataItem) %>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</sig:GridView>

</asp:Content>
