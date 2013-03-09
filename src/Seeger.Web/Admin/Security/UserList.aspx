<%@ Page Title="{ User.List }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Security.UserList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="mgnt-toolbar">
    <sig:AdminButton runat="server" OnClientClick="location.href='UserEdit.aspx';return false;" Text="<%$ Resources: User.Add %>" Function="UserMgnt" Operation="Add" />
</div>

<sig:GridView runat="server" ID="ListGrid">
    <Columns>
        <asp:BoundField HeaderText="<%$ Resources: User.UserName %>" DataField="UserName" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField HeaderText="<%$ Resources: User.Nick %>" DataField="Nick" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField HeaderText="<%$ Resources: User.Email %>" DataField="Email" ItemStyle-HorizontalAlign="Center" />
    </Columns>
</sig:GridView>

</asp:Content>
