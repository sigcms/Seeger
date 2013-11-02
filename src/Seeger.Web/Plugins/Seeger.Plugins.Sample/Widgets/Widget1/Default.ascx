<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Default.ascx.cs" Inherits="Seeger.Plugins.Sample.Widgets.Widget1.Default" %>
<%@ OutputCache Duration="15" VaryByParam="none" VaryByCustom="Site" %>

<div>
    Site: <%= String.IsNullOrEmpty(PageItem.BindedDomains) ? "Root" : PageItem.BindedDomains %>
    <%= DateTime.Now %>
</div>