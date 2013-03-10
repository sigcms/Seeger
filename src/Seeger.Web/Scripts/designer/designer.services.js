(function ($) {
    // Designer Service
    var DesignerService = {
        requestWidgetDesigner: function (widgetName, pluginName, templateName, completeCallback) {
            var context = Sig.Designer.get_current().get_context();
            var culture = context.get_culture();
            var pageId = context.get_pageId();

            $.ajax({
                url: Sig.UrlUtility.combine('/Admin/Designer/WidgetPreview.ashx?culture=' + culture + '&pageid=' + pageId),
                type: 'POST',
                cache: false,
                data: {
                    widgetName: widgetName,
                    pluginName: pluginName = null ? "" : pluginName,
                    templateName: templateName == null ? "" : templateName
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    completeCallback(data);
                },
                error: function (xhr, status, errorToThrow) {
                    document.write(xhr.responseText);
                }
            });
        },
        saveLayout: function (pageId, culture, stateManager, completeCallback) {
            var states = stateManager.serialize();
            Sig.Logger.debug(states);

            var context = Sig.Designer.get_current().get_context();

            $.ajax({
                url: '/Admin/Designer/SaveChanges.ashx',
                type: 'POST',
                cache: false,
                data: {
                    pageId: pageId,
                    culture: culture,
                    states: states
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    completeCallback(data);
                },
                error: function (xhr, status, errorToThrow) {
                    document.write(xhr.responseText);
                }
            });
        }
    };

    window.Sig.DesignerService = DesignerService;

})(jQuery);