/*
* Author: Mouhong Lin
* 2010-09-28
*/

(function ($) {
    var $t = $.telerik;
    $t.treeview.prototype.add = function (data, parent) {
        var $parent = null;

        if (arguments.length == 1) {
            $parent = $(this.element);
        } else {
            $parent = $(parent);
        }

        var siblingsCount = $parent.find("> .t-group > .t-item").length;
        var $container = siblingsCount > 0 ? $parent.find("> .t-group") : $parent;

        var html = new $t.stringBuilder();

        if (siblingsCount == 0) {
            $t.treeview.getGroupHtml({
                data: [data],
                html: html,
                isAjax: false,
                isFirstLevel: false,
                showCheckBoxes: false,
                isExpanded: true,
                renderGroup: true
            });
        } else {
            $t.treeview.getItemHtml({
                item: data,
                html: html,
                isAjax: false,
                isFirstLevel: false,
                showCheckBoxes: false,
                itemIndex: siblingsCount,
                itemsCount: siblingsCount + 1
            });
        }

        var $newHtml = $(html.string());

        $newHtml.appendTo($container);

        if (siblingsCount == 0) {
            $parent.find("> div:first").prepend("<span class='t-icon t-minus'></span>");
        } else {
            $newHtml.prev().removeClass("t-last").find("> div").removeClass("t-bot").addClass("t-mid");
        }
    };

    $t.treeview.prototype.insertInside = function (item, refItem) {
        insert(item, refItem, "inside");
    };

    $t.treeview.prototype.insertBefore = function (item, refItem) {
        insert(item, refItem, "before");
    };

    $t.treeview.prototype.insertAfter = function (item, refItem) {
        insert(item, refItem, "after");
    };

    function insert(item, refItem, type) {
        var $item = $(item, this.element);
        var $refItem = $(refItem, this.element);

        if (type == "before") {
            $item.insertBefore($refItem);
            if ($refItem.hasClass("t-first")) {
                setAsMiddleItem($refItem);
                setAsFirstItem($item);
            } else {
                setAsMiddleItem($item);
            }
        } else if (type == "after") {
            $item.insertAfter($refItem);
            if ($refItem.hasClass("t-last")) {
                setAsMiddleItem($refItem);
                setAsLastItem($item);
            } else {
                setAsMiddleItem($item);
            }
        } else if (type == "inside") {
            $item.appendTo($refItem);
            var siblingLength = $item.parent().find("> .t-item").length - 1;
            setAsLastItem($item);

            if (siblingLength == 0) {
                $refItem.find("> div:first").prepend("<span class='t-icon t-minus'></span>");
            } else if (siblingLength == 1) {
                setAsFirstItem($item.prev());
            } else {
                setAsMiddleItem($item.prev());
            }

        }
    };

    function setAsFirstItem($item) {
        if (!$item.hasClass("t-frist")) {
            $item.removeClass("t-last").addClass("t-first")
                 .find("> div").removeClass("t-mid").removeClass("t-bot").addClass("t-top");
        }
    }

    function setAsMiddleItem($item) {
        if ($item.hasClass("t-first") || $item.hasClass("t-last")) {
            $item.removeClass("t-first").removeClass("t-last")
                 .find("> div").removeClass("t-top").removeClass("t-bot").addClass("t-mid");
        }
    }

    function setAsLastItem($item) {
        if (!$item.hasClass("t-last")) {
            $item.removeClass("t-first").addClass("t-last")
                 .find("> div").removeClass("t-top").removeClass("t-mid").addClass("t-bot");
        }
    }

    $t.treeview.prototype.remove = function (li) {
        $(li, this.element).each($.proxy(function (index, item) {
            var $element = $(item);
            var $prevItem = $element.prev();
            var $nextItem = $element.next();

            if ($prevItem.length == 0 && $nextItem.length == 0) {
                var $ul = $element.parent("ul");
                $ul.prev("div").find("> .t-icon:first").remove();
                $ul.remove();
            } else {
                $element.remove();
                if ($prevItem.length == 0) {
                    $nextItem.addClass("t-first").find("> div").removeClass("t-mid").addClass("t-top");
                } else if ($nextItem.length == 0) {
                    $prevItem.addClass("t-last").find("> div").removeClass("t-mid").addClass("t-bot");
                }
            }
        }, this));
    };

    $t.treeview.prototype.hasChild = function (li, predicate) {
        var $child = $(li, this.element).find("> ul.t-group > li:first");
        if (!predicate) {
            return $child.length > 0;
        }
        var index = 0;
        while ($child.length > 0) {
            if (predicate.apply($child[0], [index, $child[0]])) {
                return true;
            }
        }

        return false;
    }

    $t.treeview.prototype.getParent = function (li) {
        var $parent = $(li, this.element).closest(".t-item");
        if ($parent.length > 0) {
            return $parent[0];
        }

        return null;
    }

    $t.treeview.prototype.isFirst = function (li) {
        return $(li, this.element).hasClass("t-first");
    }

    $t.treeview.prototype.isLast = function (li) {
        return $(li, this.element).hasClass("t-last");
    }

    $t.treeview.prototype.findChildren = function (li, predicate) {
        var $children = $(li).find("> ul.t-group > li");
        if (predicate) {
            $children = $children.filter(function (index) {
                predicate.apply(this, [index, this]);
            });
        }
        return $children;
    }

})(jQuery);