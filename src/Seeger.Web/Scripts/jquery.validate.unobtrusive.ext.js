$.validator.unobtrusive.reparse = function (selector) {
    var $container = $(selector);
    if ($container.is('form')) {
        removeFormValidation($container);
    }

    $container.find('form').each(function () {
        removeFormValidation(this);
    });

    $.validator.unobtrusive.parse($container);

    function removeFormValidation(form) {
        $(form).removeData('validator');
        $(form).removeData('unobtrusiveValidation');
    }
};