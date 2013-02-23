
(function () {
    tinymce.create('tinymce.plugins.SigDownloadPlugin', {
        init: function (ed, url) {
            // Register commands
            ed.addCommand('mceSigDownload', function () {
                // Internal image object like a flash placeholder
                if (ed.dom.getAttrib(ed.selection.getNode(), 'class').indexOf('mceItem') != -1)
                    return;

                ed.windowManager.open({
                    file: url + '/Dialog.aspx',
                    width: 655 + parseInt(ed.getLang('sigdownload.delta_width', 0)),
                    height: 320 + parseInt(ed.getLang('sigdownload.delta_height', 0)),
                    inline: 1
                }, {
                    plugin_url: url
                });
            });

            // Register buttons
            ed.addButton('sigdownload', {
                title: 'sigdownload.sigdownload_desc',
                cmd: 'mceSigDownload'
            });
        },

        getInfo: function () {
            return {
                longname: 'Seeger download',
                author: 'Seeger CMS',
                authorurl: 'http://www.seegercms.com',
                infourl: 'http://www.seegercms.com',
                version: tinymce.majorVersion + "." + tinymce.minorVersion
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('sigdownload', tinymce.plugins.SigDownloadPlugin);
})();