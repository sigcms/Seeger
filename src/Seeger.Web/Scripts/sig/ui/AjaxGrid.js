(function ($) {

    var sig = window.sig = window.sig || {};
    sig.ui = sig.ui || {};

    sig.ui.AjaxGrid = function (options) {
        var _this = this;
        var _$container = $(options.container);
        var _pageIndex = 0;
        var _gridPanel = null;

        this.$container = function () {
            return _$container;
        }

        this.gridId = function () {
            return _$container.attr('id') || null;
        }

        this.pageIndex = function () {
            return _pageIndex;
        }

        this.init = function () {
            _gridPanel = new GridPanel(_this);
        }

        this.find = function (selector) {
            return _$container.find(selector);
        }

        this.prev = function () {
            if (_pageIndex > 0) {
                _this.load(_pageIndex - 1);
            }
        }

        this.next = function () {
            _this.load(_pageIndex + 1);
        }

        this.load = function (pageIndex) {
            _pageIndex = pageIndex;

            sig.ui.Message.show(sig.GlobalResources.get('Message.Loading') + '...');

            if (window.PageMethods === undefined || window.PageMethods.LoadGridHtml === undefined)
                throw new Error('ScriptManager must be added with EnablePageMethods set to true, and also the page must be subclass of AjaxGridPageBase.');

            var context = {
                GridId: _this.gridId(),
                PageIndex: pageIndex,
                PageRawUrl: location.href
            };

            PageMethods.LoadGridHtml(context, function (html) {
                sig.ui.Message.hide();
                _gridPanel.html(html);
            }, function (e) {
                sig.ui.Message.error(e.get_message());
            });
        }

        this.refresh = function () {
            _this.load(_pageIndex);
        }
    };

    function GridPanel(grid) {
        var _this = this;
        var _grid = grid;
        var _$element = grid.find('.grid-panel');
        var _norecordMessage = sig.GlobalResources.get('Message.NoRecordToDisplay');

        this.html = function (html) {
            _$element.html(html);
            tryAppendEmptyRow();
            initPager();
        }

        function tryAppendEmptyRow() {
            if (_$element.find('.data-item').length == 0) {
                var $datalist = _$element.find('.data-list');

                if ($datalist.length == 0) {
                    $datalist = _$element.find('tbody');
                }

                var isTable = $datalist[0].nodeName.toUpperCase() == 'TBODY';

                if (isTable) {
                    var cols = _$element.find('thead > tr > th').length;
                    $datalist.append('<tr class="no-record"><td colspan="' + cols + '">' + _norecordMessage + '</td></tr>');
                } else {
                    $datalist.append('<div class="no-record">' + _norecordMessage + '</div>');
                }
            }
        }

        function initPager() {
            _$element.find('.pager a[data-page]').click(function () {
                _grid.load($(this).data('page'));
                return false;
            });
        }
    }

    $(function () {
        $('.ajax-grid').each(function () {
            var $grid = $(this);
            var grid = new sig.ui.AjaxGrid({
                container: $grid
            });
            grid.init();
            grid.load(0);

            $grid.data('AjaxGrid', grid);
        });
    });

})(jQuery);