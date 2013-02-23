(function ($) {
    var ToolboxCommandNames = {
        ExpandToolbox: "Toolbox_ExpandToolbox",
        CollapseToolbox: "Toolbox_CollapseToolbox"
    };

    function Toolbox(designer) {
        var _$element = null;
        var _height = 0;
        var _$showButton = null;
        var _designer = designer;
        var _this = this;

        if (designer === UNDEFINED_VALUE) throw new ArgumentUndefinedError("designer");
        if (designer == null) throw new ArgumentNullError("designer");

        this.hide = function () {
            _$element.hide();
            _$showButton.show();

            document.body.style.paddingTop = 0;
        }
        this.show = function () {
            _$element.show();
            _$showButton.hide();

            document.body.style.paddingTop = _$element.height() + "px";
        }
        this.init = function ($element) {
            /// <summary>Initialize the toolbox.</summary>

            _$element = $element;

            initShowToolboxButton();

            // Bind Command to Buttons
            _$element.find(Sig.Selectors.Toobox_HideButton).click(function () { _this.hide(); return false; });
            _$element.find(Sig.Selectors.Toolbox_ViewButton).click(function () { _designer.viewCurrentPage(); return false; });
            _$element.find(Sig.Selectors.Toolbox_ReloadButton).click(function () {
                if (!_designer.get_hasChanged() || confirm(Sig.Messages.ReloadConfirm_HasUnsavedChanges)) {
                    _designer.reload();
                }
                return false;
            });
            _$element.find(Sig.Selectors.Toolbox_CloseButton).click(function () {
                if (!_designer.get_hasChanged() || confirm(Sig.Messages.CloseConfirm_HasUnsavedChanges)) {
                    window.close();
                }
                return false;
            });
            _$element.find(Sig.Selectors.Toolbox_SaveButton).click(function () {
                if (_designer.get_hasChanged()) {
                    var $button = $(this);
                    $button.attr("disabled", "disabled").text(Sig.Messages.Saving + "...");
                    _designer.saveChanges({
                        success: function () {
                            var url = window.location.href;
                            var indexOfSharp = url.indexOf("#");
                            if (indexOfSharp > 0) {
                                url = url.substring(0, indexOfSharp);
                            }

                            $button.text(Sig.Messages.Save);

                            Sig.Message.show(Sig.Messages.SavedAndReloading, { duration: 0 });
                            setTimeout(function () { window.location.href = url; }, 1500);
                        },
                        error: function (result) {
                            $button.removeAttr("disabled").text(Sig.Messages.Save);
                            alert(result.Message);
                        }
                    });
                } else {
                    Sig.Window.alert(Sig.Messages.NoChange);
                }

                return false;
            });
            _$element.find(Sig.Selectors.Toolbox_HighlightButton).click(function () {
                var $this = $(this);
                if ($this.hasClass("sig-active")) {
                    $(".sig-zone").removeClass("sig-highlighted");
                    $this.removeClass("sig-active").attr("title", Sig.Messages.HighlightZones);
                } else {
                    $this.addClass("sig-active").attr("title", Sig.Messages.CancelHighlightZones);
                    $(".sig-zone").addClass("sig-highlighted");
                }

                $this.blur();
            });

            setTimeout(function () { _this.show(); }, 100);

            // Make Tabs
            _$element.find(Sig.Selectors.Toolbox_Tabs + ":first").tabs();

            // Make Widget Items Draggable
            _$element.find("div.sig-widget-item").draggable({
                helper: 'clone',
                revert: 'invalid',
                appendTo: 'body',
                convertToSortable: '.sig-zone'
            });
        }

        function initShowToolboxButton() {
            _$showButton = $("<a href='#' class='sig-show-toolbox-button' style='display:none' title='" + Sig.Messages.ShowToobox + "'>&nbsp;</a>");
            _$showButton.appendTo(document.body);

            _$showButton.click(function () {
                _this.show();
                return false;
            });
        }
    }

    function WidgetToolboxItem($element) {
        if ($element == UNDEFINED_VALUE) {
            throw new Error("'$element' cannot be undefined.");
        }
        if ($element == null) {
            throw new Error("'$element' cannot be null.");
        }

        var _$element = $element;
        var _widgetName = null;
        var _templateName = null;
        var _pluginName = null;
        var _autoOpen = false;
        var _this = this;

        this.get_widgetName = function () { return _widgetName; }
        this.get_templateName = function () { return _templateName; }
        this.get_autoOpen = function () { return _autoOpen; }
        this.applyToZone = function (zone) {

            Sig.Message.show(Sig.Messages.Loading + "...", { identity: 'LoadingMessage', duration: -1 });

            Sig.DesignerService.requestWidgetDesigner(_widgetName, _pluginName, _templateName, function (result) {

                Sig.Message.hide('LoadingMessage');

                var widget = new Sig.Widget($(result));
                if (_autoOpen) {
                    var designerContext = Sig.Designer.get_current().get_context();
                    widget.openEditor(zone, false);
                } else {
                    zone.addWidget(widget);
                    widget.resizeOverlay();
                }
            });
        }

        function _parse() {
            _widgetName = _$element.attr("widget-name");
            _templateName = _$element.attr("template-name");
            _pluginName = _$element.attr("plugin-name");
            if (_templateName == UNDEFINED_VALUE) {
                _templateName = null;
            }
            var t = _$element.attr("auto-open");
            if (t != UNDEFINED_VALUE) {
                _autoOpen = (t == '1' || t == 'true' || t == 'True');
            }
        }

        // Init
        _parse();
    }

    window.Sig.Toolbox = Toolbox;
    window.Sig.ToolboxCommandNames = ToolboxCommandNames;
    window.Sig.WidgetToolboxItem = WidgetToolboxItem;

})(jQuery);