String.prototype.format = function () {
	return String.format(this, arguments.length == 1 ? arguments[0] : arguments);
};

String.format = function (source, params) {
	var _toString = function (obj, format) {
		var ctor = function (o) {
			if (typeof o == 'number')
				return Number;
			else if (typeof o == 'boolean')
				return Boolean;
			else if (typeof o == 'string')
				return String;
			else
				return o.constructor;
		}(obj);
		var proto = ctor.prototype;
		var formatter = typeof obj != 'string' ? proto ? proto.format || proto.toString : obj.format || obj.toString : obj.toString;
		if (formatter)
			if (typeof format == 'undefined' || format == "")
				return formatter.call(obj);
			else
				return formatter.call(obj, format);
		else
			return "";
	};
	if (arguments.length == 1)
		return function () {
			return String.format.apply(null, [source].concat(Array.prototype.slice.call(arguments, 0)));
		};
	if (arguments.length == 2 && typeof params != 'object' && typeof params != 'array')
		params = [params];
	if (arguments.length > 2)
		params = Array.prototype.slice.call(arguments, 1);
	source = source.replace(/\{\{|\}\}|\{([^}: ]+?)(?::([^}]*?))?\}/g, function (match, num, format) {
		if (match == "{{") return "{";
		if (match == "}}") return "}";
		if (typeof params[num] != 'undefined' && params[num] !== null) {
			return _toString(params[num], format);
		} else {
			return "";
		}
	});
	return source;
};