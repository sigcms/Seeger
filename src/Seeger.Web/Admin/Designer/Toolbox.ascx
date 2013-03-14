<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Toolbox.ascx.cs" Inherits="Seeger.Web.UI.Admin.Designer.Toolbox" %>
<%@ Register Src="Controls/AllWidgets.ascx" TagPrefix="uc" TagName="AllWidgets" %>

<div id="sig-toolbox" style="display:none" class="sig-toolbox">

        <div id="sig-tabs">
            <ul>
                <li><a href="#plugin-widgets-tab"><%= T("Designer.Widgets") %></a></li>
            </ul>
            <div id="plugin-widgets-tab" class="sig-tab-content">
                <uc:AllWidgets runat="server" ID="AllWidgets" />
            </div>
            <div class="sig-actions">
                <asp:PlaceHolder runat="server" ID="SelectLanguageHolder">
                    <span class="sig-buttonset">
                        <asp:DropDownList runat="server" ID="CultureList" DataTextField="DisplayName" DataValueField="Name">
                        </asp:DropDownList>
                    </span>
                </asp:PlaceHolder>
                <span class="sig-buttonset">
                    <a href="javascript:void(0);" id="sig-view-button" class="sig-button sig-trivial" title="<%= T("Page.View") %>">&nbsp;</a>
                    <a href="javascript:void(0);" id="sig-highlight-button" class="sig-button sig-trivial" title="<%= T("Designer.HighlightZones") %>">&nbsp;</a>
                    <a href="javascript:void(0);" id="sig-reload-button" class="sig-button sig-trivial" title="<%= T("Common.Reload") %>">&nbsp;</a>
                </span>
                <span class="sig-buttonset">
                    <button type="button" id="sig-save-button" class="sig-button sig-primary" title="<%= T("Common.Save") %>"><%= T("Common.Save") %></button>
                    <button type="button" id="sig-close-button" class="sig-button sig-secondary" title="<%= T("Designer.CloseDesigner") %>"><%= T("Common.Close") %></button>
                </span>
            </div>
        </div>

       <a id="sig-hide-toolbox-button" href="#" title="<%= T("Designer.HideToolbox") %>"></a>

       <script type="text/javascript">
           $("#<%= CultureList.ClientID %>").change(function () {
               var designer = Sig.Designer.get_current();
               var targetCulture = this.value;

               if (!designer.get_hasChanged() || confirm(Sig.Messages.ChangePageCultureConfirm_HasUnsavedChanges)) {
                   designer.changeCulture(targetCulture);
               } else {
                   this.value = designer.get_context().get_culture();
               }
           });
       </script>

</div>
