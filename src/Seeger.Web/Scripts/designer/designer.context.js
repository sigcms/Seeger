(function ($) {
    function Context() {
        var _culture = null;

        this.init = function () {
            _culture = $.Params.get('page-culture');
        }

        this.get_installPath = function () {
            return Sig.Designer._installPath;
        }

        this.get_pageLiveUrl = function () {
            return Sig.Designer._pageLiveUrl;
        }

        this.get_culture = function () {
            return _culture;
        }

        this.get_pageId = function () {
            return Sig.Designer._pageId;
        }

        this.get_pageTemplate = function () {
            return Sig.Designer._pageTemplate;
        }

        this.get_pageModule = function () {
            return Sig.Designer._pageModule;
        }
    }

    // Public APIs
    window.Sig.Context = Context;

})(jQuery);