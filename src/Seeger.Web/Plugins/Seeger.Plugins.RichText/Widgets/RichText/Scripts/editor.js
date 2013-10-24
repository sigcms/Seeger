var Editor = {
    init: function () {
        var widget = editorContext.widget();

        if (widget.customData.content !== null && widget.customData.content !== undefined) {
            $('#' + tinyEditorId).val(widget.customData.content);
        } else {
            var contentId = widget.attribute('ContentId');

            if (contentId) {
                PageMethods.LoadContent(contentId, editorContext.culture(), function (html) {
                    $('#' + tinyEditorId).val(html);
                    widget.customData.content = html;
                    widget.customData.isContentChanged = false;
                });
            }
        }
    },

    submit: function () {
        var content = tinyMCE.get(tinyEditorId).getContent();
        var widget = editorContext.widget();
        widget.customData.isContentChanged = true;
        widget.customData.content = content;
        widget.markDirty();

        Editor.updateDesigner(widget.$element(), content);
        editorContext.accept();
    },

    updateDesigner: function ($widget, content) {
        $widget.find(".sig-richtext-widget-body").html(content);
    }
};

Editor.init();