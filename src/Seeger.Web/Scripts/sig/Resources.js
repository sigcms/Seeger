(function () {
    var sig = window.sig = window.sig || {};

    var _keyValueMap = {};
    var _keys = [];

    sig.Resources = {
        keys: function () {
            return _keys;
        },
        get: function (key) {
            var transformedKey = key.toUpperCase();
            var value = _keyValueMap[transformedKey];
            return value === null || value === undefined ? key : value;
        },
        init: function (keyValueMap) {
            if (keyValueMap === null && keyValueMap === undefined) return;

            for (var key in keyValueMap) {
                _keys.push(key);
                _keyValueMap[key.toUpperCase()] = keyValueMap[key];
            }
        }
    };

})();