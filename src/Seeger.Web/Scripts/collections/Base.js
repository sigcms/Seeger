/*
* Javascript Collections Library 0.1 beta 1
* http://sym.7hands.net/projects/jscollections
*
* Copyright (c) 2010 Dylan Lin (S.Y.M)
* Licensed under the Ms-PL license.
* http://www.opensource.org/licenses/ms-pl.html
*
* Date: 2010-07-16
*/

var UNDEFINED_VALUE;

// Errors

function ArgumentUndefinedError(argName) {
    var _argumentName = argName;
    this.message = "'" + argName + "' cannot be undefined.";
    this.get_argumentName = function () { return _argumentName; }
}
ArgumentUndefinedError.prototype = Error;

function ArgumentNullError(argName) {
    var _argumentName = argName;
    this.message = "'" + argName + "' cannot be null.";
    this.get_argumentName = function () { return _argumentName; }
}
ArgumentNullError.prototype = Error;

function ArgumentTypeError(argName, expectedTypeName) {
    var _argumentName = argName;
    var _expectedTypeName = expectedTypeName;
    this.message = "'" + argName + "' should be of type '" + expectedTypeName + "'.";
    this.get_argumentName = function() { return _argumentName; }
    this.get_expectedTypeName = function () { return _expectedTypeName; }
}
ArgumentTypeError.prototype = Error;

function InvalidOperationError(msg) {
    if (msg) {
        this.message = msg;
    }
}
InvalidOperationError = Error;

// Comparers

var Comparers = {
    Default: function (x, y) {
        if (x == y) {
            return 0;
        }
        return x > y ? 1 : -1;
    }
};