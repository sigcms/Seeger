(function ($) {
    $.fn.extend({
        dropdown: function (options) {
            var settings = $.fn.extend({}, {
                contentSelector: null,
                align: "left",
                show: function ($contentPad) { },
                hide: function ($contentPad) { }
            }, options);

            this.each(function () {
                makeDropDown($(this));
            });

            function makeDropDown($trigger) {

                var $content = settings.contentSelector ? $(settings.contentSelector) : $($trigger.attr("content"));

                $content.remove();
                $(document.body).append($content);

                $content.addClass('__jquery_dropdown_content');

                var contentPosition = null;

                var contentInited = false;

                if ($content.length > 0) {
                    $trigger.click(function () {
                        $('.__jquery_dropdown_content').hide();

                        initContent();
                        $content.show();
                        settings.show.apply($trigger, [$content]);

                        contentPosition = $content.position();
                        contentPosition.right = contentPosition.left + $content.outerWidth();
                        contentPosition.bottom = contentPosition.top + $content.outerHeight();

                        return false;
                    });

                    $(document).click(function (e) {
                        if (contentPosition != null) {
                            if (e.pageX < contentPosition.left || e.pageX > contentPosition.right || e.pageY < contentPosition.top || e.pageY > contentPosition.bottom) {
                                $content.hide();
                                settings.hide.apply($trigger, [$content]);
                            }
                        }
                    });
                }

                function initContent() {

                    if (contentInited) return;

                    var offset = $trigger.offset();
                    var triggerWidth = $trigger.outerWidth();
                    var triggerHeight = $trigger.outerHeight();

                    var top = Math.ceil(offset.top + triggerHeight) + 1;
                    var left = offset.left;

                    if (settings.align == "right") {
                        left = Math.floor(offset.left + triggerWidth - $content.outerWidth());
                    }

                    $content.css({ left: left, top: top });
                }
            }
        }
    });
})(jQuery);