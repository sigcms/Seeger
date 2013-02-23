
(function () {
    tinymce.create('tinymce.plugins.SigImagePlugin', {
        init: function (ed, url) {
            // Register commands
            ed.addCommand('mceSigImage', function () {
                // Internal image object like a flash placeholder
                if (ed.dom.getAttrib(ed.selection.getNode(), 'class').indexOf('mceItem') != -1)
                    return;

                ed.windowManager.open({
                    file: url + '/Dialog.aspx',
                    width: 640 + parseInt(ed.getLang('sigimage.delta_width', 0)),
                    height: 330 + parseInt(ed.getLang('sigimage.delta_width', 0)),
                    inline: 1
                }, {
                    plugin_url: url
                });
            });

            // Register buttons
            ed.addButton('sigimage', {
                title: 'sigimage.sigimage_desc',
                cmd: 'mceSigImage'
            });
        },

        getInfo: function () {
            return {
                longname: 'Seeger image',
                author: 'Seeger CMS',
                authorurl: 'http://www.seegercms.com',
                infourl: 'http://www.seegercms.com',
                version: tinymce.majorVersion + "." + tinymce.minorVersion
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('sigimage', tinymce.plugins.SigImagePlugin);
})();