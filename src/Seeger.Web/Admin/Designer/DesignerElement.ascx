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
    Sig.Messages.ShowToobox = '<%= T("Designer.ShowToolbox") %>';
    Sig.Messages.Edit = '<%= T("Common.Edit") %>';
    Sig.Messages.Save = '<%= T("Common.Save") %>';
    Sig.Messages.Remove = '<%= T("Common.Remove") %>';
    Sig.Messages.Saving = '<%= T("Message.Saving") %>';
    Sig.Messages.Loading = '<%= T("Message.Loading") %>';
    Sig.Messages.NoChange = '<%= T("Designer.NoChange") %>';
    Sig.Messages.HighlightZones = '<%= T("Designer.HighlightZones") %>';
    Sig.Messages.CancelHighlightZones = '<%= T("Designer.CancelHighlightZones") %>';
    Sig.Messages.ReloadConfirm_HasUnsavedChanges = '<%= T("Designer.ReloadConfirm_HasUnsavedChanges") %>';
    Sig.Messages.ChangePageCultureConfirm_HasUnsavedChanges = '<%= T("Designer.ChangePageCultureConfirm_HasUnsavedChanges") %>';
    Sig.Messages.CloseConfirm_HasUnsavedChanges = '<%= T("Designer.CloseConfirm_HasUnsavedChanges") %>';
    Sig.Messages.SavedAndReloading = '<%= T("Designer.SavedAndReloading") %>';
</script>
    