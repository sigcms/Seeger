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

        self.hasMore = ko.observable(true);

        self.comments = ko.observableArray();

        self.prepend = function (comment) {
            self.comments.unshift(mapCommentFromJS(comment));
            totalLoadedItems++;
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

        self.activeComment = null;

        self.toggleReply = function (comment, e) {
            if (comment.replying()) {
                self.cancelReply(comment, e);
            } else {
                if (self.activeComment) {
                    self.cancelReply(self.activeComment, e);
                }

                self.startReply(comment, e);
            }
        }

        self.startReply = function (comment, e) {
            self.activeComment = comment;

            if (!comment.commentBox) {
                var $container = $(e.target).closest('.cmt-comment-item').find('.cmt-comment-box-container');
                comment.commentBox = new CommentBox({
                    container: $container,
                    subjectId: opts.subjectId,
                    subjectTitle: opts.subjectTitle,
                    parentCommentId: comment.id(),
                    onSubmitted: function (data) {
                        self.completeReply(comment, mapCommentFromJS(data));
                    }
                });
                comment.commentBox.initValidation();
            }

            comment.replying(true);
        }

        self.cancelReply = function (comment) {
            comment.replying(false);
            self.activeComment = null;
        }

        self.completeReply = function (comment, reply) {
            comment.replies.push(reply);
            comment.replying(false);
            self.activeComment = null;
        }

        function onDataLoaded(data) {
            var hasReplyCommentIds = [];

            $.each(data.items, function () {
                self.comments.push(mapCommentFromJS(this));
                if (this.totalReplies > 0) {
                    hasReplyCommentIds.push(this.id);
                }
            });

            totalLoadedItems += data.items.length;
            self.hasMore(totalLoadedItems < data.totalItems);

            if (data.length > 0) {
                start = data.items[data.items.length - 1].id - 1;
            }

            if (hasReplyCommentIds.length > 0) {
                $.ajax({
                    url: '/api/cmt/comments/replies?commentIds=' + hasReplyCommentIds.join(','),
                    type: 'GET',
                    contentType: 'application/json'
                })
                .done(function (data) {
                    $.each(data, function (i) {
                        var replies = this;
                        var commentId = hasReplyCommentIds[i];
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
                    });
                });
            }
        }

        function mapCommentFromJS(comment) {
            var vm = ko.mapping.fromJS(comment);
            vm.replies = ko.observableArray();
            vm.replying = ko.observable(false);
            return vm;
        }

        function nextBatch() {
            return $.ajax({
                url: '/api/cmt/comments?subjectId=' + opts.subjectId + '&start=' + start + '&limit=' + (opts.limit || 20),
                type: 'GET',
                contentType: 'application/json'
            });
        }
    }

    function CommentBox(options) {
        var self = this;
        var opts = $.extend(true, {}, options);
        var $container = $(opts.container);

        self.initValidation = function () {
            sig.cmt.reparseUnobtrusiveValidation($container.find('form'));
        }

        self.applyBindings = function () {
            ko.applyBindings(self, $container[0]);
        }

        self.onSubmitted = opts.onSubmitted;

        self.content = ko.observable();

        self.submitting = ko.observable(false);

        self.validate = function () {
            return $container.find('form').valid();
        }

        self.submit = function () {
            if (!self.validate()) {
                return false;
            }

            self.submitting(true);

            var model = {
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