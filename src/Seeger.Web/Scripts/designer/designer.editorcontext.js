(function ($) {

    function EditorContext(widget, zone, culture, isEditing) {
        /// <summary>Provides informations for the widget editor.</summary>
        /// <param name='onAccept'>Signature: function(editorContext) { }</param>
        /// <param name='onCancel'>Signature: function(editorContext) { }</param>
        var _widget = widget;
        var _zone = zone;
        var _culture = culture;
        var _isEditing = isEditing;
        var _this = this;

        if (_zone === UNDEFINED_VALUE || _zone == null) {
            _zone = _widget.get_zone();
        }
        if (_zone == null) {
            throw new Error("Zone is required.");
        }

        this.isEditing = function () {
            return _isEditing;
        }

        this.widget = function () {
            return _widget;
        }

        this.culture = function () {
            return _culture;
        }

        this.closeEditor = function () {
            Sig.Window.close("WidgetEditorDialog");
        }

        this.cancel = function () {
            // TODO: Revert attribute/custom data changes
            _this.closeEditor();
        }

        this.accept = function () {
            _this.closeEditor();
            EditorContext.set_current(null);

            if (_widget.get_zone() == null) {
                _zone.addWidget(_widget);
            }
            _widget.resizeOverlay();
        }

        this.designer = function () {
            return Sig.Designer.get_current();
        }
    }

    // Factory methods

    EditorContext.get_current = function () {
        return window._currentEditorContext;
    }
    EditorContext.set_current = function (editorContext) {
        window._currentEditorContext = editorContext;
    }

    window.Sig.EditorContext = EditorContext;

})(jQuery);