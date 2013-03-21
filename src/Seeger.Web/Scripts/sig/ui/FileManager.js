(function ($) {
    var sig = window.sig = window.sig || {};
    sig.ui = sig.ui || {};

    sig.ui.FileManager = function (options) {
        var _this = this;
        var _$container = $(options.container);
        var _rootPath = options.rootPath || '/Files';
        var _currentPath = options.currentPath || _rootPath;
        var _handler = options.handler || '/Admin/Services/FileManagerService.asmx';

        var _toolbar = null;
        var _fileGrid = null;

        this.$container = function () {
            return _$container;
        }

        this.find = function (selector) {
            return _$container.find(selector);
        }

        this.currentPath = function () {
            return _currentPath;
        }

        this.isInRootFolder = function () {
            return _currentPath.toUpperCase() === _rootPath.toUpperCase();
        }

        this.enterParentFolder = function () {
            if (_this.isInRootFolder()) return;
            _this.list(sig.UrlUtil.getDirectory(_currentPath));
        }

        this.enterSubfolder = function (folderName) {
            if (!folderName)
                throw new Error('"folderName" is required.');

            _this.list(sig.UrlUtil.combine(_currentPath, folderName));
        }

        this.init = function () {
            _$container.html(createBaseHtml());

            _fileGrid = new FileGrid(_this);
            _fileGrid.init();

            if (_this.isInRootFolder()) {
                _fileGrid.hideBacktoParentFolderButton();
            } else {
                _fileGrid.showBacktoParentFolderButton();
            }

            _toolbar = new Toolbar(_this);

            _this.list(_currentPath);
        }

        this.list = function (path) {
            if (!path) throw new Error('"path" is required.');

            _currentPath = path;

            if (_this.isInRootFolder()) {
                _fileGrid.hideBacktoParentFolderButton();
            } else {
                _fileGrid.showBacktoParentFolderButton();
            }

            sig.WebService.invoke(serviceUrl('List'), { path: _currentPath }, function (files) {
                _fileGrid.update(files);
            });
        }

        function createBaseHtml() {
            return '<div class="fm-toolbar"></div>'
                 + '<div class="fm-breadcrumb"></div>'
                 + '<div class="fm-filegrid">'
                        + '<a href="#" class="btn-backto-parent" style="display:none">' + sig.GlobalResources.get('Back to parent folder') + '</a>'
                        + '<table class="datatable">' 
                            + '<thead>'
                                + '<tr>'
                                    + '<th>' + sig.GlobalResources.get('Filename') + '</th>'
                                    + '<th class="fm-filesize-header">' + sig.GlobalResources.get('Filesize') + '</th>'
                                + '</tr>'
                            + '</thead>'
                            + '<tbody>'
                            + '</tbody>'
                        + '</table>'
                 + '</div>';
        }

        function serviceUrl(method) {
            if (_handler[_handler.length - 1] !== '/') {
                return _handler + '/' + method;
            }
            return _handler + method;
        }
    };

    function FileGrid(fileManager) {
        var _this = this;
        var _fileManager = fileManager;
        var _$container = fileManager.find('.fm-filegrid');
        var _$body = _$container.find('tbody');

        this.init = function () {
            _$body.append(createEmptyItemHtml());

            _$container.on('click', '.btn-backto-parent', function () {
                _fileManager.enterParentFolder();
                return false;
            });
            _$container.on('click', '.fm-folder .icon-folder', function () {
                _fileManager.enterSubfolder($(this).closest('.fm-folder').data('filename'));
                return false;
            });
        }

        this.showBacktoParentFolderButton = function () {
            _$container.find('.btn-backto-parent').show();
        }

        this.hideBacktoParentFolderButton = function () {
            _$container.find('.btn-backto-parent').hide();
        }

        this.update = function (files) {
            _this.clear();
            $.each(files, function () {
                _this.add(this);
            });
        }

        this.add = function (file) {
            var $item = $(createItemHtml(file));
            _$body.append($item);
            hideEmptyItem();
        }

        this.clear = function () {
            _$container.find('.fm-item').each(function () {
                deleteItem($(this));
            });
        }

        this.delete = function (fileName) {
            deleteItem(_$container.find('.fm-item[data-filename="' + fileName + '"]'));
        }

        function deleteItem($item) {
            $item.remove();
            if (_$body.find('.fm-item:first').length === 0) {
                showEmptyItem();
            }
        }

        function showEmptyItem() {
            _$body.find('.fm-empty-item').show();
        }

        function hideEmptyItem() {
            _$body.find('.fm-empty-item').hide();
        }

        function createEmptyItemHtml() {
            return '<tr class="fm-empty-item" style="display:none"><td colspan="' + _$container.find('thead th').length + '">' + sig.GlobalResources.get('No records to display') + '</td></tr>';
        }

        function createItemHtml(file) {
            var css = 'fm-item';
            if (file.IsDirectory) {
                css += ' fm-folder';
            } else {
                css += ' fm-file';
            }

            var fileNameCss = 'icon-link';
            if (file.IsDirectory) {
                fileNameCss += ' icon-folder';
            } else {
                fileNameCss += ' icon-file';
                var ext = sig.UrlUtil.getExtension(file.Name);
                if (ext) {
                    fileNameCss += ' icon-' + ext.substr(1);
                }
            }

            var html = '<tr class="' + css + '" data-filename="' + file.Name + '">'
                            + '<td>'
                                + '<a href="#" class="' + fileNameCss + '">' + file.Name + '</a>'
                            + '</td>'
                            + '<td class="fm-filesize">' + (file.IsDirectory ? '' : friendlyFileLengthText(file.Length)) + '</td>'
                     + '</tr>';

            return html;
        }

        function friendlyFileLengthText(length) {
            if (length < 1024) {
                return length + 'B';
            }

            var kb = Math.round(length / 1024 * 100) / 100;
            if (kb < 1024) {
                return kb + 'K';
            }

            var mb = Math.round(kb / 1024 * 100) / 100;
            if (mb < 1024) {
                return mb + 'M';
            }

            return Math.round(mb / 1024 * 100) / 100 + 'G';
        }
    }

    function Toolbar(fileManager) {
        var _this = this;
        var _fileManager = fileManager;
        var _$container = fileManager.find('.fm-toolbar');
        var _buttons = [];

        this.$container = function () {
            return _$container;
        }

        this.fileManager = function () {
            return _fileManager;
        }

        this.addButton = function (button) {
            var $button = $('<button type="button"></button>');
            $button.html(button.text);
            _$container.append($button);

            if (button.click) {
                $button.click(function (event) {
                    button.click.apply(this, [event, {
                        toolbar: _this
                    }]);
                });
            }

            _buttons.push(button);
        }
    }


})(jQuery);