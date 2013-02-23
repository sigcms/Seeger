(function ($) {
    function Zone($element) {
        // Private Fields
        var _$element = $element;
        var _name = null;
        var _widgets = null;
        var _this = this;
        var _sortContext = null;

        // Public APIs
        this.init = function () {
            // Init widgets
            _widgets.initAllWidgets();

            // Make droppable
            _$element.droppable({
                accept: ".sig-widget-item",
                drop: function (event, ui) {
                    var widgetItem = new Sig.WidgetToolboxItem(ui.draggable);
                    widgetItem.applyToZone(_this);
                }
            }).sortable({
                forcePlaceholderSize: true,
                placeholder: "sig-sortable-placeholder",
                connectWith: ".sig-zone",
                start: function (event, ui) {
                    _sortContext = {
                        sourceZoneName: _name,
                        destinationZoneName: null
                    }
                },
                update: function (event, ui) {
                    // 'update' callback will be invoked twice when sort cross zones
                    //  _sortContext == null means that the 'update' callback has already been invoked, so stop executing
                    if (_sortContext == null) return;

                    var designer = Sig.Designer.get_current();
                    var newZone = designer.findZoneByName(_sortContext.destinationZoneName);
                    if (!newZone) {
                        throw new Error("New zone was not found: " + _sortContext.destinationZoneName);
                    }
                    var draggedWidget = _widgets.getWidgetById(Sig.Widget.parseWidgetId(ui.item));
                    var $prev = ui.item.prev();
                    var refWidget = null;
                    if ($prev.length > 0) {
                        refWidget = newZone.get_widgets().getWidgetById(Sig.Widget.parseWidgetId($prev));
                    }

                    // if is sorting in the same zone
                    if (_sortContext.sourceZoneName == _sortContext.destinationZoneName) {
                        newZone.get_widgets().moveWidgetAfter(refWidget, draggedWidget);
                    } else {
                        // if is sorting across zone
                        var oldZone = designer.findZoneByName(_sortContext.sourceZoneName);
                        if (!oldZone) {
                            throw new Error("Old zone was not found: " + _sortContext.sourceZoneName);
                        }

                        oldZone.removeWidget(draggedWidget, false);
                        newZone.get_widgets().addWidgetAfter(refWidget, draggedWidget);
                    }
                    // 'update' done, reset _sortContext to null
                    _sortContext = null;
                },
                beforeStop: function (event, ui) {
                    var newZoneName = ui.item.parent().attr("zone-name");
                    _sortContext.destinationZoneName = newZoneName;
                }
            });

            _$element.disableSelection();
        }
        this.get_$element = function () { return _$element; }
        this.get_name = function () { return _name; }
        this.get_widgets = function () {
            return _widgets;
        }
        this.findWidgetByName = function (name) {
            return _widgets.getWidgetByName(name);
        }
        this.addWidget = function (widget) {
            _widgets.addWidget(widget);
        }
        this.removeWidget = function (widget, updateStateItem) {
            _widgets.removeWidget(widget, updateStateItem);
        }
        this.removeWidgetById = function (widgetId, updateStateItem) {
            _widgets.removeWidgetById(widgetId, updateStateItem);
        }

        function _getStateManager() {
            return Sig.Designer.get_current().get_stateManager();
        }

        // Private Methods
        function _parse($zoneElement) {
            if (!$zoneElement.hasClass("sig-zone")) {
                throw new Error("Not a valid zone dom element. Expected CSS class 'sig-zone'.");
            }
            _name = $zoneElement.attr("zone-name");
            var $widgets = $zoneElement.find(".sig-widget");
            var widgets = new Array($widgets.length);
            $widgets.each(function (i) {
                var widget = new Sig.Widget($(this));
                widget.set_zone(_this);
                widgets[i] = widget;
            });

            _widgets = new WidgetList(_this, widgets);
        }

        // Init
        _parse(_$element);
    }

    function WidgetList(zone, widgetArray) {
        if (zone === UNDEFINED_VALUE) {
            throw new ArgumentUndefinedError("zone");
        }
        if (zone == null) {
            throw new ArgumentNullError("zone");
        }

        var _zone = zone;
        var _innerList = new LinkedList();
        var _nextValidOrder = 0;
        var _this = this;

        if (arguments.length == 2 && widgetArray) {
            for (var i in widgetArray) {
                _innerList.addLast(widgetArray[i]);
            }
            var last = _innerList.get_tail();
            if (last != null) {
                _nextValidOrder = last.get_value().get_order() + 5;
            }
        }

        this.isEmpty = function () {
            return _innerList.get_count() == 0;
        }
        this.get_nextValidWidgetOrder = function () { return _nextValidOrder; }
        this.get_first = function () {
            var firstNode = _innerList.get_head();
            return firstNode == null ? null : firstNode.get_value();
        }
        this.get_last = function () {
            var lastNode = _innerList.get_tail();
            return lastNode == null ? null : lastNode.get_value();
        }
        this.getWidgetById = function (id) {
            return _innerList.findValueByPredicate(function (n, v) { return v.get_id() == id });
        }
        this.initAllWidgets = function () {
            _innerList.traverse(function (n, v) {
                v.init();
            });
        }
        this.addWidgetAfter = function (refWidget, widget) {
            var refWidgetNode = null;
            if (refWidget != null) {
                refWidgetNode = _innerList.findNode(refWidget);
            }
            _addWidgetAfter(refWidgetNode, widget);
        }
        this.addWidget = function (widget) {
            _addWidgetAfter(_innerList.get_tail(), widget);
        }
        this.moveWidgetToFirst = function (widget) {
            var widgetNode = _innerList.findNode(widget);
            _innerList.moveBeforeHead(widgetNode);
            _this.rectifyWidgetOrders();
        }
        this.moveWidgetToLast = function (widget) {
            var widgetNode = _innerList.findNode(widget);
            _innerList.moveAfterTail(widgetNode);
            _this.rectifyWidgetOrders();
        }
        this.moveWidgetBefore = function (refWidget, widgetToMove) {
            if (refWidget == null) {
                _this.moveWidgetToLast(widgetToMove);
            } else {
                var refNode = _innerList.findNode(refWidget);
                var nodeToMove = _innerList.findNode(widgetToMove);
                _innerList.moveBefore(refNode, nodeToMove);
                _this.rectifyWidgetOrders();
            }
        }
        this.moveWidgetAfter = function (refWidget, widgetToMove) {
            if (refWidget == null) {
                _this.moveWidgetToFirst(widgetToMove);
            } else {
                var refNode = _innerList.findNode(refWidget);
                var nodeToMove = _innerList.findNode(widgetToMove);
                _innerList.moveAfter(refNode, nodeToMove);
                _this.rectifyWidgetOrders();
            }
        }
        this.rectifyWidgetOrders = function () {
            var stateManager = Sig.Designer.get_current().get_stateManager();
            var current = _innerList.get_head();
            var lastOrder = 0;
            while (current != null) {
                var widget = current.get_value();
                if (widget.get_order() <= lastOrder) {
                    var newOrder = lastOrder + 5;

                    var stateItem = stateManager.getItemById(widget.get_id());
                    if (stateItem == null) {
                        stateItem = stateManager.createItem(widget.get_name(), _zone.get_name(), widget.get_templateName(), widget.get_pluginName());
                        stateItem.set_id(widget.get_id());
                        if (!widget.get_isNew()) {
                            stateItem.markChanged();
                        }
                        stateManager.addItem(stateItem);
                    }
                    stateItem.set_order(newOrder);
                    widget.set_order(newOrder);
                }
                lastOrder = widget.get_order();
                current = current.get_next();
            }

            _reCalculateNextValidOrder();
        }
        this.removeWidget = function (widget, updateStateItem) {
            var widgetNode = _innerList.findNode(widget);
            _removeWidgetNode(widgetNode, updateStateItem);
        }
        this.removeWidgetById = function (widgetId, updateStateItem) {
            var node = _innerList.findNodeByPredicate(function (n, v) { return v.get_id() == widgetId });
            _removeWidgetNode(node, updateStateItem);
        }

        function _getStateManager() {
            return Sig.Designer.get_current().get_stateManager();
        }

        function _reCalculateNextValidOrder() {
            var last = _innerList.get_tail();
            if (last == null) {
                _nextValidOrder = 0;
            } else {
                var lastWidgetOrder = last.get_value().get_order();
                if (lastWidgetOrder >= _nextValidOrder) {
                    _nextValidOrder = lastWidgetOrder + 5;
                }
            }
        }

        function _addWidgetAfter(refWidgetNode, widget) {
            var order = 5;
            var needRectifyOrder = true;

            if (refWidgetNode == null) {
                _innerList.addFirst(widget);
                _zone.get_$element().prepend(widget.get_$element());
            } else {
                if (refWidgetNode.get_next() == null) {
                    needRectifyOrder = false;
                }
                order = refWidgetNode.get_value().get_order() + 5;
                _innerList.addAfter(refWidgetNode, widget);
                widget.get_$element().insertAfter(refWidgetNode.get_value().get_$element());
            }

            widget.set_zone(_zone);
            widget.set_order(order);
            widget.reinitOverlay();

            var stateManager = _getStateManager();
            var stateItem = null;

            if (widget.get_id() != null) {
                stateItem = stateManager.getItemById(widget.get_id());
            }

            if (stateItem == null) {
                stateItem = stateManager.createItem(widget.get_name(), _zone.get_name(), widget.get_templateName(), widget.get_pluginName());

                if (widget.get_id() != null) {
                    stateItem.set_id(widget.get_id());
                } else {
                    widget.set_id(stateItem.get_id());
                }
                stateManager.addItem(stateItem);
            }

            stateItem.set_zoneName(_zone.get_name());

            if (!widget.get_isNew()) {
                stateItem.markChanged();
            }

            stateItem.set_order(order);

            if (needRectifyOrder) {
                _this.rectifyWidgetOrders();
            }
        }
        function _removeWidgetNode(widgetNode, updateStateItem) {
            var widget = widgetNode.get_value();

            if (updateStateItem === undefined) {
                updateStateItem = true;
            }

            if (updateStateItem) {
                var stateManager = _getStateManager();
                if (widget.get_isNew()) {
                    stateManager.removeItemById(widget.get_id());
                } else {
                    var stateItem = stateManager.getItemById(widget.get_id());
                    if (stateItem == null) {
                        stateItem = stateManager.createItem(widget.get_name(), _zone.get_name(), widget.get_templateName(), widget.get_pluginName());
                        stateItem.set_id(widget.get_id());
                        stateManager.addItem(stateItem);
                    }
                    stateItem.markRemoved();
                }
            }

            _innerList.removeNode(widgetNode);
            widget.get_$element().remove();

            _reCalculateNextValidOrder();
        }
    }

    // Public APIs
    window.Sig.Zone = Zone;

})(jQuery);