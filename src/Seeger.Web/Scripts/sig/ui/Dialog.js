(function ($) {

    var sig = window.sig = window.sig || {};
    sig.ui = sig.ui || {};

    sig.ui.Dialog = function () {
        var _this;
        var _dialogId = '__dialog_' + new Date().getTime();
        var _$dialog;

        this.init = function (options) {
            _$dialog = $('#' + _dialogId);
            if (_$dialog.length == 0) {
                _$dialog = $('<div id="' + _dialogId + '"></div>');
                $(document.body).append(_$dialog);
            }

            var settings = {
                autoOpen: false
            };

            if (options) {
                settings = $.extend(true, settings, options);
            }

            _$dialog.dialog(settings);
        }

        this.$dialog = function () {
            return _$dialog;
        }

        this.title = function (title) {
            if (arguments.length == 0) {
                return _$dialog.dialog('option', 'title');
            }
            _$dialog.dialog('option', 'title', title);
        }

        this.open = function () {
            _$dialog.dialog('open');
        }

        this.close = function () {
            _$dialog.dialog('close');
        }

        this.find = function (selector) {
            return _$dialog.find(selector);
        }

        this.html = function (html) {
            _$dialog.html(html);
        }
        
        this.load = function (url, params, callback) {
            $.get(url, params, function (html) {
                _$dialog.html(html);
                if (callback) callback.apply(_this, []);
            });
        }
    };

})(jQuery);