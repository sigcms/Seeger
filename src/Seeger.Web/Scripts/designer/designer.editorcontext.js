(function ($) {

    function EditorContext(widget, zone, culture, isEditing) {
        /// <summary>Provides informations for the widget editor.</summary>
        /// <param name='onAccept'>Signature: function(editorContext) { }</param>
        /// <param name='onCancel'>Signature: function(editorContext) { }</param>
        var _widget = widget;
        var _zone = zone;
        var _culture = culture;
        var _isEditing = isEditing;
        var _stateItem = null;
        var _this = this;
        var _attrManager = new AttributeManager(_widget, _this);

        if (_zone === UNDEFINED_VALUE || _zone == null) {
            _zone = _widget.get_zone();
        }
        if (_zone == null) {
            throw new Error("Zone is required.");
        }

        this.get_isEditing = function () { return _isEditing; }
        this.get_widget = function () { return _widget; }
        this.get_culture = function () { return _culture; }
        this.closeEditor = function () {
            Sig.Window.close("WidgetEditorDialog");
        }
        this.cancel = function () {
            // TODO: If editing an existing widget, change stateItem, cancel() will not work correctly
            _this.closeEditor();
        }
        this.accept = function () {
            if (_stateItem != null) {
                _this.get_stateManager().addItem(_stateItem);
            }
            if (_widget.get_id() == null) {
                _widget.set_id(_this.get_stateItem().get_id());
            }

            _this.closeEditor();
            EditorContext.set_current(null);

            if (_widget.get_zone() == null) {
                _zone.addWidget(_widget);
            }
            _widget.resizeOverlay();
        }
        this.get_designer = function () {
            return Sig.Designer.get_current();
        }
        this.get_stateManager = function () {
            return _this.get_designer().get_stateManager();
        }
        this.get_attributeManager = function () {
            return _attrManager;
        }
        this.get_stateItem = function () {
            if (_stateItem == null) {
                // First Try: to check whether the stateItem already exists
                if (_widget.get_id() != null) {
                    _stateItem = _this.get_stateManager().getItemById(_widget.get_id());
                }

                // If null, that means the stateItem hasn't been created
                if (_stateItem == null) {
                    _stateItem = _this.get_stateManager().createItem(_widget.get_name(), _zone.get_name(), _widget.get_templateName(), _widget.get_pluginName());

                    _stateItem.set_order(_widget.get_order());

                    if (_widget.get_isNew()) {
                        _stateItem.markAdded();
                    } else {
                        _stateItem.markChanged();
                    }
                }

                // If _widget has 'widget-in-page-id assigned, assign the 'widget-in-page-id' to the _stateItem
                if (_widget.get_id() != null) {
                    _stateItem.set_id(_widget.get_id());
                }
            }
            return _stateItem;
        }
        this.stateItemExistsInStateManager = function () {
            if (_widget.get_id() != null) {
                return _this.get_stateManager().existItem(_widget.get_id());
            }

            return false;
        }

        function AttributeManager(widget, editorContext) {
            var _widget = widget;
            var _editorContext = editorContext;
            var _this = this;

            this.get_attributes = function () {
                return _widget.get_attributes();
            }
            this.set_attributes = function (attrs) {
                _widget.set_attributes(attrs);
                _editorContext.get_stateItem().set_attributes(attrs);
            }
            this.containsKey = function (key) {
                return _this.getAttribute(key) != null;
            }
            this.getAttribute = function (key) {
                var attr = _this.get_attributes()[key];
                if (attr == UNDEFINED_VALUE) {
                    return null;
                }
                return attr;
            }
            this.getValue = function (key, defaultValue) {
                var attr = _this.getAttribute(key);
                if (attr == null && defaultValue != undefined) {
                    return defaultValue;
                }
                return attr == null ? null : attr.value;
            }
            this.setAttribute = function (key, value) {
                if (key === UNDEFINED_VALUE || key == null || key.length == 0) {
                    throw new Error("'key' is required.");
                }
                if (value === UNDEFINED_VALUE) {
                    throw new Error("'value' cannot be undefined.");
                }

                var attr = _this.getAttribute(key);
                if (attr != null) {
                    attr.value = value;
                } else {
                    attr = { key: key, value: value };
                    _widget.setAttribute(key, attr);
                    _editorContext.get_stateItem().get_attributes()[key] = attr;
                }
            }
            this.setBoolean = function (key, value) {
                _this.setAttribute(key, value);
            }
            this.setString = function (key, value) {
                _this.setAttribute(key, value);
            }
            this.setInt32 = function (key, value) {
                _this.setAttribute(key, value);
            }
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