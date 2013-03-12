(function ($) {

    var sig = window.sig = window.sig || {};
    sig.ui = sig.ui || {};

    sig.ui.AjaxGrid = function (options) {
        var _this = this;
        var _$container = $(options.container);
        var _pageIndex = 0;
        var _searchPanel = null;
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
            _searchPanel = new SearchPanel(_this);
            _searchPanel.init();
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
                PageRawUrl: location.href,
                SearchModel: _searchPanel.searchModel()
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
            initGridActions();
            initPager();
        }

        this.executeAction = function (element) {
            var $element = $(element);
            var actionName = $element.data('action');

            if (!actionName)
                throw new Error('Action name is not defined.');

            var confirmMessage = $element.data('confirm');

            if (confirmMessage != false) {
                // 'Delete' is special action
                if (actionName === 'Delete' && (confirmMessage === undefined || confirmMessage === '')) {
                    confirmMessage = sig.GlobalResources.get('Message.DeleteConfirm');
                }

                if (confirmMessage) {
                    if (!window.confirm(confirmMessage)) return false;
                }
            }

            var params = [];

            $.each($element[0].attributes, function () {
                if (this.name.indexOf('data-action-param-') >= 0) {
                    params.push(this.value);
                }
            });

            params.push(function (result) {
                _grid.refresh();
            });

            params.push(function (e) {
                sig.ui.Message.error(e.get_message());
            });

            // for now only support PageMethods
            PageMethods[actionName].apply(_grid, params);
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

        function initGridActions() {
            _$element.find('.grid-action').each(function () {
                var $button = $(this);
                if ($button.data('action')) {
                    $button.click(function () {
                        _this.executeAction(this);
                        return false;
                    });
                }
            });
        }

        function initPager() {
            _$element.find('.pager a[data-page]').click(function () {
                _grid.load($(this).data('page'));
                return false;
            });
        }
    }

    function SearchPanel(grid) {
        var _this = this;
        var _grid = grid;
        var _$panel = _grid.find('.search-panel');

        this.init = function () {
            _$panel.find('.btn-search').click(function () {
                _grid.load(0); return false;
            });
            _$panel.find(':text').keyup(function (e) {
                if (e.which == 13) {
                    _grid.load(0); return false;
                }
            });
        }

        this.searchModel = function (model) {
            if (arguments.length == 0) {
                return buildSearchModel();
            }

            bindSearchModel(model);
        }

        function buildSearchModel() {
            var model = {};
            _$panel.find(':text,:radio,:checkbox,select').each(function () {
                var $input = $(this);
                var inputValue = $input.val();
                var name = $input.attr('name');
                if (name) {
                    if ($input.is(':radio')) {
                        if ($input.is(':checked')) {
                            model[name] = tryParseBooleanValue(inputValue);
                        }
                    } else if ($input.is(':checkbox')) {
                        if ($input.is(':checked')) {
                            var currentValue = model[name];
                            if (currentValue == null || currentValue == undefined) {
                                model[name] = tryParseBooleanValue(inputValue);
                            } else {
                                model[name] = model[name] + ',' + inputValue;
                            }
                        }
                    } else {
                        model[name] = inputValue;
                    }
                }
            });

            return model;
        }

        function tryParseBooleanValue(value) {
            if (value === 'true') {
                return true;
            }
            if (value === 'false') {
                return false;
            }
            return value;
        }

        function bindSearchModel(model) {
            for (var name in model) {
                var $inputs = _$panel.find('[name=' + name + ']');
                var value = model[name];

                $.each($inputs, function () {
                    var $input = $(this);
                    if ($input.is(':radio')) {
                        if (value == $input.val()) {
                            $input.attr('checked', 'checked');
                        }
                    } else if ($input.is(':checkbox')) {
                        if (typeof (value) === 'string' && value !== null && value !== undefined) {
                            var values = value.split(',');
                            var checkboxValue = $input.val();

                            if (_.any(values, function (v) { return v == checkboxValue; })) {
                                $input.attr('checked', 'checked');
                            }
                        }
                    } else {
                        $input.val(value);
                    }
                });
            }
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