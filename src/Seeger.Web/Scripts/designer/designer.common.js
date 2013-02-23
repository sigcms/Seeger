(function ($) {
    var CommandNames = {
        Save: "Save",
        Reload: "Reload",
        View: "View",
        Widget_Edit: "Widget_Edit",
        Widget_Remove: "Widget_Remove"
    };

    window.Sig.CommandNames = CommandNames;

    // Selectors
    var Selectors = {
        Toolbox: "#sig-toolbox",
        Toobox_HideButton: "#sig-hide-toolbox-button",
        Toolbox_SaveButton: "#sig-save-button",
        Toolbox_ReloadButton: "#sig-reload-button",
        Toolbox_ViewButton: "#sig-view-button",
        Toolbox_CloseButton: "#sig-close-button",
        Toolbox_HighlightButton: "#sig-highlight-button",
        Toolbox_Tabs: "#sig-tabs"
    };

    window.Sig.Selectors = Selectors;

    // Messages
    var Messages = {
        CancelHighlightZones: "Cancel hightlight zones",
        ChangePageCultureConfirm_HasUnsavedChanges: "The page has unsaved changes, are you sure to change culture without saving?",
        CloseConfirm_HasUnsavedChanges: "The page has been modified, are you sure to close the page?",
        Edit: "Edit",
        HighlightZones: "Hightlight Zones",
        Loading: "Loading",
        Remove: "Remove",
        ReloadConfirm_HasUnsavedChanges: "Are you sure to reload the page?",
        Save: "Save",
        Saving: "Saving",
        SavedAndReloading: "Save success. Now reloading...",
        ShowToobox: "Show Toolbox"
    };

    window.Sig.Messages = Messages;

})(jQuery);