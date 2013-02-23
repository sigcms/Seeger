/*
* Sigvalidate: A jquery validation plugin
*
* Copyright (c) 2011 Seeger CMS http://www.seegercms.com
*
* Examples:
* 
* Init: $("selector").sigvalidate();
* Validate Form: var isValid = $("selector").sigvalidate("validate");
*
*/

(function ($) {
    $.fn.extend({
        sigvalidate: function (options, value) {

            function Validator($form, options) {
                // Settings
                var defaults = {
                    errorClassName: "error",
                    onItemValidated: function (element, isItemValid) { },
                    onValidated: function (isValid) { }
                };
                options = $.fn.extend({}, defaults, options);

                this.validate = function () {
                    var validCount = 0;
                    var errorCount = 0;

                    $form.find("input[type=text],input[type=password],input[type=file],select,.checkboxlist,.radiolist").filter(function () {
                        var $this = $(this);
                        return $this.attr("className") && $this.is(":visible");
                    }).each(function () {
                        var $this = $(this);
                        if ($this.attr("className")) {
                            var classes = $this.attr("className").split(' ');

                            for (var i = 0, len = classes.length; i < len; ++i) {
                                var method = $.sigvalidate.methods[classes[i]];
                                if (method) {
                                    // if itemValid != true && itemValid != false, that means the element is not validatable
                                    var itemValid = method(this);

                                    if (typeof itemValid == "boolean") {
                                        if (!itemValid) {
                                            ++errorCount;
                                            $this.addClass(options.errorClassName);
                                        } else {
                                            ++validCount;
                                            $this.removeClass(options.errorClassName);
                                        }

                                        if (options.onItemValidated) {
                                            options.onItemValidated(this, itemValid);
                                        }
                                    }
                                }
                            }
                        }
                    });

                    var isValid = (errorCount == 0);

                    if (options.onValidated) {
                        options.onValidated(isValid);
                    }

                    return isValid;
                }
            }

            // Static Helpers
            Validator.get = function ($form) {
                var key = $form.attr("sigvalidate-key");
                if (key) {
                    return window._sigvalidators[key];
                }

                return null;
            }
            Validator.set = function ($form, validator) {
                var key = "sigvalidate" + new Date().getTime();
                $form.attr("sigvalidate-key", key);

                if (window._sigvalidators === undefined) {
                    window._sigvalidators = {};
                }
                window._sigvalidators[key] = validator;
            }

            var returnValue = this;

            this.each(function () {

                var $form = $(this);

                // Handle method calls
                if (typeof options == "string") {
                    var validator = Validator.get($form);
                    if (!validator) {
                        return;
                    }

                    var member = validator[options];
                    if (typeof member == "function") {
                        var result = member(value);
                        if (result !== undefined) {
                            returnValue = result;
                        }
                    } else if (value !== undefined) {
                        validator[options] = value;
                    } else {
                        returnValue = validator[options];
                    }
                } else if (Validator.get($form) == null) {
                    Validator.set($form, new Validator($form, options));
                }
            });

            return returnValue;
        }
    });

    $.extend({
        sigvalidate: {
            methods: {},
            addMethod: function (name, method) {
                $.sigvalidate.methods[name] = method;
            }
        }
    });

    // Built-in validation methods
    $.sigvalidate.addMethod("required", function (element) {
        var $element = $(element);
        var tag = $element.attr("tagName").toUpperCase();

        var isValid = true;

        if (tag == "INPUT") {
            var type = $element.attr("type");

            if (type == "text" || type == "password" || type == "file") {
                isValid = $.trim($element.val()).length > 0;
            } else if (type == "radio" || type == "checkbox") {
                isValid = $element.is(":checked");
            }
        } else if (tag == "SELECT") {
            isValid = $.trim($element.val()).length > 0;
        } else if ($element.hasClass("checkboxlist")) {
            isValid = $element.find("input[type=checkbox]:checked").length > 0;
        } else if ($element.hasClass("radiolist")) {
            isValid = $element.find("input[type=radio]:checked").length > 0;
        } else { // Not validatable
            isValid = null;
        }

        return isValid;
    });

    var Regexs = {
        Number: /^\d*$/,
        PositiveNumber: /^[0-9]*[1-9][0-9]*$/,
        Email: /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/
    };

    $.sigvalidate.addMethod("number", function (element) {
        var value = extractElementValue($(element));

        if (value != null) {
            var isValid = true;

            if (value.length > 0) {
                isValid = Regexs.Number.test(value);
            }

            return isValid;
        }
    });

    $.sigvalidate.addMethod("positive-number", function (element) {
        var value = extractElementValue($(element));

        if (value != null) {
            var isValid = true;

            if (value.length > 0) {
                isValid = Regexs.PositiveNumber.test(value);
            }

            return isValid;
        }
    });

    $.sigvalidate.addMethod("email", function (element) {
        var value = extractElementValue($(element));

        if (value != null) {
            var isValid = true;

            if (value.length > 0) {
                isValid = Regexs.Email.test(value);
            }

            return isValid;
        }
    });

    function extractElementValue($element) {
        var tag = $element.attr("tagName").toUpperCase();
        var value = null;

        if (tag == "INPUT") {
            var type = $element.attr("type");
            if (type == "text" || type == "password") {
                value = $.trim($element.val());
            }
        } else if (tag == "SELECT") {
            value = $element.val();
        }

        return value;
    }

})(jQuery);