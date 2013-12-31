(function ($) {

    var sig = window.sig = window.sig || {};
    sig.ui = sig.ui || {};

    sig.ui.UploadZone = function (element, options) {
        var _this = this;
        var _$element = $(element);
        var _uploaderId = '__uploader_' + new Date().getTime();
        var _uploader = null;

        var defaultOptions = {
            folder: _$element.data('folder'),
            autoRename: _$element.data('autorename')
        };

        var _options = $.extend(true, defaultOptions, options);

        this.init = function () {
            var $uploader = '<input type="file" id="' + _uploaderId + '" style="display:none" />';
            $(document.body).append($uploader);
            _uploader = new sig.ui.FileUpload($uploader, {
                autoUpload: true,
                pasteZone: _$element,
                queue: false,
                onUploadSuccess: function (data) {
                    _$element.val(data.result.publicUri);

                    sig.ui.Message.hide();

                    if (_options.onUploadSuccess) {
                        _options.onUploadSuccess.apply(_this, [data]);
                    }
                },
                onUploadError: function (data) {
                    sig.ui.Message.error(data.message);

                    if (_options.onUploadError) {
                        _options.onUploadError.apply(_this, [data]);
                    }
                },
                onUploadProgress: function (data) {
                    sig.ui.Message.show('Uploading... ' + data.progress + '%');

                    if (_options.onUploadProgress) {
                        _options.onUploadProgress.apply(_this, [data]);
                    }
                },
                formData: {
                    folder: _options.folder || '/',
                    autoRename: _options.autoRename === undefined ? true : _options.autoRename
                }
            });
            _uploader.init();
        }
    };

    sig.ui.UploadZone.init = function (element, options) {
        var zone = new sig.ui.UploadZone(element, options);
        zone.init();
        return zone;
    };

})(jQuery);