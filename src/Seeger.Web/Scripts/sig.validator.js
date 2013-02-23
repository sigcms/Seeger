// Requires: jquery.sigvalidate.js

(function ($) {

    var Validator = {
        validate: function (selector) {
            var $form = $(selector);

            var summary = "";

            $form.sigvalidate({
                onItemValidated: function (element, isItemValid) {
                    if (!isItemValid) {
                        var $element = $(element);

                        var msg = $element.attr("errormsg");

                        if (!msg && ErrorMessages != undefined) {
                            var msgKey = $element.attr("msgkey");
                            if (!msgKey) {
                                msgKey = $element.attr("id");
                            }
                            if (msgKey) {
                                msg = ErrorMessages[msgKey];
                            }
                        }

                        if (msg) {
                            if (summary.length > 0) {
                                summary += "\n";
                            }
                            summary += "* " + msg;
                        }
                    }
                },
                onValidated: function (isValid) {
                    if (!isValid) {
                        Sig.Window.alert(summary);
                        summary = "";
                    }
                }
            });

            return $form.sigvalidate("validate");
        }
    };

    window.Sig.Validator = Validator;

})(jQuery);