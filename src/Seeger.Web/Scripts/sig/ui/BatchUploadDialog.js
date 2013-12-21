(function ($) {

    var sig = window.sig = window.sig || {};
    sig.ui = sig.ui || {};

    sig.ui.BatchUploadDialog = function (options) {
        var _this = this;
        var _dialog = new sig.ui.Dialog();
        var _uploaderId = '__batch_file_upload_' + new Date().getTime();
        var _$uploadify = null;

        var _uploadContext = null;

        var _options = {
            onQueueComplete: null,
            folder: '/',
            bucketId: null,
            aspNetAuth: window.aspNetAuth || null,
            buttonText: 'Select files',
            uploadifyOptions: {
                swf: '/Scripts/uploadify/uploadify.swf',
                uploader: '/Admin/Handlers/FileUpload.ashx',
                multi: true,
                auto: false,
                onUploadSuccess: onUploadifyUploadSuccess,
                onUploadError: onUploadifyUploadError,
                onQueueComplete: onUploadifyQueueComplete
            }
        };

        if (options) {
            _options = $.extend(true, _options, options);
        }

        if (!_options.aspNetAuth)
            throw new Error('Unable to get aspNetAuth value.');

        this.init = function () {
            _dialog.init({
                minWidth: 500,
                minHeight: 300,
                buttons: [
                    {
                        text: sig.Resources.get('Cancel'),
                        click: function () {
                            _this.close();
                        }
                    },
                    {
                        text: sig.Resources.get('Upload'),
                        click: function () {
                            _this.startUpload();
                        }
                    }
                ],
                close: function () {
                    _dialog.html('');
                }
            });
        }

        this.folder = function (path) {
            if (arguments.length === 0) {
                return _options.folder;
            }
            _options.folder = path;
        }

        this.bucketId = function (bucketId) {
            if (arguments.length === 0) {
                return _options.bucketId;
            }
            _options.bucketId = bucketId;
        }

        this.open = function () {
            _dialog.html('<div class="batch-upload-dialog-body" style="position:relative">'
                            + '<input type="file" id="' + _uploaderId + '" />'
                            + '<div style="position:absolute;right:0;top:10px">'
                                + '<input class="auto-rename-checkbox" checked="checked" style="vertical-align:middle;display:inline-block" type="checkbox" id="' + _uploaderId + '-' + 'auto-rename" /> '
                                + '<label style="vertical-align:middle;display:inline-block" for="' + _uploaderId + '-' + 'auto-rename">' + sig.Resources.get('Auto rename file') + '</label>'
                            + '</div>'
                       + '</div>');
            _dialog.open();

            var options = $.extend(true, {}, _options.uploadifyOptions);
            options.buttonText = sig.Resources.get(_options.buttonText);
            options.fileTypeExts = _options.fileTypeExts;
            options.formData = options.formData || {};
            options.formData.aspNetAuth = _options.aspNetAuth;
            options.formData.bucketId = _options.bucketId;
            options.formData.folder = _options.folder;

            _$uploadify = _dialog.find('#' + _uploaderId).uploadify(options);
        }

        this.autoRename = function (value) {
            if (arguments.length === 0) {
                return _dialog.find('.auto-rename-checkbox').is(':checked');
            }

            if (value) {
                _dialog.find('.auto-rename-checkbox').attr('checked', 'checked');
            } else {
                _dialog.find('.auto-rename-checkbox').removeAttr('checked');
            }
        }

        this.startUpload = function () {
            _uploadContext = {
                files: [],
                errors: []
            };

            _$uploadify.uploadify('settings', 'formData', { autoRename: _this.autoRename() });
            _$uploadify.uploadify('upload', '*');
        }

        this.close = function () {
            _dialog.close();
        }

        function onUploadifyUploadSuccess(file, result, response) {
            result = JSON.parse(result);
            if (result.success) {
                _uploadContext.files.push({
                    fileName: result.data.fileName,
                    virtualPath: result.data.virtualPath
                });
            } else {
                _uploadContext.errors.push({
                    fileName: file.name,
                    error: result.message
                });
            }
        }

        function onUploadifyUploadError(file, errorCode, errorMsg, errorString) {
            _uploadContext.errors.push({
                fileName: file.name,
                error: errorString
            });
        }

        function onUploadifyQueueComplete() {
            if (_options.onQueueComplete) {
                var files = _uploadContext.files.slice(0);
                var errors = _uploadContext.errors.slice(0);

                if (errors.length > 0) {
                    var msg = '';
                    if (files.length > 0) {
                        msg += _.template(sig.Resources.get('{Count} succeeded'), { Count: files.length });
                    }
                    if (msg.length > 0) {
                        msg += ', ';
                    }
                    msg += _.template(sig.Resources.get('{Count} failed'), { Count: errors.length }) + '<br/>';
                    msg += _.map(errors, function (e) {
                        return e.fileName + ': ' + e.error;
                    })
                    .join('<br/>');

                    sig.ui.Message.error(msg);
                }

                _options.onQueueComplete.apply(_this, [{
                    files: files,
                    errors: errors
                }]);
            }
            _uploadContext = null;
            _this.close();
        }
    };

})(jQuery);