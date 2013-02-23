(function ($) {
    function CommandManager() {
        var _commands = {};

        this.register = function (name, fn) {
            _commands[name] = new Command(name, fn);
        }
        this.execute = function (name, data) {
            var cmd = _commands[name];
            if (cmd) {
                cmd.execute(data);
            } else {
                throw new Error("Command '" + name + "' was not found.");
            }
        }
    }

    function Command(name, fn) {
        var _name = name;
        var _fn = fn;

        this.get_name = function () { return _name; }
        this.execute = function (data) { _fn(data); }
    }

    // Public APIs
    window.Sig.CommandManager = CommandManager;
    window.Sig.Command = Command;

})(jQuery);