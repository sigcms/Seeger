<%@ Page Title="{ Role.List }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="RoleList.aspx.cs" Inherits="Seeger.Web.UI.Admin.Security.RoleList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

<div class="mgnt-toolbar">
    <sig:AdminButton runat="server" OnClientClick="location.href='RoleEdit.aspx';return false;" Text="<%$ Resources: Role.Add %>"
         Function="RoleMgnt" Operation="Add" />
</div>

<div class="ajax-grid">
    <div class="grid-panel"></div>
</div>

<script type="text/javascript">
    $(function () {
        $('.btn-delete').live('click', function () {
            if (!confirm(sig.GlobalResources.get('Message.DeleteConfirm'))) return;

            sig.ui.Message.show(sig.GlobalResources.get('Message.Processing'));

            PageMethods.Delete($(this).attr('item-id'), function () {
                $('.ajax-grid').data('AjaxGrid').refresh();
            }, function (e) {
                sig.ui.Message.error(e.get_message());
            });
        });
    });
</script>

</asp:Content>
