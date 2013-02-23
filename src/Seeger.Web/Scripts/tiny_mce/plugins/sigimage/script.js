var ImageDialog = {
    init: function (ed) {
        tinyMCEPopup.resizeToInnerSize();
    },

    insert: function (src, alt, width, height) {
        var ed = tinyMCEPopup.editor, dom = ed.dom;

        var style = '';
        if (width != undefined && width > 0) {
            style = 'width:' + width + 'px;';
        }
        if (height != undefined && height > 0) {
            style += 'height:' + height + 'px;';
        }

        tinyMCEPopup.execCommand('mceInsertContent', false, dom.createHTML('img', {
            src: src,
            alt: alt,
            style: style
        }));

        tinyMCEPopup.close();
    }
};

tinyMCEPopup.onInit.add(ImageDialog.init, ImageDialog);
