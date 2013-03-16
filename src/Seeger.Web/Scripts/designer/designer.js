(function ($) {
    function Designer() {
        /// <summary>Represents the page designer object.</summary>
        var _this = this;
        var _zones = new LinkedList();
        var _toolbox = null;
        var _context = null;
        var _commandManager = new Sig.CommandManager();
        var _stateManager = new Sig.WidgetStateManager();

        this.init = function ($container) {
            //Sig.Logger.debug("Initializing Sig.Designer...");

            // Init toolbox
            _toolbox = new Sig.Toolbox(_this);
            _toolbox.init($container.find(Sig.Selectors.Toolbox + ":first"));

            // Init context
            _context = new Sig.Context();
            _context.init();

            // Register Default Commands
            _commandManager.register(Sig.CommandNames.Save, _this.saveChanges);
            _commandManager.register(Sig.CommandNames.Reload, _this.reload);
            _commandManager.register(Sig.CommandNames.View, _this.viewCurrentPage);
            Sig.WidgetInitializer.registerCommands(_commandManager, _this);

            // Init zones
            $container.find(".sig-zone").each(function () {
                var zone = new Sig.Zone($(this));
                _zones.addLast(zone);
                zone.init();
            });
        }

        // Zones
        this.get_zones = function () {
            return _zones;
        }
        this.findZoneByName = function (name) {
            return _zones.findValueByPredicate(function (node, value) {
                return value.get_name() == name;
            });
        }
        this.get_toolbox = function () { return _toolbox; }
        this.get_context = function () { return _context; }

        // Widget State Manager
        this.get_stateManager = function () { return _stateManager; }

        this.get_hasChanged = function () { return _stateManager.get_isDirty(); }

        // Commands
        this.saveChanges = function (options) {

            if (!_this.get_hasChanged()) return;

            // alert(_stateManager.serialize());
            // return;

            Sig.DesignerService.saveLayout(_context.get_pageId(), _context.get_culture(), _stateManager, function (result) {
                result = eval("(" + result + ")");
                if (!result.Success) {
                    if (options.error) {
                        options.error(result);
                    } else {
                        throw new Error(result.Message);
                    }
                } else if (options.success) {
                    options.success(result);
                }
            });
        }
        this.reload = function () {
            location.href = location.href;
        }
        this.viewCurrentPage = function () {
            window.open(_context.get_pageLiveUrl() + "?showUnpublished=true");
        }
        this.changeCulture = function (culture) {
            var url = location.href.replace('culture=' + _context.get_culture(), 'page-culture=' + culture);
            location.href = url;
        }
        // Command Helpers
        this.get_commandManager = function () {
            return _commandManager;
        }
        this.executeCommand = function (cmdName, cmdData) {
            _commandManager.execute(cmdName, cmdData);
        }
    }

    window._designer;

    Designer.get_current = function () {
        if (window._designer == null)
            throw new Error("Designer is not available until the page is completely loaded.");

        return window._designer;
    }

    // Public APIs

    window.Sig.Designer = Designer;

    // Initialization
    $(function () {
        //Sig.Logger.debug("Launching...");

        window._designer = new Designer();
        window._designer.init($(document.body));
    });

})(jQuery);