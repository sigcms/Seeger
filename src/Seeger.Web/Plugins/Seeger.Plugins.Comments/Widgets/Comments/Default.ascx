<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Default.ascx.cs" Inherits="Seeger.Plugins.Comments.Widgets.Comments.Default" %>

<div class="cmt-comment-panel">
    <div id="cmt-comment-list" class="cmt-comment-list">
        <%= RenderCommentListHeader() %>
        <div data-bind="visible: loading" class="cmt-loading">
            <i class="fa fa-spin fa-refresh"></i>
        </div>
        <div data-bind="visible: !loading()">
            <div class="cmt-comment-list-body" data-bind="foreach: comments">
                <div class="cmt-comment-item cmt-root-comment-item">
                    <a href="#" class="cmt-avatar">
                        <img data-bind="attr: { src: commenterAvatar }" />
                    </a>
                    <div class="cmt-comment-body">
                        <div class="cmt-comment-content">
                            <span data-bind="text: commenterNick" class="cmt-nick"></span>
                            <span data-bind="html: content"></span>
                        </div>
                        <div class="cmt-comment-footer">
                            <span class="cmt-posted-time" data-bind="text: humanizedPostedTime"></span>
                            <% if (IsCommenterAuthenticated)
                               { %>
                            <a href="#" data-bind="click: function ($data, e) { $root.startReply($data, '', e); }">回复</a>
                            <% } %>
                        </div>
                        <div data-bind="visible: replies().length > 0">
                            <div class="cmt-comments cmt-replies" data-bind="foreach: replies">
                                <div class="cmt-comment-item cmt-reply-item">
                                    <a href="#" class="cmt-avatar">
                                        <img data-bind="attr: { src: commenterAvatar }" />
                                    </a>
                                    <div class="cmt-comment-body">
                                        <div class="cmt-comment-content">
                                            <span data-bind="text: commenterNick" class="cmt-nick"></span>
                                            <span data-bind="html: content"></span>
                                        </div>
                                        <div class="cmt-comment-footer">
                                            <span class="cmt-posted-time" data-bind="text: humanizedPostedTime"></span>
                                            <% if (IsCommenterAuthenticated)
                                               { %>
                                            <a href="#" data-bind="click: $root.startInlineReply.bind($data, $parent)">回复</a>
                                            <% } %>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div data-bind="visible: totalUnloadedReplies() > 0" class="cmt-more-replies">
                                <a href="#" data-bind="click: $root.moreReplies">更多回复 <span class="cmt-total-unloaded-replies">(<span data-bind="    text: totalUnloadedReplies"></span>)</span></a>
                            </div>
                        </div>
                        <div data-bind="if: replying" class="cmt-comment-box-container">
                            <div data-bind="template: { name: 'CommentBoxTemplate', data: { model: commentBox, isReply: true } }"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div data-bind="visible: hasMore" class="cmt-more-comments">
                <button type="button" class="btn btn-default btn-block" data-bind="click: more">更多评论 <span class="cmt-total-unloaded-comments">(<span data-bind="    text: totalUnloadedComments"></span>)</span></button>
            </div>
        </div>
    </div>
    <div id="cmt-comment-box">
        <div data-bind="template: { name: 'CommentBoxTemplate', data: { model: $data, isReply: false } }"></div>
    </div>
</div>

<script type="text/html" id="CommentBoxTemplate">
    <%= RenderCommentBoxTemplate() %>
</script>

<script src="/Plugins/Seeger.Plugins.Comments/Scripts/comments.js"></script>
<script>
    $(function () {
        var subjectId = '<%= Subject.Id %>';
        var subjectTitle = '<%= Subject.Title %>';
        var subjectType = '<%= Subject.Type ?? "Default" %>';

        var commentList = new sig.cmt.CommentList({
            container: $('#cmt-comment-list'),
            subjectId: subjectId,
            subjectTitle: subjectTitle,
            subjectType: subjectType
        });

        commentList.applyBindings();
        commentList.load();

        var commentBox = new sig.cmt.CommentBox({
            container: $('#cmt-comment-box'),
            subjectType: subjectType,
            subjectId: subjectId,
            subjectTitle: subjectTitle,
            onSubmitted: function (comment) {
                commentList.prepend(comment);
            }
        });

        commentBox.applyBindings();
        commentBox.init();
    });
</script>
