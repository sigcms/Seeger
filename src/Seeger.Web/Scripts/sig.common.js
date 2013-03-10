///<reference path='/Scripts/jquery/jquery-1.4.2.js' />
///<reference path='/Scripts/jquery/jquery-ui-1.8.min.js' />

// Register Namespace
if (!window.Sig) {
    window.Sig = {};
}

window.isDigit = function (val) {
    if (val.length > 1) {
        throw new Error("Val can only be a char.");
    }
    return /^\d{1}$/.test(val);
};

(function($) {
    // Window
    var Window = {
        alert: function (msg) {
            window.alert(msg);
        },
        open: function (name, options) {
            var destoryOnClose = false;
            if (name == null) {
                destoryOnClose = true;
                name = new Date().getTime().toString();
            }

            var winWidth = $(window).width();
            var winHeight = $(window).height();

            var cache = false;
            if (typeof (options.cache) != undefined) {
                cache = options.cache;
            }
            var url = options.url;
            var title = options.title;
            var width = options.width;
            var height = options.height;
            var zIndex = 1000;
            if (typeof (options.zIndex) != undefined) {
                zIndex = options.zIndex;
            }

            if (!cache) {
                var randomNumber = new Date().getTime().toString();
                if (url.indexOf('?') > 0) {
                    url += "&r=" + randomNumber;
                } else {
                    url += "?r=" + randomNumber;
                }
            }

            if (width <= 0) {
                width = winWidth;
            } else if (width <= 1) {
                width = Math.floor(winWidth * width);
            }
            if (height <= 0) {
                height = winHeight;
            } else if (height <= 1) {
                height = Math.floor(winHeight * height);
            }

            var win = $("#" + name);
            var dialogObjectExists = false;

            if (win.length > 0) {
                win.find("iframe:first").attr("src", url);
                dialogObjectExists = true;
            } else {
                win = $("<div id='" + name + "'><iframe frameborder='0' src='" + url + "'></iframe></div>");
                var iframe = win.find("iframe:first");
                win.dialog({
                    autoOpen: false,
                    resizable: false,
                    zIndex: zIndex,
                    modal: true,
                    beforeClose: function(event, ui) {
                        if (typeof (options.beforeClose) == "function") {
                            options.beforeClose.call(this, event, ui);
                        }
                    },
                    close: function (event, ui) {
                        if (destoryOnClose) {
                            win.dialog("destory");
                        }
                        if (typeof (options.close) == "function") {
                            options.close.call(this, event, ui);
                        }
                    },
                    resize: function (event, ui) {
                        var parent = iframe.parent();
                        iframe.width(parent.width());
                        iframe.height(parent.height());
                    }
                });
            }

            if (title != null) {
                win.dialog("option", "title", title);
            }
            win.dialog("option", "width", width);
            win.dialog("option", "height", height);
            win.dialog("open");

            var frame = win.find("iframe:first");
            frame.width(width - 30);
            frame.height(height - 55);

            if (typeof (options.data) != undefined) {
                frame[0].contentWindow.data = options.data;
            }
        },
        close: function (name) {
            $("#" + name).dialog("close");
        }
    };

    window.Sig.Window = Window;

    // Message
    var Message = {
        show: function (message, options) {
            var defaultOptions = {
                identity: null,
                speed: 'normal',
                duration: 2000,
                top: 'middle',
                left: 'center',
                cssClass: null
            };

            $.extend(defaultOptions, options);

            var cssClass = 'sig-tooltip-message';

            if (defaultOptions.cssClass) {
                cssClass += ' ' + defaultOptions.cssClass;
            }

            var $html = null;

            if (defaultOptions.identity == null || defaultOptions.identity.length == 0) {
                defaultOptions.identity = new Date().getTime().toString();
                $html = _createMessageElement(defaultOptions.identity, message, cssClass);
                $html.appendTo(document.body);
            } else {
                $html = $("#" + defaultOptions.identity);
                if ($html.length == 0) {
                    $html = _createMessageElement(defaultOptions.identity, message, cssClass);
                    $html.appendTo(document.body);
                } else {
                    $html.html(message).attr("className", cssClass);
                }
            }

            if (defaultOptions.top == 'middle') {
                defaultOptions.top = Math.ceil($(window).height() / 2 - $html.height() / 2);
            }
            if (defaultOptions.left == 'center') {
                defaultOptions.left = Math.ceil($(window).width() / 2 - $html.width() / 2);
            }

            $html.css({ left: defaultOptions.left, top: defaultOptions.top });

            $html.fadeIn(defaultOptions.speed);

            if (defaultOptions.duration > 0) {
                setTimeout(function () {
                    $html.fadeOut(defaultOptions.speed);
                }, defaultOptions.duration);
            }

            function _createMessageElement(id, content, cssClass) {
                var $element = $("<div id='" + id + "' class='" + cssClass + "'>" + content + "</div>");
                $element.css({
                    position: 'fixed',
                    display: 'none',
                    'z-index': 10000
                });
                return $element;
            }
        },
        error: function(message, options) {
            if (!options) {
                options = { };
            }

            if (options.cssClass) {
                options.cssClass += ' error';
            } else {
                options.cssClass = 'error';
            }

            this.show(message, options);
        },
        hide: function (identity) {
            $("#" + identity).hide();
        }
    };

    window.Sig.Message = Message;

    var UrlUtility = {
        combine: function (url1, url2) {
            if (!url2) return url1;

            if (url1 == '' || url1 == '/') {
                return url2; 
            }
            if (url2 == '' || url2 == '/') {
                return url1;
            }

            if (url1[url1.length - 1] == '/') {
                url1 = url1.sustr(0, url1.length - 1);
            }
            if (url2[0] == '/') {
                url2 = url2.substr(1);
            }

            return url1 + '/' + url2;
        }
    };

    window.Sig.UrlUtility = UrlUtility;

})(jQuery);