<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DesignerElement.ascx.cs" Inherits="Seeger.Web.UI.Admin.Designer.DesignerElement" %>

<%@ Register Src="ToolBox.ascx" TagName="ToolBox" TagPrefix="uc" %>

<sig:ScriptReference runat="server" Path="/Scripts/jquery.querystring.js" />
<sig:ScriptReference runat="server" Path="/Scripts/jquery.looklesstab.js" />

<uc:ToolBox ID="ToolBox1" runat="server" />

<sig:ScriptReference runat="server" Path="/Scripts/sig.common.js" />
<sig:ScriptReference runat="server" Path="/Scripts/json2.js" />
<sig:ScriptReference runat="server" Path="/Scripts/collections/Base.js" />
<sig:ScriptReference runat="server" Path="/Scripts/collections/LinkedList.js" />

<sig:ScriptReference runat="server" Path="/Scripts/designer/designer.common.js" />
<sig:ScriptReference runat="server" Path="/Scripts/designer/designer.states.js" />
<sig:ScriptReference runat="server" Path="/Scripts/designer/designer.logger.js" />
<sig:ScriptReference runat="server" Path="/Scripts/designer/designer.toolbox.js" />
<sig:ScriptReference runat="server" Path="/Scripts/designer/designer.commands.js" />
<sig:ScriptReference runat="server" Path="/Scripts/designer/designer.context.js" />
<sig:ScriptReference runat="server" Path="/Scripts/designer/designer.editorcontext.js" />
<sig:ScriptReference runat="server" Path="/Scripts/designer/designer.widget.js" />
<sig:ScriptReference runat="server" Path="/Scripts/designer/designer.zone.js" />
<sig:ScriptReference runat="server" Path="/Scripts/designer/designer.services.js" />
<sig:ScriptReference runat="server" Path="/Scripts/designer/designer.js" />

<script type="text/javascript">
    Sig.Designer._pageId = <%= PageId %>;
    Sig.Designer._pageTemplate = '<%= TemplateName %>';
    Sig.Designer._pageLayout = '<%= LayoutName %>';
    Sig.Designer._pageLiveUrl = '<%= PageLiveUrl %>';

    // Init messages
    Sig.Messages.ShowToobox = '<%= Localize("Designer.ShowToolbox") %>';
    Sig.Messages.Edit = '<%= Localize("Common.Edit") %>';
    Sig.Messages.Save = '<%= Localize("Common.Save") %>';
    Sig.Messages.Remove = '<%= Localize("Common.Remove") %>';
    Sig.Messages.Saving = '<%= Localize("Message.Saving") %>';
    Sig.Messages.Loading = '<%= Localize("Message.Loading") %>';
    Sig.Messages.NoChange = '<%= Localize("Designer.NoChange") %>';
    Sig.Messages.HighlightZones = '<%= Localize("Designer.HighlightZones") %>';
    Sig.Messages.CancelHighlightZones = '<%= Localize("Designer.CancelHighlightZones") %>';
    Sig.Messages.ReloadConfirm_HasUnsavedChanges = '<%= Localize("Designer.ReloadConfirm_HasUnsavedChanges") %>';
    Sig.Messages.ChangePageCultureConfirm_HasUnsavedChanges = '<%= Localize("Designer.ChangePageCultureConfirm_HasUnsavedChanges") %>';
    Sig.Messages.CloseConfirm_HasUnsavedChanges = '<%= Localize("Designer.CloseConfirm_HasUnsavedChanges") %>';
    Sig.Messages.SavedAndReloading = '<%= Localize("Designer.SavedAndReloading") %>';
</script>
    