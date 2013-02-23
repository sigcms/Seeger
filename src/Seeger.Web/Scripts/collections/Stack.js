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

/// <reference path='Base.js' />

function Stack() {
    /// <summary>Represents a first-in, last out collection of objects.</summary>
    var _count = 0;
    var _innerArray = new Array();

    this.get_count = function () {
        /// <summary>Gets the number of elements contained in the Stack.</summary>
        return _innerArray.length;
    }
    this.push = function (value) {
        /// <summary>Inserts an object at the top of the Stack.</summary>
        _validateValue(value, "value");

        _innerArray[_innerArray.length] = value;
    }
    this.contains = function (value, comparer) {
        /// <summary>Determines whether an element is in the Stack.</summary>
        /// <param name='comparer'>The comparer function to use. If not specified, a default comparer will be used. Signature: function(x, y) { }</param>
        _validateValue(value);

        if (!comparer) {
            comparer = Comparers.Default;
        } else {
            if (typeof (comparer) != "function") {
                throw new ArgumentTypeError("comparer", "Function");
            }
        }

        for (var i in _innerArray) {
            if (comparer(_innerArray[i], value) == 0) {
                return true;
            }
        }

        return false;
    }
    this.peek = function () {
        /// <summary>Returns the object at the top of the Stack without removing it.</summary>
        if (this.get_count() == 0) {
            return null;
        }
        return _innerArray[this.get_count() - 1];
    }
    this.pop = function () {
        /// <summary>Removes and returns the object at the top of the Stack.</summary>
        if (this.get_count() == 0) {
            return null;
        }
        var result = this.peek();
        _innerArray.length -= 1;
        return result;
    }
    this.clear = function () {
        /// <summary>Removes all objects from the Stack.</summary>
        _innerArray.length = 0;
    }
    this.toArray = function () {
        /// <summary>Copies the Stack to a new array.</summary>
        var count = this.get_count();
        var array = new Array(count);
        for (var i = count - 1; i >= 0; i--) {
            array[count - 1 - i] = _innerArray[i];
        }
        return array;
    }
    this.toString = function () {
        /// <summary>Returns a String  that represents the Stack.</summary>
        if (this.get_count() == 0) {
            return "[Empty]";
        }
        var count = this.get_count();
        var result = _innerArray[count - 1].toString();
        for (var i = count - 2; i >= 0; i--) {
            result += ", " + _innerArray[i].toString();
        }

        return "[" + result + "]";
    }

    function _validateValue(value, argName) {
        if (value === UNDEFINED_VALUE) {
            throw new ArgumentUndefinedError(argName);
        }
        if (value == null) {
            throw new ArgumentNullError(argName);
        }
    }
}