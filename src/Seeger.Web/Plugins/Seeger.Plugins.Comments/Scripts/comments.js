(function ($) {

    var sig = window.sig = window.sig || {};
    sig.cmt = sig.cmt || {};

    function CommentList(options) {
        var self = this;
        var opts = $.extend(true, {}, options);
        var $container = $(opts.container);
        var start = -1;
        var totalLoadedItems = 0;

        self.applyBindings = function () {
            ko.applyBindings(self, $container[0]);
        }

        self.loading = ko.observable(true);

        self.loadingMore = ko.observable(false);

        self.hasMore = ko.observable(false);

        self.totalComments = ko.observable();

        self.totalUnloadedComments = ko.observable();

        self.comments = ko.observableArray();

        self.prepend = function (comment) {
            self.comments.unshift(mapCommentFromJS(comment));
            totalLoadedItems++;
            self.totalComments(self.totalComments() + 1);
        }

        self.load = function () {
            self.loading(true);
            nextBatch().done(function (data) {
                onDataLoaded(data);
                self.loading(false);
            });
        }

        self.more = function () {
            self.loadingMore(true);
            nextBatch().done(function (data) {
                onDataLoaded(data);
                self.loadingMore(false);
            });
        }

        // Comments will have at most 2 levels: root comment and replies.
        // Reply to an other reply will be represented as an inline reply to the replied reply
        self.startInlineReply = function (rootComment, reply, e) {
            self.startReply(rootComment, '@' + reply.commenterNick() + ' ', e);
        }

        self.startReply = function (comment, content, e) {
            var needInit = false;

            if (!comment.commentBox) {
                var $container = $(e.target).closest('.cmt-root-comment-item').find('.cmt-comment-box-container');
                comment.commentBox = new CommentBox({
                    container: $container,
                    subjectType: opts.subjectType,
                    subjectId: opts.subjectId,
                    subjectTitle: opts.subjectTitle,
                    parentCommentId: comment.id(),
                    onSubmitted: function (data) {
                        self.completeReply(comment, mapCommentFromJS(data));
                    },
                    onCanceled: function () {
                        self.cancelReply(comment);
                    }
                });

                needInit = true;
            }

            comment.replying(true);

            // Have to init after setting replying to true, because the dom is created after replying set to true
            if (needInit) {
                comment.commentBox.init();
            }

            comment.commentBox.content('');
            comment.commentBox.focus();

            if (content) {
                comment.commentBox.insert(content);
            }
        }

        self.cancelReply = function (comment) {
            comment.replying(false);
        }

        self.completeReply = function (comment, reply) {
            comment.replies.push(reply);
            comment.replying(false);
        }

        self.moreReplies = function (comment) {
            var start = comment.nextBatchReplyStartId || 0;
            loadReplies([comment.id()], start, 5);
        }

        function onDataLoaded(data) {
            self.totalComments(data.totalItems);

            var hasReplyCommentIds = [];

            $.each(data.items, function () {
                self.comments.push(mapCommentFromJS(this));
                if (this.totalReplies > 0) {
                    hasReplyCommentIds.push(this.id);
                }
            });

            totalLoadedItems += data.items.length;
            self.hasMore(totalLoadedItems < data.totalItems);
            self.totalUnloadedComments(data.totalItems - totalLoadedItems);

            if (data.length > 0) {
                start = data.items[data.items.length - 1].id - 1;
            }

            if (hasReplyCommentIds.length > 0) {
                loadReplies(hasReplyCommentIds, 0, 3);
            }
        }

        function loadReplies(commentIds, start, limit) {
            var url = '/api/cmt/comments/replies?commentIds=' + commentIds.join(',');
            if (start) {
                url += '&start=' + start;
            }
            if (limit) {
                url += '&limit=' + limit;
            }

            return $.ajax({
                url: url,
                type: 'GET',
                contentType: 'application/json'
            })
            .done(function (data) {
                $.each(data, function (i) {
                    var replies = this;
                    var commentId = commentIds[i];
                    var comment = null;
                    var comments = self.comments();

                    for (var i = 0, len = comments.length; i < len; i++) {
                        if (comments[i].id() === commentId) {
                            comment = comments[i];
                            break;
                        }
                    }

                    $.each(replies.items, function () {
                        comment.replies.push(mapCommentFromJS(this));
                    });

                    comment.nextBatchReplyStartId = replies.items[replies.items.length - 1].id + 1;
                    comment.totalUnloadedReplies(comment.totalUnloadedReplies() - replies.items.length);
                });
            });
        }

        function mapCommentFromJS(comment) {
            var vm = ko.mapping.fromJS(comment);
            vm.replies = ko.observableArray();
            vm.replying = ko.observable(false);
            vm.totalUnloadedReplies = ko.observable(comment.totalReplies);
            return vm;
        }

        function nextBatch() {
            return $.ajax({
                url: '/api/cmt/comments?subjectId=' + opts.subjectId + '&subjectType=' + opts.subjectType + '&start=' + start + '&limit=' + (opts.limit || 10),
                type: 'GET',
                contentType: 'application/json'
            });
        }
    }

    function CommentBox(options) {
        var self = this;
        var opts = $.extend(true, {}, options);
        var $container = $(opts.container);

        if ($container.length === 0)
            throw 'CommentBox requires a container dom element.';

        self.init = function () {
            self.initValidation();

            $container.on('click', '.cmt-emotions a', function (e) {
                self.insert($(this).html());
                e.preventDefault();
            });
        }

        self.initValidation = function () {
            sig.cmt.reparseUnobtrusiveValidation($container.find('form'));
        }

        self.applyBindings = function () {
            ko.applyBindings(self, $container[0]);
        }

        self.focus = function () {
            $container.find('textarea').focus();
        }

        self.onSubmitted = opts.onSubmitted;

        self.onCanceled = opts.onCanceled;

        self.content = ko.observable();

        self.submitting = ko.observable(false);

        self.validate = function () {
            return $container.find('form').valid();
        }

        self.insert = function (text) {
            if (text === null || text === undefined || text === '') {
                return;
            }

            var textarea = $container.find('textarea')[0];
            //IE support
            if (document.selection) {
                textarea.focus();
                sel = document.selection.createRange();
                sel.text = text;
            }
                //MOZILLA and others
            else if (textarea.selectionStart || textarea.selectionStart == '0') {
                var startPos = textarea.selectionStart;
                var endPos = textarea.selectionEnd;
                textarea.value = textarea.value.substring(0, startPos)
                    + text
                    + textarea.value.substring(endPos, textarea.value.length);
                textarea.selectionStart = startPos + text.length;
                textarea.selectionEnd = startPos + text.length;
            } else {
                textarea.value += text;
            }

            $(textarea).trigger('change');
        }

        self.cancel = function () {
            if (self.onCanceled) {
                self.onCanceled.apply(self, []);
            }
        }

        self.submit = function () {
            if (!self.validate()) {
                return false;
            }

            self.submitting(true);

            var model = {
                subjectType: opts.subjectType,
                subjectId: opts.subjectId,
                subjectTitle: opts.subjectTitle,
                content: self.content(),
                parentCommentId: opts.parentCommentId || null
            };

            $.ajax({
                url: '/api/cmt/comments',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(model)
            })
            .done(function (data) {
                self.submitting(false);
                self.content('');

                if (self.onSubmitted) {
                    self.onSubmitted.apply(self, [data]);
                }
            });
        }

        self.destroy = function () {
            $container.remove();
        }
    }

    sig.cmt.reparseUnobtrusiveValidation = function (selector) {
        var $container = $(selector);
        if ($container.is('form')) {
            removeFormValidation($container);
        }

        $container.find('form').each(function () {
            removeFormValidation(this);
        });

        $.validator.unobtrusive.parse($container);

        function removeFormValidation(form) {
            $(form).removeData('validator');
            $(form).removeData('unobtrusiveValidation');
        }
    }

    sig.cmt.CommentList = CommentList;
    sig.cmt.CommentBox = CommentBox;

})(jQuery);