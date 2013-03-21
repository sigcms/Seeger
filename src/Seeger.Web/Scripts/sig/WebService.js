(function ($) {

    var sig = window.sig = window.sig || {};

    sig.WebService = {
        invoke: function (servicePath, parameters, success, error) {
            var data = null;

            if (parameters) {
                data = JSON.stringify(parameters);
            }

            $.ajax({
                url: servicePath,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: data,
                success: function (data, textStatus, xhr) {
                    if (success) {
                        if (data.d) {
                            data = data.d;
                        }
                        success(data, textStatus, xhr);
                    }
                },
                error: function (xhr) {
                    var err = $.parseJSON(xhr.responseText);
                    if (error) {
                        error(err);
                    } else {
                        sig.WebService.defaultErrorHandler(err);
                    }
                }
            });
        },
        defaultErrorHandler: function (err) {
            sig.ui.Message.error(err.get_message());
        }
    };

})(jQuery);