(function ($) {

    var sig = window.sig = window.sig || {};
    sig.ui = sig.ui || {};

    sig.ui.SelectFileButton = {
        init: function (button, options) {
            $(button).each(function () {
                var $button = $(this);

                var filter = $button.data('filter');
                var aspNetAuth = $button.data('aspNetAuth');

                var dialogOptions = {
                    filter: filter,
                    aspNetAuth: aspNetAuth,
                    allowMultiSelect: false,
                    onOK: function (files) {
                        var updateTarget = $button.data('update-target');
                        if (updateTarget) {
                            var updateTargetAttr = $button.data('update-target-attr') || 'value';
                            $(updateTarget).attr(updateTargetAttr, files[0].virtualPath);
                        }
                    }
                };

                if (options) {
                    dialogOptions = $.extend(true, dialogOptions, options);
                }

                var dialog = new sig.ui.SelectFileDialog(dialogOptions);

                $button.data('SelectFileDialog', dialog);

                $button.click(function () {
                    $(this).data('SelectFileDialog').open();
                    return false;
                });
            });
        }
    };

    $(function () {
        sig.ui.SelectFileButton.init($('[data-toggle="select-file"]'));
    });

})(jQuery);