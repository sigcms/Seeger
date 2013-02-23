(function ($) {

    var baseUrl = "http://www.sigcms.com/plugins/officialsite/api/";

    var ProductService = {
        checkUpdates: function (culture, callback) {
            var url = baseUrl + "CheckUpdates.ashx?culture=" + culture + "&callback=?";
            $.getJSON(url, callback);
        },
        getSeegerNews: function (culture, callback) {
            var url = baseUrl + "News.ashx?culture=" + culture + "&callback=?";
            $.getJSON(url, callback);
        }
    };

    window["ProductService"] = ProductService;

})(jQuery);