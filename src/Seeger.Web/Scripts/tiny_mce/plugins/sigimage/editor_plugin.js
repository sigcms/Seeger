
(function () {
    var editor = null;

    var dialog = new sig.ui.SelectFileDialog({
        filter: '.jpg;.jpeg;.png;.gif',
        allowMultiSelect: true,
        onOK: function (files) {
            var dom = editor.dom;

            for (var i = 0, len = files.length; i < len; i++) {
                var file = files[i];
                editor.execCommand('mceInsertContent', false,
                    dom.createHTML('img', {
                        src: file.virtualPath
                    }));
            }

            dialog.close();
        }
    });

    tinymce.create('tinymce.plugins.SigImagePlugin', {
        init: function (ed, url) {
            editor = ed;

            // Register commands
            ed.addCommand('mceSigImage', function () {
                // Internal image object like a flash placeholder
                if (ed.dom.getAttrib(ed.selection.getNode(), 'class').indexOf('mceItem') != -1) return;

                dialog.open();
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
                authorurl: 'http://www.sigcms.com',
                infourl: 'http://www.sigcms.com',
                version: tinymce.majorVersion + "." + tinymce.minorVersion
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('sigimage', tinymce.plugins.SigImagePlugin);
})();