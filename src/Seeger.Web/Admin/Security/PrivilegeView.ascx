<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrivilegeView.ascx.cs"
    Inherits="Seeger.Web.UI.Admin.Security.PrivilegeView" %>
<div id='<%= ClientID %>'>
    <asp:Repeater runat="server" ID="FunctionRepeater" OnItemDataBound="FunctionRepeater_ItemDataBound">
        <ItemTemplate>
            <fieldset class="mgnt-role-function">
                <legend>
                    <%# Eval("DisplayName") %>
                </legend>
                <div class="mgnt-role-oplist">
                    <asp:CheckBoxList ID="Operations" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" />
                </div>
            </fieldset>
        </ItemTemplate>
    </asp:Repeater>
</div>

<script type="text/javascript">
    (function ($) {

        var $operationGroups = $("#<%= ClientID %> .mgnt-role-oplist");

        $(function () {
            $operationGroups.each(function () {
                setupOperationGroupBehavior($(this));
            });
        });

        $("#<%= ClientID %> .mgnt-role-oplist > span input[type=checkbox]").click(function () {
            setupOperationGroupBehavior($(this).closest(".mgnt-role-oplist"));
        });

        function setupOperationGroupBehavior($operationGroup) {
            var $items = $operationGroup.find("> span > span");

            // $item disabled => because it's not assignable
            // $checkbox disabled => because a higher weighted privilege is checked
            var rightMostCheckedItemWeight = -1;
            for (var len = $items.length, i = len - 1; i >= 0; --i) {
                var $item = $($items[i]);
                var $checkbox = $item.find("input[type=checkbox]");
                if (rightMostCheckedItemWeight == -1 && !$item.attr("disabled")) {
                    $checkbox.removeAttr("disabled");
                    if ($checkbox.is(":checked")) {
                        rightMostCheckedItemWeight = parseInt($item.attr("weight"), 10);
                    }
                } else {
                    var weight = parseInt($item.attr("weight"), 10);
                    if (weight < rightMostCheckedItemWeight) {
                        $checkbox.attr("checked", "checked").attr("disabled", "disabled");
                    }
                }
            }
        }

    })(jQuery);
</script>
