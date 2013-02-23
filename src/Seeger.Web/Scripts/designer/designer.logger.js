(function ($) {
    var debug = true;

    var Logger = {
        /// <summary>A simple logger.</summary>
        debug: function (msg) {
            if (debug) _log(msg);
        },
        trace: function (msg) {
            _log(msg);
        }
    }

    function _log(msg) {
        if (window.console) {
            msg = "[" + _getTimeString() + "] " + msg;
            window.console.log(msg + "\r\n");
        }
    }
    function _getTimeString() {
        var now = new Date();
        return now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds() + "." + now.getMilliseconds();
    }

    window.Sig.Logger = Logger;

})(jQuery);