//prevent conflicts by wrapping plugin
(function($) {
    //private function (by scope)
    function QueryStringParser() {
        this.load();
    }

    //add methods to the QueryStringParser class
    $.extend(QueryStringParser.prototype, {
        load: function(suppliedQueryString) {
            //create a new object
            this.Values = new Object();

            //find the index of ? in the supplied string (for testing) or the current url
            var queryString = suppliedQueryString || document.URL;
            var qsIndex = (suppliedQueryString || document.URL).indexOf('?');
            //no query string...return
            if (qsIndex < 0)
                return;

            //strip out the query string only
            queryString = queryString.substring(qsIndex);            
            //no query string...no problem
            if (queryString.length <= 1) {
                return;
            }

            //split into pairs
            var pairs = queryString.split('&');
            //foreach pair
            for (var i = 0; i < pairs.length; i++) {
                //set the value (decode string just in case)
                this.Values[pairs[i].split('=')[0].toLowerCase()] = decodeURIComponent(pairs[i].split('=')[1]);
            }
        },
        get: function(key) {
            return (this.Values[key.toLowerCase()]) ? this.Values[key.toLowerCase()] : '';
        },
        set: function(key, value) {
            this.Values[key.toLowerCase()] = value;
            //chain...in true jQuery fashion
            return this;
        }
    });

    //the $.extend method doesn't appear to allow overriding toString on prototypes
    QueryStringParser.prototype.toString = function() {
        var params = [];

        for (var prop in this.Values) {
            //add the encoded value
            params.push(prop + "=" + encodeURIComponent(this.Values[prop]));
        }

        //return a complete query string
        return '?' + params.join('&');
    };

    //set the global property
    $.Params = new QueryStringParser();
})(jQuery);