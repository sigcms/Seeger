var Editor = {
    init: function () {
        var widget = editorContext.widget();
        var contentId = widget.attribute('ContentId');

        // Bind form
        if (widget.customData.hasLoaded) {
            Editor.updateForm({
                Title: widget.customData.title,
                Body: widget.customData.content
            });
        } else if (contentId) {
            Editor.loadContent(contentId, function (content) {
                Editor.updateForm(content);

                widget.customData.hasLoaded = true;
                widget.customData.title = content.Title;
                widget.customData.content = content.Body;
            });
        }

        // Load existing contents
        PageMethods.LoadExistingContents(editorContext.culture(), function (contents) {
            if (contents.length > 0) {
                var html = '';
                $.each(contents, function () {
                    html += '<option value="' + this.Id + '">' + this.Title + '</option>';
                });

                var $dropdown = $('.existing-contents-dropdown');
                $dropdown.append(html);
                $('.existing-contents-panel').show();
            }
        });

        $('.btn-add-existing').click(function () {
            var mode = $(this).data('add-mode');
            var contentId = $('.existing-contents-dropdown').val();
            if (contentId) {
                Editor.loadContent(contentId, function (content) {
                    Editor.updateForm(content);

                    widget.customData.hasLoaded = true;
                    widget.customData.title = content.Title;
                    widget.customData.content = content.Body;

                    if (mode == 'shared') {
                        widget.attribute('ContentId', contentId);
                    } else {
                        widget.markDirty();
                    }
                });
            }
        });
    },

    updateForm: function (content) {
        $('#ContentTitle').val(content.Title);
        $('#ContentBody').val(content.Body);

        var editor = tinymce.get('ContentBody');
        if (editor) {
            editor.setContent(content.Body);
        }
    },

    loadContent: function (contentId, callback) {
        PageMethods.LoadContent(contentId, editorContext.culture(), function (content) {
            if (callback) callback(content);
        });
    },

    submit: function () {
        var content = tinymce.get('ContentBody').getContent();
        var widget = editorContext.widget();
        widget.customData.title = $.trim($('#ContentTitle').val());
        widget.customData.content = content;
        widget.markDirty();

        Editor.updateDesigner(widget);
        editorContext.accept();
    },

    updateDesigner: function (widget) {
        var $element = widget.$element();
        var text = widget.customData.title || widget.customData.content;
        $element.find(".sig-richtext-widget-body").html(text);
    }
};

$(function () {
    Editor.init();
});