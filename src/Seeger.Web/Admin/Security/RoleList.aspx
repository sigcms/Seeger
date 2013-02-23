<%@ Page Title="{ Role.List }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="RoleList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Security.RoleList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="mgnt-toolbar">
    <sig:AdminButton runat="server" OnClientClick="location.href='RoleEdit.aspx';return false;" Text="<%$ Resources: Role.Add %>"
         Function="RoleMgnt" Operation="Add" />
</div>

<sig:GridView runat="server" ID="ListGrid">
    <Columns>
        <asp:BoundField HeaderText="<%$ Resources: Role.Name %>" DataField="Name" />
    </Columns>
</sig:GridView>

</asp:Content>
