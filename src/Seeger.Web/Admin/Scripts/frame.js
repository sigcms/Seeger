(function ($) {

    $(function () {
        setupLayout();
        window.onresize = setupLayout;
    });

    function setupLayout() {
        var $frameMain = null;
        var $menubar = null;
        var $frameMainContent = null;
        var $contentIFrame = null;
        var headerHeight = 0;
        var documentHeight = 0;

        initialize();
        resizeLayout();
        window.onresize = resizeLayout;

        function initialize() {
            $frameMain = $("#frame-main");
            $menubar = $("#frame-menubar");
            $contentIFrame = $("#content-iframe");
            $frameMainContent = $("#frame-main-content");
            headerHeight = $("#frame-header").outerHeight();
            documentHeight = $(document).height();
        }

        function resizeLayout() {
            var mainPanelHeight = documentHeight - headerHeight;

            if ($.browser.msie) {
                mainPanelHeight -= 7;
            }

            $frameMain.height(mainPanelHeight);
            $menubar.height(mainPanelHeight);

            var iframeWidth = $frameMainContent.width();
            $contentIFrame.width(iframeWidth);
            $contentIFrame.height(mainPanelHeight);
        }
    }

})(jQuery);
