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
            folder: '/Files',
            aspNetAuth: window.aspNetAuth || null,
            buttonText: 'Select files',
            uploadifyOptions: {
                swf: '/Scripts/uploadify/uploadify.swf',
                uploader: '/Admin/Handlers/FileUpload.ashx',
                multi: true,
                auto: false,
                onUploadSuccess: onUploadSuccess,
                onQueueComplete: onQueueComplete
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

        this.open = function () {
            _dialog.html('<div class="batch-upload-dialog-body"><input type="file" id="' + _uploaderId + '" /></div>');
            _dialog.open();

            var options = $.extend(true, {}, _options.uploadifyOptions);
            options.buttonText = sig.Resources.get(_options.buttonText);
            options.fileTypeExts = _options.fileTypeExts;
            options.formData = options.formData || {};
            options.formData.aspNetAuth = _options.aspNetAuth;
            options.formData.folder = _options.folder;

            _$uploadify = _dialog.find('#' + _uploaderId).uploadify(options);
        }

        this.startUpload = function () {
            _uploadContext = {
                files: []
            };
            _$uploadify.uploadify('upload', '*');
        }

        this.close = function () {
            _dialog.close();
        }

        function onUploadSuccess(file, result, response) {
            result = JSON.parse(result);

            if (result.success) {
                _uploadContext.files.push({
                    success: true,
                    fileName: result.data.fileName,
                    virtualPath: result.data.virtualPath
                });
            } else {
                _uploadContext.files.push({
                    success: false,
                    fileName: result.data.fileName,
                    virtualPath: result.data.virtualPath,
                    error: result.message
                });
            }
        }

        function onQueueComplete() {
            if (_options.onQueueComplete) {
                _options.onQueueComplete.apply(_this, [{ files: _uploadContext.files }]);
            }
            _uploadContext = null;
            _this.close();
        }
    };

})(jQuery);