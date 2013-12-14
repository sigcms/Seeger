<%@ Page Title="File Buckets" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Buckets.aspx.cs" Inherits="Seeger.Web.UI.Admin.Files.Buckets" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

    <div class="mgnt-toolbar">
        <button type="button" onclick="location.href='BucketEdit.aspx'">Create Bucket</button>
    </div>
    <div class="ajax-grid">
        <div class="grid-panel"></div>
    </div>

</asp:Content>
