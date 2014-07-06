(function () {
    var sig = window.sig = window.sig || {};

    sig.UrlUtil = {
        combine: function () {
            if (arguments.length === 0) return null;
            if (arguments.length === 1) return arguments[0];

            var url = '';

            for (var i = 0, len = arguments.length; i < len; i++) {
                var segment = arguments[i];

                if (segment === null || segment === undefined || segment === '') continue;

                if (url.length == 0) {
                    url = segment;
                } else {
                    if (url[url.length - 1] !== '/') {
                        url += '/';
                    }
                    if (segment[0] === '/') {
                        segment = segment.substr(1);
                    }
                    url += segment;
                }
            }

            return url;
        },
        getDirectory: function (path) {
            if (path === null || path === undefined || path === '' || path === '/') return null;

            if (path[path.length - 1] === '/') {
                path = path.substr(0, path.length - 1);
            }

            var lastSlashIndex = path.lastIndexOf('/');
            if (lastSlashIndex < 0) {
                return null;
            }

            if (lastSlashIndex === 0) {
                return '/';
            }

            return path.substr(0, lastSlashIndex);
        },
        getExtension: function (path) {
            if (path === null || path === undefined || path === '') {
                return null;
            }

            var indexOfDot = path.lastIndexOf('.');
            if (indexOfDot < 0) {
                return null;
            }

            if (indexOfDot == path.length - 1) {
                return null;
            }

            return path.substr(indexOfDot);
        },
        getFileName: function (path) {
            if (!path) {
                return null;
            }

            var index = path.lastIndexOf('/');
            if (index >= 0) {
                var filename = path.substr(index + 1);
                return filename;
            }

            return null;
        }
    };
})();