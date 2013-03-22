(function ($) {
    var sig = window.sig = window.sig || {};
    sig.ui = sig.ui || {};

    sig.ui.FileManager = function (options) {
        var _this = this;
        var _$container = $(options.container);
        var _rootPath = options.rootPath || '/Files';
        var _currentPath = options.currentPath || _rootPath;
        var _handler = options.handler || '/Admin/Services/FileManagerService.asmx';
        var _enableMultiSelect = options.enableMultiSelect === undefined ? true : options.enableMultiSelect;

        var _toolbar = null;
        var _fileGrid = null;

        this.$container = function () {
            return _$container;
        }

        this.grid = function () {
            return _fileGrid;
        }

        this.enableMultiSelect = function () {
            return _enableMultiSelect;
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
            _toolbar.init();

            _this.list(_currentPath);
        }

        this.list = function (path, callback) {
            if (!path) throw new Error('"path" is required.');

            _currentPath = path;

            if (_this.isInRootFolder()) {
                _fileGrid.hideBacktoParentFolderButton();
            } else {
                _fileGrid.showBacktoParentFolderButton();
            }

            sig.WebService.invoke(serviceUrl('List'), { path: _currentPath }, function (files) {
                _fileGrid.update(files);
                if (callback) callback.apply(_this, []);
            });
        }

        this.refresh = function (callback) {
            _this.list(_currentPath, callback);
        }

        this.createFolder = function (folderName) {
            if (!folderName) throw new Error('"folderName" is required.');

            sig.WebService.invoke(serviceUrl('CreateFolder'), { path: _currentPath, folderName: folderName }, function () {
                _this.refresh();
            });
        }

        this.selectFile = function (fileName) {
            _fileGrid.select(fileName);
        }

        this.unselectFile = function (fileName) {
            _fileGrid.unselect(fileName);
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
            _$container.on('click', '.fm-item', function () {
                if (_this.isSelected(this)) {
                    _this.unselect(this);
                } else {
                    _this.select(this);
                }
            });
            _$container.on('mouseenter', '.fm-item', function () {
                $(this).addClass('fm-hover');
            });
            _$container.on('mouseleave', '.fm-item', function () {
                $(this).removeClass('fm-hover');
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
            $item.data('file', file);
            hideEmptyItem();
        }

        this.isSelected = function (fileName) {
            if (!fileName)
                throw new Error('"fileName" is required.');

            var $item = typeof (fileName) === 'string' ? findItem(fileName) : $(fileName);
            return $item.is('.fm-selected');
        }

        this.select = function (fileName) {
            if (!fileName)
                throw new Error('"fileName" is required.');

            if (!_fileManager.enableMultiSelect()) {
                _this.unselectAll();
            }

            var $item = typeof (fileName) === 'string' ? findItem(fileName) : $(fileName);
            $item.addClass('fm-selected');
        }

        this.selectAll = function () {
            _$container.find('.fm-item:not(.fm-selected').each(function () {
                _this.select(this);
            });
        }

        this.unselect = function (fileName) {
            if (!fileName)
                throw new Error('"fileName" is required.');

            var $item = typeof (fileName) === 'string' ? findItem(fileName) : $(fileName);
            $item.removeClass('fm-selected');
        }

        this.unselectAll = function () {
            _$container.find('.fm-item.fm-selected').each(function () {
                _this.unselect(this);
            });
        }

        this.clear = function () {
            _$container.find('.fm-item').each(function () {
                deleteItem($(this));
            });
        }

        this.delete = function (fileName) {
            if (!fileName)
                throw new Error('"fileName" is required.');

            deleteItem(findItem(fileName));
        }

        function findItem(fileName) {
            return _$body.find('.fm-item[data-filename="' + fileName + '"]');
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

        this.init = function () {
            _this.addButton(NewFolderButton);
            _this.addButton(BatchUploadButton);
        }

        this.addButton = function (button) {
            var $button = $('<button type="button"></button>');
            $button.html(sig.GlobalResources.get(button.text));
            _$container.append($button);

            if (button.click) {
                $button.click(function (event) {
                    button.click.apply(this, [event, {
                        toolbar: _this,
                        fileManager: _fileManager
                    }]);
                });
            }

            _buttons.push(button);
        }
    }

    var NewFolderButton = {
        text: 'New folder',
        click: function (event, context) {
            var folderName = '';
            do {
                folderName = prompt(sig.GlobalResources.get('Please enter the folder name') + ':');
            } while (folderName !== null && $.trim(folderName).length === 0);

            if (folderName) {
                var manager = context.fileManager;
                manager.createFolder(folderName);
            }
        }
    };

    var BatchUploadButton = {
        _dialog: null,
        text: 'Upload',
        click: function (event, context) {
            var dialog = BatchUploadButton._dialog;

            if (dialog == null) {
                dialog = new sig.ui.BatchUploadDialog({
                    onQueueComplete: function (result) {
                        context.fileManager.refresh(function () {
                            var manager = this;
                            $.each(result.files, function () {
                                if (this.success) {
                                    manager.selectFile(this.fileName);
                                }
                            });
                        });
                    }
                });

                dialog.init();
                BatchUploadButton._dialog = dialog;
            }

            dialog.folder(context.fileManager.currentPath());
            dialog.open();
        }
    };

})(jQuery);