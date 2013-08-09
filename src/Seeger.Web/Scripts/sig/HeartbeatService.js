(function ($) {

    var sig = window.sig = window.sig || {};

    var defaultSettings = {
        serverUrl: '/Admin/Heartbeat.ashx',
        interval: 5 * 60 * 1000, // 5 mintues
    };

    sig.HeartbeatService = function (options) {
        var _this = this;
        var _options = $.extend(true, defaultSettings, options || {});
        var _intervalId = null;

        this.start = function () {
            if (!_intervalId) {
                _intervalId = setInterval(_this.heartbeat, _options.interval);
            }
        }

        this.stop = function () {
            if (_intervalId) {
                clearInterval(_intervalId);
                _intervalId = null;
            }
        }

        this.heartbeat = function () {
            $.get(_options.serverUrl);
        }
    };

    var _instance = null;

    sig.HeartbeatService.instance = function () {
        if (!_instance) {
            _instance = new sig.HeartbeatService();
        }
        return _instance;
    };

})(jQuery);