$(function () {
    initTemplateList();
    initLayoutList();
    initThemeList();

    var template = $templateList.val();
    var layout = $currentLayout.val();
    var skin = $currentSkin.val();

    changeTemplate(template);
    selectLayout(layout || $('#layout-list-container .image-item:visible:first').attr('layout-name'));
    selectTheme(skin || $('#skin-list-container .image-item:visible:first').attr('skin-name'));
});

// Save Callback
function onSaved(name, id, parentId, isEditing, iconUrl, pageInfo) {
    var pageTree = window.parent.pageTree;
    if (!isEditing) {
        var data = {
            Text: buildNodeText(name, id, iconUrl, pageInfo),
            Value: id,
            Encoded: false
        };
        if (parentId > 0) {
            pageTree.add(data, window.parent.jQuery("#page-" + parentId).closest(".t-item"));
        } else {
            pageTree.add(data);
        }

        window.parent.hideNoPageHint();
    } else {
        var node = window.parent.jQuery("#page-" + id).parent();
        node.html(buildNodeText(name, id, iconUrl, pageInfo));
    }

    window.parent.Sig.Window.close('Dialog');

    window.parent.updateCurrentPage();
    window.parent.message(Messages.SaveSuccess);

    function buildNodeText(name, id, iconUrl, pageInfo) {
        return "<img class='t-image' src='" + iconUrl + "' alt='' />"
                       + "<span id='page-" + id + "' class='node-text'>" + name + "</span>"
                       + "<input class='pageinfo' type='hidden' name='pageinfo' value='" + pageInfo + "' />";
    }
}

// Layout / Theme Selector
var lastTemplate = null;

function initTemplateList() {
    $templateList.change(function () {
        changeTemplate(this.value);
    });
}

function changeTemplate(name) {
    if (lastTemplate != null) {
        $(".image-item-selector[template-name='" + lastTemplate + "']").hide();
    }

    lastTemplate = name;

    var $layouts = $(".image-item-selector[template-name='" + name + "']");
    $layouts.show();

    selectLayout($layouts.filter(':first').find(".image-item:first").attr('layout-name'));

    $skinListContainer.find('.image-item-selector').hide();

    var $skins = $skinListContainer.find('.image-item-selector[template-name="' + name + '"]');
    $skins.show();

    if ($skins.length == 0) {
        $('#skin-row').hide();
        selectTheme(null);
    } else {
        $('#skin-row').show();
        selectTheme($skins.filter(':first').find('.image-item:first').attr('skin-name'));
    }
}

function initLayoutList() {
    $layoutListContainer.find(".image-item").click(function () {
        var $this = $(this);
        selectLayout($this.attr("layout-name"));
        $this.blur();
        return false;
    });
}

function initThemeList() {
    $skinListContainer.find(".image-item").click(function () {
        var $this = $(this);
        selectTheme($this.attr("skin-name"));
        $this.blur();
        return false;
    });
}

var $selectedLayout = null;

function selectLayout(name) {
    var $layout = $layoutListContainer.find(".image-item[layout-name='" + name + "']");
    selectLayoutElement($layout);
    $currentLayout.val($selectedLayout.attr("layout-name"));
}

function selectLayoutElement($element) {
    if ($selectedLayout != null) {
        $selectedLayout.removeClass("selected");
    }
    $selectedLayout = $element;
    $selectedLayout.addClass("selected");
}

var $selectedTheme = null;
function selectTheme(name) {
    if (name != null) {
        var $theme = $skinListContainer.find(".image-item[skin-name='" + name + "']");
        selectThemeElement($theme);
    } else {
        selectThemeElement(null);
    }
    $currentSkin.val(name);
}
function selectThemeElement($element) {
    if ($selectedTheme != null) {
        $selectedTheme.removeClass("selected");
    }

    if ($element != null && $element.length > 0) {
        $selectedTheme = $element;
        $selectedTheme.addClass("selected");
    } else {
        $selectedTheme = null;
    }
}