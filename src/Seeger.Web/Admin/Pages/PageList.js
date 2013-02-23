var pageTree;
var $messageContainer;
var $propertyBody;
var $propertyPanelToolbar;
var $pageProperties;
var $noPageHint;

var Messages = {
    Success: null,
    Processing: null,
    Loading: null,
    None: null,
    DeletePageConfirm: null,
    DeleteSuccess: null,
    DialogTitle: {
        AddPage: null,
        EditPage: null
    }
};

var settings = {
    multilingual: false,
    pageExtension: '',
    installPath: '/'
};
var selectedPage = null;

// Init
$(function () {
    $noPageHint = $("#no-page-hint");
    initPageTree();
    initPropertyPanel();
});

function initPageTree() {
    $("#PageTree").tTreeView({
        onNodeDrop: onNodeDrop,
        onNodeDragStart: onNodeDragStart,
        onNodeDropped: onNodeDropped,
        onSelect: onSelect,
        dragAndDrop: { enabled: true }
    });

    pageTree = $("#PageTree").data("tTreeView");
}

/* Page Drag & Drop */

var dragContext = { $item: null, $prev: null, $next: null, $parent: null };
var dropContext = { dropPosition: null, $item: null };

function onNodeDrop(e) {
    if (!e.isValid) return;

    var srcPageId = getPageId(dragContext.$item);
    var targetPageId = getPageId($(e.destinationItem));

    if (srcPageId == targetPageId) return;

    message(Messages.Processing + "...");

    Seeger.Web.UI.Admin.Pages.Services.Move(srcPageId, targetPageId, e.dropPosition, function (result) {
        if (!result.Success) {
            message(result.Message, 3000, "error");
            ejectMovement();
        } else {
            message(Messages.Success);
        }
    });
}

function onNodeDropped(e) {
    dropContext.$item = $(e.item);
}

function onNodeDragStart(e) {
    dragContext.$item = $(e.item);
    dragContext.$prev = dragContext.$item.prev();
    dragContext.$next = dragContext.$item.next();
    dragContext.$parent = dragContext.$item.parents(".t-item:first");
}

function ejectMovement() {
    pageTree.remove(dropContext.$item);

    if (dragContext.$prev.length > 0) {
        pageTree.insertAfter(dragContext.$item, dragContext.$prev);
    } else if (dragContext.$next.length > 0) {
        pageTree.insertBefore(dragContext.$item, dragContext.$next);
    } else {
        pageTree.insertInside(dragContext.$item, dragContext.$parent);
    }
}

/* Page Info Panel */

var dropdownAction = null;

function initPropertyPanel() {
    $messageContainer = $("#content-no-selection");
    $propertyBody = $("#content-page-selected");
    $propertyPanelToolbar = $("#page-toolbar");
    $pageProperties = $("#page-properties");
    
    $("#pagePropertiesTemplate").template("PageProperties");

    $propertyPanelToolbar.find("> a[content]").dropdown({
        show: function () {
            dropdownAction = this.attr("href").substr(1);
        },
        hide: function () {
            dropdownAction = null;
        }
    });

    $("#page-toolbar > a").click(function() {
        var $this = $(this);
        if (!$this.attr("content")) {
            handleAction($this.attr("href").substr(1), $this);
        }
        return false;
    });

    $(".dropdown-content li > a").click(function () {
        handleAction(dropdownAction, $(this));
        return false;
    });
}

function onSelect(e) {
    var $item = $(e.item);
    selectedPage = getPageInfo($item);
    updatePropertyPanel(selectedPage);

    $("#add-child-button").removeAttr("disabled");
}

function handleAction(action, $sender) {
    if (!action) return;

    if (action == "design") {
        if (!settings.multilingual) {
            designPage(selectedPage);
        } else if (!$sender.attr("content")) {
            designPage(selectedPage, getCulture($sender));
        }
    } else if (action == "view") {
        window.open(getFinalPagePath(selectedPage, getCulture($sender)) + "?showUnpublished=true");
    } else if (action == "edit") {
        editPage(selectedPage);
    } else if (action == "delete") {
        deletePage(selectedPage);
    } else if (action == "addchild") {
        addChildPage(selectedPage);
    } else if (action == "seo") {
        if (!settings.multilingual) {
            openSeoDialog(selectedPage);
        } else {
            openSeoDialog(selectedPage, getCulture($sender));
        }
    }

    function getCulture($element) {
        var culture = $element.attr("href");
        if (culture != undefined && culture.length > 1) {
            return culture.substr(1);
        }
        return null;
    }
}

