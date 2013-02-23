<%@ Page Title="{ CustomRedirect.List }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="CustomRedirectList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Urls.CustomRedirectList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="mgnt-toolbar">
    <sig:AdminButton runat="server" ID="AddButton" UseSubmitBehavior="false" 
         OnClientClick="location.href='CustomRedirectEdit.aspx';return false;" 
         Text="<%$ Resources: Common.Add %>"
         Function="CustomRedirect"
         Operation="Add" />
</div>

<sig:GridView runat="server" ID="ListGrid" AllowPaging="false">
    <Columns>
        <asp:TemplateField HeaderText="<%$ Resources: CustomRedirect.RedirectMode %>" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <%# Localize("RedirectMode." + Eval("RedirectMode").ToString()) %>
            </ItemTemplate>        
        </asp:TemplateField>
        <asp:BoundField HeaderText="<%$ Resources: CustomRedirect.From %>" DataField="From" />
        <asp:BoundField HeaderText="<%$ Resources: CustomRedirect.To %>" DataField="To" />
        <asp:CheckBoxField HeaderText="<%$ Resources: CustomRedirect.MatchByRegex %>" DataField="MatchByRegex" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
    </Columns>
</sig:GridView>

</asp:Content>
