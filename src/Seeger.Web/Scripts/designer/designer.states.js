(function ($) {

    var WidgetState = {
        added: 'added',
        removed: 'removed',
        changed: 'changed'
    };

    function WidgetStateItem(widgetName, zoneName, templateName, pluginName) {
        if (widgetName == UNDEFINED_VALUE || widgetName == null || widgetName.length == 0) {
            throw new Error("'widgetName' cannot be undefined, null or empty.");
        }
        if (zoneName == UNDEFINED_VALUE || zoneName == null || zoneName.length == 0) {
            throw new Error("'zoneName' cannot be undefined, null or empty.");
        }

        var _widgetName = widgetName;
        var _zoneName = zoneName;
        var _pluginName = "";
        var _templateName = "";
        // for pesisted one: id is the WidgetInPageId;
        // for new one: id is a string starting with 'new';
        var _id = 0;
        var _order = 0;
        var _state = WidgetState.added;
        var _customData = {};
        var _attributes = {};
        var _this = this;

        if (templateName) {
            _templateName = templateName;
        }
        if (pluginName) {
            _pluginName = pluginName;
        }

        this.get_widgetName = function () { return _widgetName; }
        this.get_zoneName = function () { return _zoneName; }
        this.set_zoneName = function (value) { _zoneName = value; }
        this.get_pluginName = function () { return _pluginName; }
        this.set_pluginName = function (value) { _pluginName = value; }
        this.get_templateName = function () { return _templateName; }
        this.set_templateName = function (value) { _templateName = value; }
        this.get_id = function () { return _id; }
        this.set_id = function (id) {
            if (id === UNDEFINED_VALUE || id == null || id.length == 0) {
                throw new Error("Invalid id.");
            }
            var indexOfNew = id.indexOf("new");
            if (indexOfNew == 0) {
                _this.markAdded();
            } else if (indexOfNew < 0) {
                _this.markChanged();
            } else {
                throw new Error("Invalid id.");
            }
            _id = id;
        }
        this.get_order = function () { return _order; }
        this.set_order = function (value) { _order = value; }
        this.get_state = function () { return _state; }
        this.get_customData = function () { return _customData; }
        this.set_customData = function (value) {
            if (value === undefined) throw new Error("'value' cannot be undefined.");
            _customData = value;
        }
        this.get_attributes = function () { return _attributes; }
        this.set_attributes = function (value) {
            if (value === undefined) throw new Error("'value' cannot be undefined.");
            _attributes = value;
        }
        this.get_isNew = function () { return _state == WidgetState.added; }
        this.markAdded = function () { _state = WidgetState.added; }
        this.markChanged = function () { _state = WidgetState.changed; }
        this.markRemoved = function () { _state = WidgetState.removed; }

        this.toString = function () {
            return "[id=" + _id + ",widget=" + _widgetName + ",zone=" + _zoneName + ",module=" + _pluginName + ",template=" + _templateName + ",state=" + _state + ",order=" + _order + "]";
        }
        this.serialize = function () {
            var dto = new Object();
            dto.id = _id;
            dto.widgetName = _widgetName;
            dto.zoneName = _zoneName;
            dto.pluginName = _pluginName;
            dto.templateName = _templateName;
            dto.order = _order;
            dto.state = _state;
            dto.customData = _customData;
            dto.attributes = _attributes;

            return JSON.stringify(dto);
        }
    }

    function WidgetStateManager() {
        var _innerDic = {};
        var _this = this;
        var _count = 0;

        this.get_count = function () { return _count; }
        this.get_isDirty = function () { return _count > 0; }
        this.getItemById = function (id) {
            if (id === UNDEFINED_VALUE || id == null) {
                throw new Error("id cannot be null or undefined value.");
            }

            var val = _innerDic[id];
            if (val === UNDEFINED_VALUE) {
                return null;
            }

            return val;
        }
        this.existItem = function (id) {
            return _this.getItemById(id) != null;
        }
        this.addItem = function (stateItem) {
            if (stateItem == UNDEFINED_VALUE || stateItem == null) {
                throw new Error("'stateItem' cannot be undefined or null.");
            }
            if (!(stateItem instanceof WidgetStateItem)) {
                throw new Error("'stateItem' should be of type 'WidgetStateItem'.");
            }
            var id = stateItem.get_id();
            if (id === UNDEFINED_VALUE || id == null) {
                throw new Error("Id must be specified for the state item.");
            }

            if (!_this.existItem(id)) {
                ++_count;
            }
            _innerDic[id] = stateItem;
        }
        this.updateItem = function (id, newStateItem) {
            if (_this.existItem(id)) {
                if (id != newStateItem.get_id()) {
                    throw new Error("The id of the new state item must be same as the id of the old state item.");
                }
                _innerDic[id] = newStateItem;
            } else {
                throw new Error("Not found state item with id = " + id);
            }
        }
        this.removeItem = function (stateItem) {
            if (stateItem == UNDEFINED_VALUE || stateItem == null) {
                throw new Error("'stateItem' cannot be undefined or null.");
            }
            if (!(stateItem instanceof WidgetStateItem)) {
                throw new Error("'stateItem' should be of type 'WidgetStateItem'.");
            }
            if (_this.existItem(stateItem.get_id())) {
                delete _innerDic[id];
                --_count;
            }
        }
        this.removeItemById = function (id) {
            if (_this.existItem(id)) {
                delete _innerDic[id];
                --_count;
            }
        }
        this.serialize = function () {
            if (_count == 0) {
                return "";
            }

            var result = "";
            for (var i in _innerDic) {
                result += "," + _innerDic[i].serialize();
            }

            if (result.length > 0) {
                result = "[" + result.substr(1) + "]";
            }

            return result;
        }
        this.reset = function () {
            _innerDic = {};
        }
        this.createItem = function (widgetName, zoneName, templateName, pluginName) {
            var stateItem = new WidgetStateItem(widgetName, zoneName, templateName, pluginName);
            stateItem.set_id(_createItemId());
            return stateItem;
        }
        this.toString = function () {
            return _this.serialize();
        }

        function _createItemId() {
            return "new" + new Date().getTime();
        }
    }

    // Register Classes
    window.Sig.WidgetState = WidgetState;
    window.Sig.WidgetStateItem = WidgetStateItem;
    window.Sig.WidgetStateManager = WidgetStateManager;

})(jQuery);