(function () {
    var sig = window.sig = window.sig || {};

    var _keyValueMap = {};

    sig.GlobalResources = {
        keys: function () {
            var keys = [];
            for (var key in dictionary) {
                keys.push(key);
            }
            return keys;
        },
        get: function (key) {
            var value = _keyValueMap[key];
            return value === null || value === undefined ? key : value;
        },
        init: function (keyValueMap) {
            _keyValueMap = keyValueMap || {};
        }
    };

})();