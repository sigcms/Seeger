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

function Queue() {
    /// <summary>Represents a first-in, first-out collection of objects.</summary>
    _count = 0;
    _innerArray = new Array();

    this.get_count = function () {
        /// <summary>Gets the number of elements contained in the Queue.</summary>
        return _innerArray.length;
    }
    this.enqueue = function (value) {
        /// <summary>Adds an object to the end of the Queue.</summary>
        _validateValue(value, "value");

        _innerArray[_innerArray.length] = value;
    }
    this.contains = function (value, comparer) {
        /// <summary>Determines whether an element is in the Queue.</summary>
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
        /// <summary>Returns the object at the beginning of the Queue without removing it.</summary>
        if (this.get_count() == 0) {
            return null;
        }
        return _innerArray[0];
    }
    this.dequeue = function () {
        /// <summary>Removes and returns the object at the beginning of the Queue.</summary>
        if (this.get_count() == 0) {
            return null;
        }
        return _innerArray.shift();
    }
    this.clear = function () {
        /// <summary>Removes all objects from the Queue.</summary>
        _innerArray.length = 0;
    }
    this.toArray = function () {
        /// <summary>Copies the Queue elements to a new array.</summary>
        var count = this.get_count();
        var array = new Array(count);
        for (var i = 0; i < count; i++) {
            array[i] = _innerArray[i];
        }
        return array;
    }
    this.toString = function () {
        /// <summary>Returns a String that represents the Queue.</summary>
        if (this.get_count() == 0) {
            return "[Empty]";
        }
        var count = this.get_count();
        var result = _innerArray[0].toString();
        for (var i = 1; i < count; i++) {
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