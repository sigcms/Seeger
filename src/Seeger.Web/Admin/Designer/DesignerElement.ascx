<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DesignerElement.ascx.cs" Inherits="Seeger.Web.UI.Admin.Designer.DesignerElement" %>

<%@ Register Src="ToolBox.ascx" TagName="ToolBox" TagPrefix="uc" %>

<sig:ScriptReference runat="server" Path="/Scripts/jquery.querystring.js" />
<sig:ScriptReference runat="server" Path="/Scripts/jquery.looklesstab.js" />

<uc:ToolBox ID="ToolBox1" runat="server" />

<sig:ScriptReference runat="server" Path="/Scripts/sig.common.js" />
<sig:ScriptReference runat="server" Path="/Scripts/json2.js" />
<sig:ScriptReference runat="server" Path="/Scripts/collections/Base.js" />
<sig:ScriptReference runat="server" Path="/Scripts/collections/LinkedList.js" />

<sig:ScriptReference runat="server" Path="/Scripts/sig.core.js" />
<sig:ScriptReference runat="server" Path="/Scripts/Resources.ashx" />

<sig:ScriptReference runat="server" Path="/Scripts/designer/designer.common.js" />
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
    Sig.Messages.ShowToobox = sig.Resources.get('Designer.ShowToolbox');
    Sig.Messages.Edit = sig.Resources.get('Common.Edit');
    Sig.Messages.Save = sig.Resources.get('Common.Save');
    Sig.Messages.Remove = sig.Resources.get('Common.Remove');
    Sig.Messages.Saving = sig.Resources.get('Message.Saving');
    Sig.Messages.Loading = sig.Resources.get('Message.Loading');
    Sig.Messages.NoChange = sig.Resources.get('Designer.NoChange');
    Sig.Messages.HighlightZones = sig.Resources.get('Designer.HighlightZones');
    Sig.Messages.CancelHighlightZones = sig.Resources.get('Designer.CancelHighlightZones');
    Sig.Messages.ReloadConfirm_HasUnsavedChanges = sig.Resources.get('Designer.ReloadConfirm_HasUnsavedChanges');
    Sig.Messages.ChangePageCultureConfirm_HasUnsavedChanges = sig.Resources.get('Designer.ChangePageCultureConfirm_HasUnsavedChanges');
    Sig.Messages.CloseConfirm_HasUnsavedChanges = sig.Resources.get('Designer.CloseConfirm_HasUnsavedChanges');
    Sig.Messages.SavedAndReloading = sig.Resources.get('Designer.SavedAndReloading');
</script>
    