
(function () {
    var dialog = new InsertHtmlDialog();

    function InsertHtmlDialog() {
        var _this = this;
        var _tinyMceEditor = null;
        var _dialog = new sig.ui.Dialog();

        _dialog.init({
            title: sig.Resources.get('Insert HTML'),
            width: 450,
            height: 280,
            buttons: [
                {
                    text: sig.Resources.get('Cancel'),
                    click: function () {
                        _this.clear();
                        _this.close();
                    }
                },
                {
                    text: sig.Resources.get('Insert'),
                    click: function () {
                        _this.commit();
                        _this.close();
                    }
                }
            ]
        });
        _dialog.html('<div><textarea rows="4" cols="40" style="width:415px;height:150px"></textarea></div>');

        this.editor = function (editor) {
            if (arguments.length === 0) {
                return _tinyMceEditor;
            }

            _tinyMceEditor = editor;
        }

        this.html = function (html) {
            if (arguments.length === 0) {
                return _dialog.find('textarea').val();
            }

            _dialog.find('textarea').val(html);
        }

        this.commit = function () {
            if (!_tinyMceEditor)
                throw new Error('TinyMCE Editor must be set first.');

            var html = _this.html();
            if (html) {
                _tinyMceEditor.execCommand('mceInsertContent', false, '<div>' + html + '</div>');
            }
        }

        this.clear = function () {
            _this.html('');
        }

        this.open = function () {
            _this.html('');
            _dialog.open();
        }

        this.close = function () {
            _dialog.close();
        }
    }
    
    tinymce.create('tinymce.plugins.InsertHtmlPlugin', {
        init: function (ed, url) {
            // Register commands
            ed.addCommand('mceInsertHtml', function () {
                // Internal image object like a flash placeholder
                if (ed.dom.getAttrib(ed.selection.getNode(), 'class').indexOf('mceItem') != -1) return;

                dialog.editor(ed);
                dialog.open();
            });

            // Register buttons
            ed.addButton('inserthtml', {
                title: sig.Resources.get('Insert HTML'),
                cmd: 'mceInsertHtml'
            });
        },

        getInfo: function () {
            return {
                longname: 'Insert HTML',
                author: 'Seeger CMS Team',
                authorurl: 'http://www.sigcms.com',
                infourl: 'http://www.sigcms.com',
                version: tinymce.majorVersion + "." + tinymce.minorVersion
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('inserthtml', tinymce.plugins.InsertHtmlPlugin);
})();