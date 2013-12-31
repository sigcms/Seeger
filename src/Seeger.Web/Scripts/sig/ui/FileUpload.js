(function ($) {

    var sig = window.sig = window.sig || {};
    sig.ui = sig.ui || {};

    sig.ui.FileUpload = function (element, options) {
        var _this = this;
        var _$element = $(element);
        var _options = $.extend(true, {}, sig.ui.FileUpload.defaults, options);
        var _queue = new FileUploadQueue(options.queue ? $(options.queue) : null, _this);
        var _filesByName = {};
        
        this.init = function () {
            var options = toUnderlyingUploaderOptions(_options);

            options.add = function (e, data) {
                $.each(data.files, function () {
                    if (!this.name) {
                        var ext = '.jpg';

                        if (this.type) {
                            ext = this.type.substr(this.type.indexOf('/') + 1);
                        }

                        this.name = 'Screenshot-' + new Date().getTime() + '.' + ext;
                    }

                    _filesByName[this.name] = {
                        file: this,
                        data: data
                    };
                });

                _queue.add(data.files);

                if (_options.autoUpload) {
                    data.submit();
                }
            };

            options.progress = function (e, data) {
                var fileCount = data.files.length;

                $.each(data.files, function () {
                    var total = data.total / fileCount;
                    var uploaded = data.loaded / fileCount;
                    var progress = sig.ui.FileUpload.computeProgress(uploaded, total);
                    _queue.onUploadProgress(this.name, uploaded, total);
                    _options.onUploadProgress({
                        file: this,
                        totalBytes: total,
                        uploadedBytes: uploaded,
                        progress: progress
                    });
                });
            };

            options.progressall = function (e, data) {
                _options.onQueueProgress({
                    totalBytes: data.total,
                    uploadedBytes: data.loaded,
                    progress: sig.ui.FileUpload.computeProgress(data.loaded, data.total)
                });
            };

            options.done = function (e, data) {
                if (data.result.success) {
                    $.each(data.files, function () {
                        _options.onUploadSuccess({
                            file: this,
                            result: data.result.data
                        });
                        _queue.onUploadSuccess(this.name);
                    });
                } else {
                    $.each(data.files, function () {
                        _options.onUploadError({
                            file: this,
                            message: data.result.message
                        });
                        _queue.onUploadError(this.name, data.result.message);
                    });
                }
            };

            options.stop = function () {
                _options.onQueueComplete();
            };

            _$element.fileupload(options);
        }

        this.remove = function (fileName) {
            _queue.remove(fileName);
            delete _filesByName[fileName];
        }

        this.startUpload = function () {
            var datas = [];
            for (var fileName in _filesByName) {
                var entry = _filesByName[fileName];
                if (!_.any(datas, function (x) { return x === entry.data; })) {
                    datas.push(entry.data);
                }
            }

            $.each(datas, function () {
                this.submit();
            });
        }

        function toUnderlyingUploaderOptions(options) {
            return {
                url: options.handler,
                dataType: 'json',
                autoUpload: options.autoUpload,
                formData: options.formData
            };
        }
    };

    sig.ui.FileUpload.computeProgress = function (uploaded, total) {
        return parseInt(uploaded / total * 100, 10);
    };

    sig.ui.FileUpload.defaults = {
        handler: '/Admin/Handlers/FileUpload.ashx',
        onUploadSuccess: function (data) { },
        onUploadError: function (data) { },
        onUploadProgress: function (data) { },
        onQueueProgress: function (data) { },
        onQueueComplete: function (data) { }
    };

    function FileUploadQueue(element, fileupload) {
        var _this = this;
        var _$element = element ? $(element) : null;
        var _fileupload = fileupload;
        var _filesByName = {};

        this.add = function (files) {
            $.each(files, function () {
                var $item = null;

                if (_$element) {
                    $item = $(createItemHtml(this));
                    _$element.append($item);

                    $item.data('fileupload-file', this);

                    // init
                    $item.find('.fileupload-cancel').click(function (e) {
                        var $item = $(this).closest('.fileupload-queue-item');
                        var file = $item.data('fileupload-file');
                        _fileupload.remove(file.name);
                        e.preventDefault();
                    });
                }

                _filesByName[this.name] = {
                    file: this,
                    $element: $item
                };
            });
        }

        this.remove = function (fileName) {
            var entry = _filesByName[fileName];
            if (entry) {
                if (entry.$element) {
                    entry.$element.remove();
                }
                delete _filesByName[fileName];
            }
        }

        this.onUploadSuccess = function (fileName) {
            var entry = _filesByName[fileName];
            if (entry.$element) {
                entry.$element.remove();
            }
        }

        this.onUploadProgress = function (fileName, uploadedBytes, totalBytes) {
        }

        this.onUploadError = function (fileName, error) {
            var entry = _filesByName[fileName];
            if (entry.$element) {
                setItemError(entry.$element, error);
            }
        }

        function setItemError($item, error) {
            var $status = $item.find('.fileupload-queue-item-status');
            if ($status.length === 0) {
                $status = $('<div class="fileupload-queue-item-status" />');
                $item.append($status);
            }

            $status.html(error).addClass('status-error');
        }

        function createItemHtml(file) {
            return '<div class="fileupload-queue-item">'
                        + file.name
                        + '<a href="#" class="fileupload-cancel">&times;</a>'
                 + '</div>';
        }
    };

})(jQuery);