(function ($) {
    var sig = window.sig = window.sig || {};
    sig.ui = sig.ui || {};

    sig.ui.FileManager = function (options) {

        // validate options
        if (!options.element)
            throw new Error('"options.element" is required.');

        var _this = this;
        var _$element = $(options.element);
        var _toolbar = null;
        var _breadcrumb = null;
        var _fileGrid = null;

        var _options = {
            bucketId: null,
            folder: '/',
            handler: '/Admin/Services/FileManagerService.asmx',
            allowMultiSelect: true,
            allowSelectFolder: true,
            allowSelectFile: true,
            filter: '*.*'
        };

        if (options) {
            _options = $.extend(true, _options, options);
        }

        var _currentPath = _options.folder;

        this.$element = function () {
            return _$element;
        }

        this.option = function (key, value) {
            if (arguments.length === 2) {
                _options[key] = value;
            } else {
                return _options[key];
            }
        }

        this.bucketId = function () {
            return _options.bucketId;
        }

        this.grid = function () {
            return _fileGrid;
        }

        this.filter = function () {
            return _options.filter;
        }

        this.allowMultiSelect = function () {
            return _this.option('allowMultiSelect');
        }

        this.allowSelectFolder = function () {
            return _this.option('allowSelectFolder');
        }

        this.allowSelectFile = function () {
            return _this.option('allowSelectFile');
        }

        this.find = function (selector) {
            return _$element.find(selector);
        }

        this.currentPath = function () {
            return _currentPath;
        }

        this.enterSubfolder = function (folderName) {
            if (!folderName)
                throw new Error('"folderName" is required.');

            _this.list(sig.UrlUtil.combine(_this.currentPath(), folderName));
        }

        this.init = function () {
            _$element.html(createBaseHtml());

            _fileGrid = new FileGrid(_this);
            _fileGrid.init();

            _toolbar = new Toolbar(_this);
            _toolbar.init();

            _breadcrumb = new Breadcrumb(_this);
            _breadcrumb.init();

            _this.list(_this.currentPath());
        }

        this.loadBuckets = function (callback) {
            sig.WebService.invoke(serviceUrl('Buckets'), null, function (buckets) {
                callback(buckets);
            });
        }

        this.switchBucket = function (bucketId) {
            if (_this.bucketId() == bucketId) {
                return;
            }

            _options.bucketId = bucketId;

            _this.list('/');
        }

        this.list = function (path, callback) {
            if (!path) throw new Error('"path" is required.');

            _currentPath = path;
            _breadcrumb.setPath(_currentPath);

            sig.WebService.invoke(serviceUrl('List'), { bucketId: _options.bucketId, path: _this.currentPath(), filter: _options.filter }, function (files) {
                files = _.map(files, function (f) { return toClientFileObject(f); });
                _fileGrid.update(files);
                if (callback) callback.apply(_this, []);
            });
        }

        this.refresh = function (callback) {
            _this.list(_this.currentPath(), callback);
        }

        this.createFolder = function (folderName) {
            if (!folderName) throw new Error('"folderName" is required.');

            sig.WebService.invoke(serviceUrl('CreateFolder'), { bucketId: _options.bucketId, path: _this.currentPath(), folderName: folderName }, function () {
                _this.refresh();
            });
        }

        this.selectEntry = function (entryName) {
            _fileGrid.selectEntry(entryName);
        }

        this.unselectEntry = function (entryName) {
            _fileGrid.unselectEntry(entryName);
        }

        this.selectedEntries = function () {
            return _fileGrid.selectedEntries();
        }

        function createBaseHtml() {
            return '<div class="fm-toolbar"><div class="fm-toolbar-primary"></div><div class="fm-toolbar-secondary"></div></div>'
                 + '<div class="fm-breadcrumb"></div>'
                 + '<div class="fm-filegrid">'
                        + '<table class="datatable">'
                            + '<thead>'
                                + '<tr>'
                                    + '<th>' + sig.Resources.get('Filename') + '</th>'
                                    + '<th class="fm-filesize-header">' + sig.Resources.get('Filesize') + '</th>'
                                + '</tr>'
                            + '</thead>'
                            + '<tbody>'
                            + '</tbody>'
                        + '</table>'
                 + '</div>';
        }

        function serviceUrl(method) {
            var handler = _this.option('handler');
            if (handler[handler.length - 1] !== '/') {
                return handler + '/' + method;
            }
            return handler + method;
        }

        function toClientFileObject(file) {
            return {
                name: file.Name,
                virtualPath: file.VirtualPath,
                publicUri: file.PublicUri,
                length: file.Length,
                isDirectory: file.IsDirectory
            };
        }
    };

    function FileGrid(fileManager) {
        var _this = this;
        var _fileManager = fileManager;
        var _$element = fileManager.find('.fm-filegrid');
        var _$body = _$element.find('tbody');

        this.$element = function () {
            return _$element;
        }

        this.init = function () {
            _$body.append(createEmptyItemHtml());

            _$element.on('click', '.fm-folder .icon-folder', function () {
                _fileManager.enterSubfolder($(this).closest('.fm-folder').data('entryname'));
                return false;
            });
            _$element.on('click', '.fm-item', function (e) {
                if (_this.isFile(this) && !_fileManager.allowSelectFile()) return;
                if (_this.isDirectory(this) && !_fileManager.allowSelectFolder()) return;

                if (_this.isSelected(this)) {
                    _this.unselectEntry(this);
                } else {
                    if (!e.ctrlKey) {
                        _this.unselectAll();
                    }

                    _this.selectEntry(this);
                }
            });
            _$element.on('mouseenter', '.fm-item', function () {
                $(this).addClass('fm-hover');
            });
            _$element.on('mouseleave', '.fm-item', function () {
                $(this).removeClass('fm-hover');
            });
        }

        this.maxHeight = function (value) {
            if (arguments.length === 0) {
                return _$element.css('max-height');
            }
            _$element.css('max-height', value);
        }

        this.update = function (entries) {
            _this.deleteAll();
            $.each(entries, function () {
                _this.addEntry(this);
            });

            if (!_this.hasEntries()) {
                showEmptyItem();
            }
        }

        this.isFile = function (entryName) {
            if (!entryName)
                throw new Error('"entryName" is required.');

            var $item = typeof (entryName) === 'string' ? findItem(entryName) : $(entryName);
            return !$item.data('FileSystemEntry').isDirectory;
        }

        this.isDirectory = function (entryName) {
            return !_this.isFile(entryName);
        }

        this.addEntry = function (entry) {
            var $item = $(createItemHtml(entry));
            _$body.append($item);
            $item.data('FileSystemEntry', entry);
            hideEmptyItem();
        }

        this.isSelected = function (entryName) {
            if (!entryName)
                throw new Error('"entryName" is required.');

            var $item = typeof (entryName) === 'string' ? findItem(entryName) : $(entryName);
            return $item.is('.fm-selected');
        }

        this.selectEntry = function (entryName) {
            if (!entryName)
                throw new Error('"entryName" is required.');

            if (!_fileManager.allowMultiSelect()) {
                _this.unselectAll();
            }

            var $item = typeof (entryName) === 'string' ? findItem(entryName) : $(entryName);
            $item.addClass('fm-selected');
        }

        this.selectAll = function () {
            _$element.find('.fm-item:not(.fm-selected').each(function () {
                _this.selectEntry(this);
            });
        }

        this.selectedEntries = function () {
            var files = [];
            _$element.find('.fm-item.fm-selected').each(function () {
                files.push($(this).data('FileSystemEntry'));
            });

            return files;
        }

        this.unselectEntry = function (entryName) {
            if (!entryName)
                throw new Error('"entryName" is required.');

            var $item = typeof (entryName) === 'string' ? findItem(entryName) : $(entryName);
            $item.removeClass('fm-selected');
        }

        this.unselectAll = function () {
            _$element.find('.fm-item.fm-selected').each(function () {
                _this.unselectEntry(this);
            });
        }

        this.deleteAll = function () {
            _$element.find('.fm-item').each(function () {
                deleteItem($(this));
            });
        }

        this.deleteEntry = function (entryName) {
            if (!entryName)
                throw new Error('"entryName" is required.');

            deleteItem(findItem(entryName));
        }

        this.hasEntries = function () {
            return _$body.find('.fm-item:first').length > 0;
        }

        function findItem(entryName) {
            return _$body.find('.fm-item[data-entryname="' + entryName + '"]');
        }

        function deleteItem($item) {
            $item.remove();
            if (!_this.hasEntries()) {
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
            return '<tr class="fm-empty-item" style="display:none"><td colspan="' + _$element.find('thead th').length + '">' + sig.Resources.get('No records to display') + '</td></tr>';
        }

        function createItemHtml(file) {
            var css = 'fm-item';
            if (file.isDirectory) {
                css += ' fm-folder';
            } else {
                css += ' fm-file';
            }

            var fileNameCss = 'icon-link';
            if (file.isDirectory) {
                fileNameCss += ' icon-folder';
            } else {
                fileNameCss += ' icon-file';
                var ext = sig.UrlUtil.getExtension(file.name);
                if (ext) {
                    fileNameCss += ' icon-' + ext.substr(1);
                }
            }

            var html = '<tr class="' + css + '" data-entryname="' + file.name + '">'
                            + '<td>'
                                + '<a href="#" class="' + fileNameCss + '">' + file.name + '</a>'
                            + '</td>'
                            + '<td class="fm-filesize">' + (file.isDirectory ? '' : friendlyFileLengthText(file.length)) + '</td>'
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

    function Breadcrumb(fileManager) {
        var _this = this;
        var _fileManager = fileManager;
        var _$element = fileManager.find('.fm-breadcrumb');

        this.$container = function () {
            return _$element;
        }

        this.init = function () {
            _this.setPath(fileManager.currentPath());

            _$element.on('click', '.fm-path-segment', function () {
                var path = $(this).data('path');
                fileManager.list(path);
                return false;
            });
        }

        this.setPath = function (path) {
            var html = createHtml(path);
            _$element.html(html);
        }

        function createHtml(path) {
            var html = '<a href="#" data-path="/" class="fm-path-segment fm-path-root">' + sig.Resources.get('Root') + '</a>';
            html += '<span class="divider">/</span>';

            var folders = path.split('/');
            var currentPath = '/';

            $.each(folders, function () {
                if (this && (this != '') && (this != '/')) {
                    currentPath = currentPath + this + '/';
                    html += '<a href="#" data-path="' + currentPath + '" class="fm-path-segment">' + this + '</a>';
                    html += '<span class="divider">/</span>';
                }
            });

            return html;
        }
    }

    function Toolbar(fileManager) {
        var _this = this;
        var _fileManager = fileManager;
        var _$element = fileManager.find('.fm-toolbar');
        var _controls = [];

        this.$container = function () {
            return _$element;
        }

        this.fileManager = function () {
            return _fileManager;
        }

        this.init = function () {
            _this.addControl(NewFolderControl);
            _this.addControl(BatchUploadControl);
            _this.addControl(SwitchBucketControl, false);
        }

        this.addControl = function (control, primary) {
            if (primary === undefined) {
                primary = true;
            }

            var $control = $(control.create.apply(_this, [{
                toolbar: _this,
                fileManager: _fileManager
            }]));

            var containerClass = primary ? '.fm-toolbar-primary' : '.fm-toolbar-secondary';

            _$element.find(containerClass).append($control);

            if (control.init) {
                control.init.apply(_this, [$control, {
                    toolbar: _this,
                    fileManager: _fileManager
                }]);
            }

            control.$element = $control;
            _controls.push(control);
        }
    }

    var NewFolderControl = {
        create: function (context) {
            return '<button type="button">' + sig.Resources.get('New folder') + '</button>';
        },
        init: function (element, context) {
            $(element).click(function () {
                var folderName = '';
                do {
                    folderName = prompt(sig.Resources.get('Please enter the folder name') + ':');
                } while (folderName !== null && $.trim(folderName).length === 0);

                if (folderName) {
                    var manager = context.fileManager;
                    manager.createFolder(folderName);
                }
            });
        }
    };

    var BatchUploadControl = {
        _dialog: null,
        create: function (context) {
            return '<button type="button">' + sig.Resources.get('Upload') + '</button>';
        },
        init: function (element, context) {
            $(element).click(function () {
                var dialog = BatchUploadControl._dialog;

                if (dialog == null) {
                    var filter = context.fileManager.filter();
                    var fileTypeExts = '';
                    if (filter) {
                        var exts = filter.split(';');
                        $.each(exts, function () {
                            if (this.indexOf('.') === 0) {
                                fileTypeExts += '*' + this;
                            } else {
                                fileTypeExts += this;
                            }
                            fileTypeExts += ';';
                        });
                    }

                    dialog = new sig.ui.BatchUploadDialog({
                        aspNetAuth: context.fileManager.option('aspNetAuth'),
                        bucketId: context.fileManager.bucketId(),
                        fileTypeExts: fileTypeExts,
                        onQueueComplete: function (result) {
                            context.fileManager.refresh(function () {
                                var manager = this;
                                $.each(result.files, function () {
                                    manager.selectEntry(this.fileName);
                                });
                            });
                        }
                    });

                    dialog.init();
                    BatchUploadControl._dialog = dialog;
                }

                dialog.bucketId(context.fileManager.bucketId());
                dialog.folder(context.fileManager.currentPath());

                dialog.open();
            });
        }
    };

    var SwitchBucketControl = {
        create: function (context) {
            return '<select class="fm-buckets-dropdown"></select>';
        },
        init: function (element, context) {
            context.fileManager.loadBuckets(function (buckets) {
                var html = '';

                $.each(buckets, function () {
                    var selected = false;
                    if (!context.fileManager.bucketId()) {
                        selected = this.IsDefault;
                    } else if (context.fileManager.bucketId() === this.BucketId) {
                        selected = true;
                    }

                    html += '<option value="' + this.BucketId + '"' + (selected ? ' selected' : '') + '>' + this.DisplayName + '</option>';
                });

                $(element).html(html);
                $(element).change(function () {
                    context.fileManager.switchBucket($(this).val());
                });
            });
        }
    };

})(jQuery);