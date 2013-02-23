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

function LinkedListNode(value, prev, next) {
    // Private Fields
    var _value = null;
    var _prev = null;
    var _next = null;
    var _this = this;

    // Internal Fields
    this._list = null;

    // Public APIs
    this.get_value = function () { return _value; }
    this.set_value = function (data) { _value = data; }
    this.get_prev = function () { return _prev; }
    this.set_prev = function (node) {
        if (node === UNDEFINED_VALUE) throw new ArgumentUndefinedError("node");
        if (node != null && !(node instanceof LinkedListNode)) throw new ArgumentTypeError("node", "LinkedListNode");

        _prev = node;
    }
    this.get_next = function () { return _next; }
    this.set_next = function (node) {
        if (node === UNDEFINED_VALUE) throw new ArgumentUndefinedError("node");
        if (node != null && !(node instanceof LinkedListNode)) throw new ArgumentTypeError("node", "LinkedListNode");

        _next = node;
    }
    this.invalidate = function () {
        _prev = null;
        _next = null;
        _this._list = null;
    }
    this.remove = function () {
        _this._list.removeNode(_this);
    }
    this.toString = function () {
        return _value == null ? "null" : _value.toString();
    }

    // Helper methods
    function _init(value, prev, next) {
        if (value) { _value = value; }
        if (prev) { _prev = prev; }
        if (next) { _next = next; }
    }

    // Initailization
    _init(value, prev, next);
}

