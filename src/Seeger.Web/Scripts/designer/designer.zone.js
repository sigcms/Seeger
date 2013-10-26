(function ($) {
    function Zone($element) {
        // Private Fields
        var _$element = $element;
        var _name = null;
        var _widgets = null;
        var _removedWidgets = [];
        var _this = this;
        var _sortContext = null;

        // Public APIs
        this.init = function () {
            // Init widgets
            _widgets.initAllWidgets();

            var widgetIds = [];

            _widgets.each(function (w) {
                widgetIds.push(parseInt(w.get_id(), 10));
            });

            sig.WebService.invoke('/Admin/Services/DesignerService.asmx/LoadWidgetAttributes', { locatedWidgetIds: widgetIds }, function (result) {
                _widgets.each(function (w) {
                    var attrs = result[w.get_id()];
                    w.attributes(attrs);
                    w.markUnchanged();
                });
            });

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

        this.isDirty = function () {
            if (_removedWidgets.length > 0) {
                return true;
            }

            var dirty = false;

            _widgets.each(function (w) {
                if (w.isDirty()) {
                    dirty = true;
                    return false;
                }
            });

            return dirty;
        }

        this.removedWidgets = function () {
            return _removedWidgets;
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
        this.each = function (action) {
            _innerList.traverse(function (n, v) {
                action.apply(v, [v]);
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
            var current = _innerList.get_head();
            var lastOrder = 0;
            while (current != null) {
                var widget = current.get_value();
                if (widget.get_order() <= lastOrder) {
                    var newOrder = lastOrder + 5;
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
                _zone.get_$element().prepend(widget.$element());
            } else {
                if (refWidgetNode.get_next() == null) {
                    needRectifyOrder = false;
                }
                order = refWidgetNode.get_value().get_order() + 5;
                _innerList.addAfter(refWidgetNode, widget);
                widget.$element().insertAfter(refWidgetNode.get_value().$element());
            }

            if (!widget.get_zone()) {
                widget.markAdded();
            }

            widget.set_zone(_zone);
            widget.set_order(order);
            widget.reinitOverlay();

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
                if (!widget.get_isNew()) {
                    widget.markRemoved();
                    _zone.removedWidgets().push(widget);
                }
            }

            _innerList.removeNode(widgetNode);
            widget.$element().remove();

            _reCalculateNextValidOrder();
        }
    }

    // Public APIs
    window.Sig.Zone = Zone;

})(jQuery);