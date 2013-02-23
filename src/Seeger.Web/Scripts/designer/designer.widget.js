(function ($) {

    function Widget($element) {
        /// <summary>Represents a widget in the designer.</summary>
        /// <param name='$element'>The jQuery object that represents the widget DOM.</param>

        // Private Fields
        var _$element = $element;
        var _id = 0;
        var _name = null;
        var _order = 0;
        var _pluginName = null;
        var _templateName = null;
        var _editable = false;
        var _editorWidth = 0;
        var _editorHeight = 0;
        var _overlay = null;
        var _zone = null;
        var _inited = false;
        var _this = this;

        // Public APIs
        this.get_$element = function () { return _$element; }
        this.get_zone = function () { return _zone; }
        this.get_zoneName = function () {
            if (_zone == null) {
                return null;
            }
            return _zone.get_name();
        }
        this.set_zone = function (value) {
            if (value == UNDEFINED_VALUE) throw new Error("'value' cannot be undefined.");
            if (!(value instanceof Sig.Zone)) throw new Error("'value' should be of type Sig.Zone.");
            _zone = value;
        }
        this.get_inited = function () { return _inited; }
        this.get_attachedToZone = function () { return _zone != null; }
        this.get_id = function () { return _id; }
        this.set_id = function (value) {
            if (value == UNDEFINED_VALUE || value == null || value == "") {
                throw new Error("'value' cannot be undefined, null or empty.");
            }
            _id = value;
            _$element.attr("widget-in-page-id", value);
        }
        this.get_name = function () { return _name; }
        this.get_order = function () { return _order; }
        this.set_order = function (value) {
            if (isNaN(value)) {
                throw new Error("'value' should be a integer.");
            }
            _order = value;
            _$element.attr("order", value);
        }
        this.get_pluginName = function () { return _pluginName; }
        this.get_templateName = function () { return _templateName; }
        this.get_editable = function () { return _editable; }
        this.get_width = function () { return _$element.outerWidth(); }
        this.get_height = function () { return _$element.outerHeight(); }
        this.get_attributes = function () {
            var attrs = _$element.data("attributes");
            if (attrs == UNDEFINED_VALUE) {
                return {};
            }
            return attrs;
        }
        this.set_attributes = function (attrs) {
            _$element.data("attributes", attrs);
        }
        this.getAttribute = function (key) {
            var val = _this.get_attributes[key];
            if (val == UNDEFINED_VALUE) return null;
            return val;
        }
        this.setAttribute = function (key, value) {
            var attrs = _this.get_attributes();
            attrs[key] = value;
            _this.set_attributes(attrs);
        }
        this.get_isNew = function () { return _id == null || _id.indexOf("new") >= 0; }
        this.init = function () {
            _initOverlay();
            _initHoverEffects();

            _inited = true;
        }
        this.ensureInited = function () {
            if (!_inited) {
                _this.init();
            }
        }
        this.reinitOverlay = function () {
            _initHoverEffects();
            _initOverlay();
            _overlay.ajustSize();
        }
        this.resizeOverlay = function () {
            _this.ensureInited();
            _overlay.ajustSize();
        }
        this.getEditorUrl = function (pageId, culture, containingZone) {
            var designerContext = Sig.Designer.get_current().get_context();

            var cul = culture;
            if (culture === UNDEFINED_VALUE) {
                cul = designerContext.get_culture();
            }

            if (!containingZone) {
                containingZone = _zone;
            }
            if (containingZone == null) {
                throw new Error("Zone is required.");
            }

            var path = Sig.UrlUtility.combine(designerContext.get_installPath(), "/Plugins/" + _pluginName);

            if (_templateName != null) {
                path += "/" + _templateName;
            }

            path += "/Widgets/" + _name + "/Editor.aspx";

            var query = "?pageid=" + pageId
                + "&widgetName=" + _name
                + "&widgetPlugin=" + _pluginName
                + "&widgetTemplate=" + (_templateName == null ? "" : _templateName)
                + "&zoneName=" + containingZone.get_name()
                + "&widgetInPageId=" + (_this.get_isNew() ? "0" : _id)
                + "&culture=" + cul;

            return path + query;
        }
        this.openEditor = function (containingZone, editing, width, height) {
            if (editing === UNDEFINED_VALUE) {
                editing = true;
            }

            var designerContext = Sig.Designer.get_current().get_context();
            var culture = designerContext.get_culture();
            var pageId = designerContext.get_pageId();

            var widget = this;

            // Setup EditorContext
            var editorContext = new Sig.EditorContext(widget, containingZone, culture, editing);
            Sig.EditorContext.set_current(editorContext);

            Sig.Window.open("WidgetEditorDialog", {
                title: Sig.Messages.Edit,
                url: _this.getEditorUrl(pageId, culture, containingZone),
                width: width ? width : _editorWidth,
                height: height ? height : _editorHeight,
                zIndex: 30000,
                close: function () {
                    // TODO: Do something. But note that 'close' will be call in EditorContext.accept()
                }
            });
        }

        // Private Methods
        function _initOverlay() {
            if (_overlay == null) {
                _overlay = WidgetOverlay.create();
                _overlay.set_widget(_this);
            }
            _overlay.rebindCommands();
        }

        function _initHoverEffects() {
            _$element.unbind("hover");
            _$element.hover(function () { $(this).addClass('hover'); }, function () { $(this).removeClass('hover'); });
        }

        // Init
        parse(_$element);

        function parse($element) {
            if (!$element.hasClass("sig-widget")) {
                throw new Error("Not a valid widget dom element. Expected CSS class 'sig-widget'.");
            }
            _id = Widget.parseWidgetId($element);
            _name = $element.attr("widget-name");

            _templateName = $element.attr("template-name");
            if (_templateName.length == 0) _templateName = null;

            _pluginName = $element.attr("plugin-name");
            if (_pluginName.length == 0)
                throw new Error("'plugin-name' attribute is missing.");

            _editable = $element.attr("editable") == "true";
            _editorWidth = parseInt($element.attr("editor-width"), 10);
            _editorHeight = parseInt($element.attr("editor-height"), 10);

            if (_editorWidth <= 0) {
                _editorWidth = .95;
            }
            if (_editorHeight <= 0) {
                _editorHeight = .95;
            }

            _order = $element.attr("order");
            if (_order !== undefined) {
                _order = parseInt($element.attr("order"), 10);
            } else {
                _order = 0;
            }
        }
    }

    Widget.parseWidgetId = function ($element) {
        var id = $element.attr("widget-in-page-id");
        if (id == UNDEFINED_VALUE || id == "") {
            id = null;
        }
        return id;
    };

    function WidgetOverlay($element, widget) {
        /// <summary>Widget Overlay</summary>
        /// <param name='$element'>The jQuery object that represents the overlay DOM.</param>
        /// <param name='widget'>The widget that the overlay belongs to.</param>

        var _$element = $element;
        var _widget = widget ? widget : null;
        var _$editButton = _$element.find(".sig-edit-widget-button:first");
        var _$removeButton = _$element.find(".sig-remove-widget-button:first");
        var _this = this;

        this.get_$element = function () { return _$element; }
        this.get_widget = function () { return _widget; }
        this.set_widget = function (widget) {
            _widget = widget;
            _$element.appendTo(_widget.get_$element());
            _this.ajustSize();
            _this.rebindCommands();
        }
        this.rebindCommands = function () {
            _bindCommandsToButtons();
        }
        this.ajustSize = function () {
            if (!_widget) {
                throw new Error("'widget' must be set before ajustment.");
            }
            _$element.width(_widget.get_width());
            _$element.height(_widget.get_height());
        }

        function _bindCommandsToButtons() {
            if (_widget.get_editable()) {
                _$editButton.unbind("click");
                _$editButton.click(function () {
                    Sig.Designer.get_current().executeCommand(Sig.CommandNames.Widget_Edit, _widget);
                });
            } else {
                _$editButton.hide();
            }

            _$removeButton.unbind("click");
            _$removeButton.click(function () {
                Sig.Designer.get_current().executeCommand(Sig.CommandNames.Widget_Remove, _widget);
            });
        }
    }

    WidgetOverlay.create = function () {
        return new WidgetOverlay($("<div class='sig-widget-overlay'><div class='sig-widget-toolbar'>"
                + createLink("sig-edit-widget-button", Sig.Messages.Edit)
                + createLink("sig-remove-widget-button", Sig.Messages.Remove)
                + "</div></div>"));

        function createLink(className, text) {
            return "<a href='javascript:void(0)' class='" + className + "' title='" + text + "'>" + text + "</a>";
        }
    };

    var WidgetInitializer = {
        registerCommands: function (commandManager, designer) {
            // Register Edit command
            commandManager.register(Sig.CommandNames.Widget_Edit, function (widget) {
                var context = designer.get_context();
                widget.openEditor(widget.get_zone(), true);
            });
            // Register Remove command
            commandManager.register(Sig.CommandNames.Widget_Remove, function (widget) {
                widget.get_zone().removeWidgetById(widget.get_id());
            });
        }
    };

    // Public APIs
    window.Sig.Widget = Widget;
    window.Sig.WidgetOverlay = WidgetOverlay;
    window.Sig.WidgetInitializer = WidgetInitializer;

})(jQuery);