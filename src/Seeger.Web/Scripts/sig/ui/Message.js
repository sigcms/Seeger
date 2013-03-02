(function ($) {

    var sig = window.sig = window.sig || {};
    sig.ui = sig.ui || {};

    var delayHideTimeout = null;

    function Message() {
        var _this = this;
        var _$element = null;
        var _delayTimeoutId = null;
        var _delay = 0;

        this.show = function (options) {
            if (typeof options == 'string') {
                options = {
                    message: options
                };
            } else if (options.Success !== undefined) {
                options = {
                    message: options.Message,
                    type: options.Success ? 'success' : 'error'
                };
            }

            ensureInited();
            clearDelayTimeout();

            if (_delay > 0) {
                _delayTimeoutId = setTimeout(function () {
                    _delay = 0;
                    showNow(options);
                }, _delay);
            } else {
                showNow(options);
            }
        }

        this.success = function (message) {
            _this.show({
                message: message,
                type: 'success'
            });
        }

        this.error = function (message) {
            _this.show({
                message: message,
                type: 'error'
            });
        }

        this.hide = function (delay) {
            _delay = delay || _delay;

            clearDelayTimeout();

            if (_$element) {
                if (_delay > 0) {
                    _delayTimeoutId = setTimeout(function () {
                        _delay = 0;
                        _$element.hide();
                    }, delay);
                } else {
                    _$element.hide();
                }
            }
        }

        this.delay = function (delayMilliseconds) {
            _delay = delayMilliseconds;
            return _this;
        }

        function clearDelayTimeout() {
            if (_delayTimeoutId) {
                window.clearTimeout(_delayTimeoutId);
            }
        }

        function ensureInited() {
            if (_$element == null) {
                _$element = $('.sig-ui-message');

                if (_$element.length == 0) {
                    _$element = $('<div class="sig-ui-message" style="display:none"></div>');
                    $(document.body).append(_$element);
                }
            }
        }

        function showNow(options) {
            _$element.removeClass('sig-ui-message-error').removeClass('sig-ui-message-success');

            if (options.type) {
                _$element.addClass('sig-ui-message-' + options.type);
            }

            _$element.html(options.message);

            _$element.css('position', 'fixed');

            var left = Math.floor(($(window).width() - _$element.outerWidth()) / 2);
            _$element.css({ left: left, top: 0 });

            _$element.show();
        }
    }

    var _instance = new Message();

    sig.ui.Message = {
        show: function (options) {
            _instance.show(options);
        },
        success: function (message) {
            _instance.success(message);
        },
        error: function (message) {
            _instance.error(message);
        },
        hide: function (delay) {
            _instance.hide(delay);
        },
        delay: function (delayMilliseconds) {
            _instance.delay(delayMilliseconds);
            return _instance;
        }
    };

})(jQuery);