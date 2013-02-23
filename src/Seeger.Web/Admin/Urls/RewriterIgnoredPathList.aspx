<%@ Page Title="{ RewriterIgnoredPath.List }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="RewriterIgnoredPathList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Urls.ReservedPathList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="mgnt-toolbar">
    <sig:AdminButton runat="server" ID="AddButton" UseSubmitBehavior="false"
         OnClientClick="location.href='RewriterIgnoredPathEdit.aspx';return false;"
         Text="<%$ Resources: Common.Add %>"
         Function="RewriterIgnoredPath"
         Operation="Add" />
</div>

<sig:GridView runat="server" ID="ListGrid" AllowPaging="false">
    <Columns>
        <asp:BoundField HeaderText="<%$ Resources: RewriterIgnoredPath.Name %>" DataField="Name" />
        <asp:BoundField HeaderText="<%$ Resources: RewriterIgnoredPath.Path %>" DataField="Path" />
        <asp:CheckBoxField HeaderText="<%$ Resources: RewriterIgnoredPath.MatchByRegex %>" DataField="MatchByRegex" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100" />
    </Columns>
</sig:GridView>

</asp:Content>
