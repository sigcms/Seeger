<%@ Page Title="File Buckets" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Buckets.aspx.cs" Inherits="Seeger.Web.UI.Admin.Files.Buckets" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

    <div class="page-header">
        <h1>
            <a class="btn btn-success" href="BucketEdit.aspx" title="New Bucket"><i class="fa fa-plus fa-2x"></i></a>
            <span>File Buckets</span>
        </h1>
    </div>

    <div class="ajax-grid">
        <div class="grid-panel"></div>
    </div>

</asp:Content>
