<%@ Page Title="Menu.Logs" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="Logs.aspx.cs" Inherits="Seeger.Web.UI.Admin._System.Logs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

    <div class="mgnt-toolbar">
        <button type="button" class="btn-clear-logs"><%= T("Clear logs") %></button>
    </div>
    <div class="ajax-grid">
        <div class="grid-panel"></div>
    </div>

    <script type="text/javascript">
        $(function () {
            $('.btn-clear-logs').click(function () {
                if (!confirm(sig.Resources.get('Are you sure to clear all logs?'))) return;

                sig.ui.Message.show(sig.Resources.get('Message.Processing'));

                PageMethods.ClearLogs(function () {
                    $('.ajax-grid').data('AjaxGrid').refresh();
                }, function (e) {
                    sig.ui.Message.error(e.get_message());
                });
            });
        });
    </script>

</asp:Content>
