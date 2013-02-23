var Editor = {
    init: function () {
        if (!editorContext.get_isEditing()) return;

        var customData = editorContext.get_stateItem().get_customData();

        if (customData == undefined || customData.notFirstLoad == undefined) return;

        contentId = customData.contentId;
        $("#" + domId_Content).val(customData.content);
    },

    submit: function () {
        var content = tinyMCE.get(domId_Content).getContent();

        editorContext.get_stateItem().set_customData({ notFirstLoad: true, contentId: contentId, name: '', content: content });

        Editor.updateDesigner(editorContext.get_widget().get_$element(), content);
        editorContext.accept();
    },

    updateDesigner: function ($widget, content) {
        $widget.find(".sig-richtext-widget-body").html(content);
    }
};

Editor.init();