function updatePropertyPanel(page) {
    $messageContainer.hide();
    $propertyBody.show();

    $pageProperties.html($.tmpl("PageProperties", page));

    var $skinCell = $pageProperties.find("#skin-cell");
    if (page.Skin.length > 0) {
        $skinCell.html("<img src='" + page.SkinPreviewImage + "' title='" + page.Skin + "' alt='" + page.Skin + "' />");
    } else {
        $skinCell.html(Messages.None);
    }

    if (page.IsDeletable) {
        $("#delete-button").show();
    } else {
        $("#delete-button").hide();
    }
}

function updateCurrentPage() {
    if (selectedPage == null) return;

    var id = selectedPage.Id;
    var $item = $("#page-" + id).closest(".t-item");
    selectedPage = getPageInfo($item);
    updatePropertyPanel(selectedPage);
}

/* Page Management APIs */

function getFinalPagePath(pageInfo, culture) {
    var url = pageInfo.PagePath;
    if (settings.multilingual && culture != null) {
        url = "/" + culture + url;
    }
    if (settings.pageExtension.length > 0) {
        url += settings.pageExtension;
    }
    
    return getFullVirutalPath(url);
}

function designPage(pageInfo, pageCulture) {
    var url = pageInfo.DesignerPath + "?pageid=" + pageInfo.Id;
    if (pageCulture != undefined && pageCulture != null && pageCulture.length > 0) {
        url += "&page-culture=" + pageCulture;
    }
    window.open(url);
}

function editPage(pageInfo) {
    Sig.Window.open("Dialog", {
        title: Messages.DialogTitle.EditPage,
        url: getFullVirutalPath("/Admin/Pages/PageEdit.aspx?pageid=" + pageInfo.Id),
        width: 650,
        height: 450
    });
}

function openSeoDialog(pageInfo, culture) {
    var cultureQuery = "";

    if (culture) {
        cultureQuery = "&culture=" + culture;
    }

    Sig.Window.open("Dialog", {
        title: "SEO",
        url: getFullVirutalPath("/Admin/Pages/PageSEO.aspx?pageid=" + pageInfo.Id + cultureQuery),
        width: 500,
        height: 260
    });
}

function addChildPage(parentPageInfo) {
    var parentId = 0;

    if (parentPageInfo) {
        parentId = parentPageInfo.Id;
    }

    Sig.Window.open("Dialog", {
        title: Messages.DialogTitle.AddPage,
        url: getFullVirutalPath("/Admin/Pages/PageEdit.aspx?parentpageid=" + parentId),
        width: 650,
        height: 450
    });
}

function deletePage(pageInfo) {
    if (confirm(Messages.DeletePageConfirm)) {

        message(Messages.Processing + "...", false);

        var $treeNode = $("#page-" + pageInfo.Id).closest(".t-item");

        Seeger.Web.UI.Admin.Pages.Services.CascadeDelete(pageInfo.Id, function (result) {
            result = eval(result);
            if (!result.Success) {
                alert(result.Message);
            } else {
                pageTree.remove($treeNode);
                $("#add-child-button").attr("disabled", "disabled");

                if ($("#PageTree li").length == 0) {
                    showNoPageHint();
                }

                selectedPage = null;
                $("#content-page-selected").hide();
                $("#content-no-selection").show();

                message(Messages.DeleteSuccess);
            }
        });
    }
}

function getPageId($item) {
    return parseInt($item.find("> div .node-text").attr("id").substr(5))
}

function getPageInfo($treeNode) {
    return eval("(" + $treeNode.find(".pageinfo:first").val() + ")");
}

function hideNoPageHint() {
    $noPageHint.hide();
}

function showNoPageHint() {
    $noPageHint.show();
}

function getFullVirutalPath(virtualPathRelativeToCmsRoot) {
    if (settings.installPath.length == 0 || settings.installPath == '/') {
        return virtualPathRelativeToCmsRoot;
    }

    return settings.installPath + virtualPathRelativeToCmsRoot;
}

/* Message */

function message(msg, duration, type) {

    var cssClass = '';
    if (type == 'error') {
        cssClass = 'error';
    }

    if (duration == undefined) {
        duration = 2000;
    }

    Sig.Message.show(msg, { identity: 'PageList_Message', cssClass: cssClass, duration: duration });
}

function hideMessage() {
    Sig.Message.hide('PageList_Message');
}