function LinkedList(array, comparer) {
    /// <summary>Represents a doubly linked list.</summary>
    /// <param name='comparer'>The comparer function to use. If not specified, a default comparer will be used. Signature: function(x, y) { }</param>

    // Private Fields
    var _head = null;
    var _tail = null;
    var _count = 0;
    var _comparer = null;
    var _this = this;

    if (arguments.length > 0 && !(array instanceof Array)) {
        throw new ArgumentTypeError("array", "Array");
    }

    // Public APIs
    this.get_head = function () {
        /// <summary>Gets the first node of the LinkedList.</summary>
        return _head;
    }
    this.get_tail = function () {
        /// <summary>Gets the last node of the LinkedList.</summary>
        return _tail;
    }
    this.get_count = function () {
        /// <summary>Gets the number of nodes actually contained in the LinkedList.</summary>
        return _count;
    }
    this.addLast = function (value) {
        /// <summary>Adds a new node containing the specified value at the end of the LinkedList.</summary>
        _validateNodeValue(value, "value");

        var node = new LinkedListNode(value);
        if (_head == null) {
            _head = _tail = node;
        } else {
            _tail.set_next(node);
            node.set_prev(_tail);
            _tail = node;
        }
        node._list = _this;
        _count++;

        return node;
    }
    this.addFirst = function (value) {
        /// <summary>Adds a new node containing the specified value at the start of the LinkedList.</summary>
        _validateNodeValue(value, "value");

        var node = new LinkedListNode(value);
        if (_head == null) {
            _head = _tail = node;
        } else {
            _head.set_prev(node);
            node.set_next(_head);
            _head = node;
        }
        node._list = _this;
        _count++;

        return node;
    }
    this.addBefore = function (node, value) {
        /// <summary>Adds a new node containing the specified value before the specified existing node in the LinkedList.</summary>
        _ensureNodeIsInList(node);
        _validateNodeValue(value, "value");

        var newNode = new LinkedListNode(value);
        newNode._list = _this;

        var oldPrev = node.get_prev();
        newNode.set_next(node);
        newNode.set_prev(oldPrev);
        node.set_prev(newNode);
        if (oldPrev != null) {
            oldPrev.set_next(newNode);
        }

        if (node == _head) {
            _head = newNode;
        }

        return newNode;
    }
    this.addAfter = function (node, value) {
        /// <summary>Adds a new node containing the specified value after the specified existing node in the LinkedList.</summary>
        _ensureNodeIsInList(node);
        _validateNodeValue(value, "value");

        var next = node.get_next();
        if (next == null) {
            return _this.addLast(value);
        } else {
            return _this.addBefore(next, value);
        }
    }
    this.moveBefore = function (refNode, nodeToMove) {
        /// <summary>Moves an existing node before another existing node.</summary>
        /// <param name='refNode'>The destination node.</param>
        /// <param name='nodeToMove'>The node to be moved.</param>

        _ensureNodeIsInList(refNode);
        _ensureNodeIsInList(nodeToMove);

        if (refNode == nodeToMove) return;
        if (nodeToMove.get_next() == refNode) return;

        _extractNode(nodeToMove);

        var newPrev = refNode.get_prev();
        if (newPrev != null) {
            newPrev.set_next(nodeToMove);
        } else {
            _head = nodeToMove;
        }

        nodeToMove.set_prev(newPrev);
        nodeToMove.set_next(refNode);
    }
    this.moveBeforeHead = function (nodeToMove) {
        /// <summary>Moves an existing node before the head of the LinkedList, and make it the new head.</summary>
        /// <param name='nodeToMove'>The node to be moved.</param>

        _ensureNodeIsInList(nodeToMove);

        if (nodeToMove == _head) return;

        _extractNode(nodeToMove);

        _head.set_prev(nodeToMove);
        nodeToMove.set_next(_head);
        nodeToMove.set_prev(null);

        _head = nodeToMove;
    }
    this.moveAfter = function (refNode, nodeToMove) {
        /// <summary>Moves an existing node after another existing node.</summary>
        /// <param name='refNode'>The destination node.</param>
        /// <param name='nodeToMove'>The node to be moved.</param>

        _ensureNodeIsInList(refNode);
        _ensureNodeIsInList(nodeToMove);

        if (refNode == nodeToMove) return;
        if (nodeToMove.get_prev() == refNode) return;

        var newNext = refNode.get_next();
        if (newNext != null) {
            _this.moveBefore(newNext, nodeToMove);
        } else {
            _this.moveAfterTail(nodeToMove);
        }
    }
    this.moveAfterTail = function (nodeToMove) {
        /// <summary>Moves an existing node after the tail of the LinkedList, and make it the new tail.</summary>
        /// <param name='nodeToMove'>The node to be moved.</param>
        _ensureNodeIsInList(nodeToMove);

        if (nodeToMove == _tail) return;

        _extractNode(nodeToMove);

        _tail.set_next(nodeToMove);
        nodeToMove.set_prev(_tail);
        nodeToMove.set_next(null);

        _tail = nodeToMove;
    }
    this.moveByOffset = function (offset, nodeToMove) {
        /// <summary>Moves an existing node to a new location by offset.</summary>
        /// <param name='offset'>If offset < 0, move left; if offset > 0, move right; if offset == 0, do nothing.</param>
        /// <param name='nodeToMove'>The node to be moved.</param>

        _ensureNodeIsInList(nodeToMove);

        if (offset == 0 || offset == 1) return;

        if (offset <= -(_this.get_count() - 1)) {
            _this.moveBeforeHead(nodeToMove);
        } else if (offset >= _this.get_count() - 1) {
            _this.moveAfterTail(nodeToMove);
        } else {
            var destNode = nodeToMove;
            if (offset < 0) {
                do {
                    destNode = destNode.get_prev();
                    offset++;
                } while (destNode != null && offset < 0);

                if (destNode == null) {
                    _this.moveBeforeHead(nodeToMove);
                } else {
                    _this.moveBefore(destNode, nodeToMove);
                }
            } else {
                do {
                    destNode = destNode.get_next();
                    offset--;
                } while (destNode != null && offset > 0);

                if (destNode == null) {
                    _this.moveAfterTail(nodeToMove);
                } else {
                    _this.moveBefore(destNode, nodeToMove);
                }
            }
        }
    }
    this.contains = function (value, comparer) {
        /// <summary>Determines whether a value is in the LinkedList.</summary>
        /// <param name='comparer'>The comparer function to use. If not specified, a default comparer will be used. Signature: function(x, y) { }</param>
        return this.findNode(value, comparer) != null;
    }
    this.findNode = function (value, comparer) {
        /// <summary>Finds the first node that contains the specified value.</summary>
        /// <param name='comparer'>The comparer function to use. If not specified, a default comparer will be used. Signature: function(x, y) { }</param>
        _validateNodeValue(value, "value");

        if (!comparer) {
            comparer = _comparer;
        } else {
            if (typeof (comparer) != "function") {
                throw new ArgumentTypeError("comparer", "Function");
            }
        }

        var current = _head;
        while (current != null) {
            if (comparer(current.get_value(), value) == 0) {
                return current;
            }
            current = current.get_next();
        }
        return null;
    }
    this.findLastNode = function (value, comparer) {
        /// <summary>Finds the last node that contains the specified value.</summary>
        /// <param name='comparer'>The comparer function to use. If not specified, a default comparer will be used. Signature: function(x, y) { }</param>
        _validateNodeValue(value);

        if (!comparer) {
            comparer = _comparer;
        } else {
            if (typeof(comparer) != "function") {
                throw new ArgumentTypeError("comparer", "Function");
            }
        }

        var current = _tail;
        while (current != null) {
            if (comparer(current.get_value(), value) == 0) {
                return current;
            }
            current = current.get_prev();
        }
        return null;
    }
    this.findNodeByPredicate = function (predicate) {
        /// <summary>Finds the first node that matches the predicate.</summary>
        /// <param name='predicate'>Optional. Signature: function (node, nodeValue) { }</param>
        if (!predicate) {
            return _head;
        }
        if (typeof (predicate) != "function") {
            throw new ArgumentTypeError("predicate", "Function");
        }

        var current = _head;
        while (current != null) {
            if (predicate(current, current.get_value())) {
                return current;
            }
            current = current.get_next();
        }
        return null;
    }
    this.findLastNodeByPredicate = function (predicate) {
        /// <summary>Finds the last node that matches the predicate.</summary>
        /// <param name='predicate'>Optional. Signature: function (node, nodeValue) { }</param>
        if (!predicate) {
            return _tail;
        }

        if (typeof (predicate) != "function") {
            throw new ArgumentTypeError("predicate", "Function");
        }

        var current = _tail;

        while (current != null) {
            if (predicate(current, current.get_value())) {
                return current;
            }
            current = current.get_prev();
        }

        return null;
    }
    this.findValueByPredicate = function (predicate) {
        /// <summary>Finds the value of the first node that matches the predicate.</summary>
        /// <param name='predicate'>Optional. Signature: function (node, nodeValue) { }</param>
        var node = this.findNodeByPredicate(predicate);
        if (node != null) {
            return node.get_value();
        }
        return null;
    }
    this.findLastValueByPredicate = function (predicate) {
        /// <summary>Finds the value of the last node that matches the predicate.</summary>
        /// <param name='predicate'>Optional. Signature: function (node, nodeValue) { }</param>
        var node = this.findLastNodeByPredicate(predicate);
        if (node != null) {
            return node.get_value();
        }
        return null;
    }
    this.remove = function (value, comparer) {
        /// <summary>Removes the first occurrence of the specified value from the LinkedList.</summary>
        /// <param name='value'>The value to remove.</param>
        /// <param name='comparer'>The comparer function to use. If not specified, a default comparer will be used. Signature: function(x, y) { }</param>
        _validateNodeValue(value, "value");

        var node = this.findNode(value, comparer);
        if (node != null) {
            _this.removeNode(node);
            return true;
        }
        return false;
    }
    this.removeAll = function (value, comparer) {
        /// <summary>Removes all occurrences of the specified value from the LinkedList.</summary>
        /// <param name='value'>The value to remove.</param>
        /// <param name='comparer'>The comparer function to use. If not specified, a default comparer will be used. Signature: function(x, y) { }</param>
        _validateNodeValue(value, "value");

        if (!comparer) {
            comparer = _comparer;
        } else {
            if (typeof (comparer) != "function") {
                throw new ArgumentTypeError("comparer", "Function");
            }
        }

        var current = _head;
        while (current != null) {
            var next = current.get_next();
            if (comparer(value, current.get_value()) == 0) {
                _this.removeNode(current);
            }
            current = next;
        }
    }
    this.removeByPredicate = function (predicate) {
        /// <summary>Removes the first occurrence which matches the predicate from the LinkedList.</summary>
        /// <param name='predicate'>function (node, nodeValue) { }</param>
        if (typeof (predicate) != "function") {
            throw new ArgumentTypeError("predicate", "Function");
        }
        var current = _head;
        while (current != null) {
            if (predicate(current, current.get_value())) {
                _this.removeNode(current);
                return true;
            }
            current = current.get_next();
        }
        return false;
    }
    this.removeAllByPredicate = function (predicate) {
        /// <summary>Removes all occurrences which matches the predicate from the LinkedList.</summary>
        /// <param name='predicate'>function (node, nodeValue) { }</param>
        if (typeof (predicate) != "function") {
            throw new ArgumentTypeError("predicate", "Function");
        }
        var current = _head;
        while (current != null) {
            var next = current.get_next();
            if (predicate(current, current.get_value())) {
                _this.removeNode(current);
            }
            current = next;
        }
    }
    this.removeFirst = function () {
        /// <summary>Removes the node at the start of the LinkedList.</summary>
        if (_count > 0) {
            _this.removeNode(_head);
            return true;
        }
        return false;
    }
    this.removeLast = function () {
        /// <summary>Removes the node at the end of the LinkedList.</summary>
        if (_count > 0) {
            _this.removeNode(_tail);
            return true;
        }
        return false;
    }
    this.removeNode = function (node) {
        /// <summary>Remove a existing node from the LinkedList.</summary>

        _ensureNodeIsInList(node);

        var prev = node.get_prev();
        var next = node.get_next();
        if (node == _head) {
            _head = next;
        }
        if (node == _tail) {
            _tail = prev;
        }
        if (prev != null) {
            prev.set_next(next);
        }
        if (next != null) {
            next.set_prev(prev);
        }
        node.invalidate();
        _count--;
    }
    this.clear = function () {
        /// <summary>Removes all nodes from the LinkedList.</summary>
        var current = _head;
        while (current != null) {
            var node = current.get_next();
            current.invalidate();
            current = node;
        }

        _head = null;
        _tail = null;
        _count = 0;
    }
    this.reverse = function () {
        /// <summary>Reverses the order of the elements in the entire LinkedList.</summary>
        if (_count == 0) return;

        var current = _tail;

        // switch tail and head
        _tail = _head;
        _head = current;

        while (current != null) {
            var newPrev = current.get_next();
            var oldPrev = current.get_prev();

            current.set_prev(newPrev);
            current.set_next(oldPrev);

            current = oldPrev;
        }
    }
    this.swap = function (node1, node2) {
        /// <summary>Swap two existing nodes in the LinkedList.</summary>
        _ensureNodeIsInList(node1);
        _ensureNodeIsInList(node2);

        if (node1 == node2) return;

        var node1NewPrev = null;
        var node1NewNext = null;
        var node2NewPrev = null;
        var node2NewNext = null;

        if (node1.get_next() == node2) {            // node1 <--> node2
            node1NewPrev = node2;
            node1NewNext = node2.get_next();
            node2NewPrev = node1.get_prev();
            node2NewNext = node1;
        } else if (node1.get_prev() == node2) {     // node2 <--> node1
            node1NewPrev = node2.get_prev();
            node1NewNext = node2;
            node2NewPrev = node1;
            node2NewNext = node1.get_next();
        } else {                                    // node1 <--> other node <--> node2, TODO: MUST change 'other node'.next ot node1
            node1NewPrev = node2.get_prev();
            node1NewNext = node2.get_next();
            node2NewPrev = node1.get_prev();
            node2NewNext = node1.get_next();
        }

        node1.set_prev(node1NewPrev);
        if (node1NewPrev != null) {
            node1NewPrev.set_next(node1);
        }
        node1.set_next(node1NewNext);
        if (node1NewNext != null) {
            node1NewNext.set_prev(node1);
        }
        node2.set_prev(node2NewPrev);
        if (node2NewPrev != null) {
            node2NewPrev.set_next(node2);
        }
        node2.set_next(node2NewNext);
        if (node2NewNext != null) {
            node2NewNext.set_prev(node2);
        }

        if (node1.get_prev() == null) {
            _head = node1;
        } else if (node1.get_next() == null) {
            _tail = node1;
        }
        if (node2.get_prev() == null) {
            _head = node2;
        } else if (node2.get_next() == null) {
            _tail = node2;
        }
    }
    this.traverse = function (visitor) {
        /// <summary>Traverse the whole list from head to tail using the visitor.</summary>
        /// <param name='visitor'>Return true for stopping the traversal. Siganture: function (node, nodeValue) { }</param>
        if (typeof (visitor) != "function") {
            throw new ArgumentTypeError("visitor", "Function");
        }
        var current = _head;
        while (current != null) {
            if (visitor(current, current.get_value())) {
                break;
            }
            current = current.get_next();
        }
    }
    this.toArray = function () {
        var array = new Array(_this.get_count());
        var i = 0;
        _this.traverse(function (n, v) {
            array[i] = v;
            ++i;
            return false;
        });
        return array;
    }
    this.toString = function () {
        /// <summary>Returns a String  that represents the current LinkedList.</summary>
        if (this.get_count() == 0) {
            return "[Empty]";
        }
        var current = _head;
        var result = current.get_value().toString();
        while (current.get_next() != null) {
            current = current.get_next();
            result += ", " + current.get_value().toString();
        }

        return "[" + result + "]";
    }

    // Helper methods
    function _init(array, comparer) {
        _comparer = comparer ? comparer : Comparers.Default;

        for (var i in array) {
            _this.addLast(array[i]);
        }
    }

    function _extractNode(node) {
        var prev = node.get_prev();
        var next = node.get_next();

        if (prev != null) {
            prev.set_next(next);
        } else {
            _head = next;
        }
        if (next != null) {
            next.set_prev(prev);
        } else {
            _tail = prev;
        }
    }
    function _ensureNodeIsInList(node) {
        if (node === UNDEFINED_VALUE) {
            throw new ArgumentUndefinedError("node");
        }
        if (node == null) {
            throw new ArgumentNullError("node");
        }
        if (node._list != _this) {
            throw new InvalidOperationError("'node' should be an existing node in the list.");
        }
    }
    function _validateNodeValue(value, argName) {
        if (value === UNDEFINED_VALUE) {
            throw new ArgumentUndefinedError(argName);
        }
        if (value == null) {
            throw new ArgumentNullError(argName);
        }
    }

    // Initialization
    _init(array, comparer);
}