(function ($) {

    $.extend($.fn, {
        filebrowser: function (options) {

            var settings = $.fn.extend({}, {
                root: '/',
                serverHandler: 'FileBrowser.ashx',
                fileClick: null,
                directoryLoaded: null,
                text_currentPath: 'Current Path'
            }, options);

            var $browser = $(this);
            var $selectedFile = null;
            var currentDirectory = settings.root;

            // Create basic dom structure
            $browser.append($("<div class='fb-browser-main'></div>"));

            // First load
            loadDirectory(currentDirectory);

            // Init click events
            $browser.find(".fb-folder-panel ul li > a").live("click", function () {
                loadDirectory($(this).attr("rel"));
                return false;
            });
            $browser.find(".fb-file-panel ul li > a").live("click", function () {
                if ($selectedFile != null) {
                    $selectedFile.removeClass("fb-selected");
                }
                $selectedFile = $(this);
                $selectedFile.addClass("fb-selected");

                if (settings.fileClick) {
                    settings.fileClick.apply(this, [$selectedFile.attr("rel"), $selectedFile.html()]);
                }

                return false;
            });

            function loadDirectory(dir) {

                currentDirectory = dir;

                $browser.find(".fb-folder-panel, .fb-file-panel").html("<span class='fb-loading'></span>");
                $browser.find(".fb-browser-main").load(appendQueryString(settings.serverHandler, "dir=" + dir + "&r=" + new Date().getTime()), function () {
                    if (settings.directoryLoaded) {
                        settings.directoryLoaded(dir);
                    }
                    $selectedFile = null;
                });
            }

            function addFile(fullFileName, setSelected) {
                var ext = getExtension(fullFileName);
                if (ext.length > 1) {
                    ext = ext.substr(1);
                } else {
                    ext = "";
                }

                var css = "fb-file";
                if (ext.length > 0) {
                    css += " fb-" + ext;
                }

                var $newElement = $("<li><a href='#' rel='" + fullFileName + "' class='" + css + "'>" + getFileName(fullFileName) + "</a></li>");
                $browser.find(".fb-file-panel > ul").append($newElement);

                if (setSelected) {
                    if ($selectedFile != null) {
                        $selectedFile.removeClass("fb-selected");
                    }
                    $selectedFile = $newElement.find("a");
                    $selectedFile.addClass("fb-selected");
                }
            }

            function getExtension(fileName) {
                var index = fileName.lastIndexOf('/');
                if (index > 0) {
                    fileName = fileName.substr(index + 1);
                }
                index = fileName.lastIndexOf('.');
                return fileName.substr(index);
            }

            function getFileName(fullFilePath) {
                var index = fullFilePath.lastIndexOf('/');
                if (index > 0) {
                    return fullFilePath.substr(index + 1);
                }

                return fullFilePath;
            }

            function appendQueryString(url, queryString) {
                if (url.indexOf('?') > 0) {
                    return url + '&' + queryString;
                }
                return url + '?' + queryString;
            }

            return {
                addFile: addFile
            };
        }
    });

})(jQuery);