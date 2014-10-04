///<reference path='/Scripts/jquery/jquery-1.4.2.js' />

/*
* Lookless Tab v0.1
*
* Copyright (c) 2010 S.Y.M http://sym.7hands.net
*
* Date: 2010-08-17
*
*/

(function ($) {
    $.fn.extend({
        looklesstab: function (options) {
            var $tabMenu = this;
            if (!$tabMenu.is("ul")) {
                $tabMenu = $tabMenu.find("ul:first");
            }

            if ($tabMenu.length == 0) { return this; }

            // Important variables
            var activeCssClass = "current";
            var $menusLinks = new Array();
            var $contentPads = new Array();
            var currentIndex = -1;

            $tabMenu.children().children("a").each(function () {
                $menusLinks.push($(this));
            });

            if ($menusLinks.length == 0) { return this; }

            for (var i in $menusLinks) {
                var href = $menusLinks[i].attr("href");

                // jquery.attr('href') method will return absolute url in IE (version < 9)
                if (href && href.indexOf('http://') == 0) {
                    href = href.substr(location.href.length);
                }

                if (href.length > 0 && href != "#") {
                    var $pad = $(href);
                    $contentPads.push($pad);
                    $pad.hide();
                } else {
                    $contentPads.push("");
                }
            }

            var defaultActiveIndex = 0;

            if (options && options.defaultActiveIndex) {
                defaultActiveIndex = parseInt(options.defaultActiveIndex.toString(), 10);
            }

            // Set default state
            activate(defaultActiveIndex);

            // Bind Events
            for (var i in $menusLinks) {
                var $link = $menusLinks[i];
                $link.bind("click", i, function (e) {
                    activate(e.data);
                    return false;
                });
            }

            // Helpers
            function activate(index) {
                setAsInactive(currentIndex);
                setAsActive(index);

                currentIndex = index;
            }

            function setAsActive(index) {
                if (index < 0) { return; }

                $menusLinks[index].parent().addClass(activeCssClass);
                var $pad = $contentPads[index];
                if ($pad != "") {
                    $pad.addClass(activeCssClass).show();
                }
            }

            function setAsInactive(index) {
                if (index < 0) { return; }

                $menusLinks[index].parent().removeClass(activeCssClass);
                var $pad = $contentPads[index];
                if ($pad != null) {
                    $pad.removeClass(activeCssClass).hide();
                }
            }

            return this;
        }
    });
})(jQuery);