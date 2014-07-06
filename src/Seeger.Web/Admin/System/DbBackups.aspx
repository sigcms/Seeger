<%@ Page Title="Database Backup" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="DbBackups.aspx.cs" Inherits="Seeger.Web.UI.Admin._System.DbBackups" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

    <div class="page-header">
        <h1>
            <button type="button" class="btn btn-success btn-new-backup" title="<%= T("Create new database backup") %>"><i class="fa fa-2x fa-plus"></i></button>
            <span><%= T("Database Backup") %></span>
        </h1>
    </div>

    <div class="ajax-grid">
        <div class="grid-panel"></div>
    </div>

    <script>
        $(function () {
            $('.btn-new-backup').click(function () {
                if (!confirm(sig.Resources.get('Are you sure to create a new backup?'))) return false;

                var $button = $(this);

                sig.ui.Message.show(sig.Resources.get('Message.Processing'));

                $button.attr('disabled', 'disabled');

                PageMethods.NewBackup(function () {
                    sig.ui.Message.show(sig.Resources.get('Database backup completed successfully!'));
                    $button.removeAttr('disabled');
                    setTimeout(function () {
                        $('.ajax-grid').data('AjaxGrid').refresh();
                    }, 500);
                }, function (e) {
                    $button.removeAttr('disabled');
                    sig.ui.Message.error(e.get_message());
                });

                return false;
            });
        });
    </script>

</asp:Content>
