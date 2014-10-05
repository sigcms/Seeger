<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Default.ascx.cs" Inherits="Seeger.Plugins.Comments.Widgets.Comments.Default" %>

<div id="cmt-comment-list">
    <div data-bind="visible: loading">
        <i class="fa fa-spin fa-refresh"></i>
    </div>
    <div data-bind="visible: !loading()">
        <ul class="media-list" data-bind="foreach: comments">
            <li class="media cmt-comment-item">
                <a href="#" class="pull-left">
                    <img class="media-object" src="http://placehold.it/64x64" />
                </a>
                <div class="media-body">
                    <h4 class="media-heading">
                        <span data-bind="text: commenterNick"></span>
                    </h4>
                    <p data-bind="html: content"></p>
                    <p>
                        <span data-bind="text: postedTimeUtc"></span>
                        <a href="#" data-bind="click: $root.toggleReply">回复</a>
                    </p>
                    <div data-bind="visible: replies().length > 0">
                        <ul class="media-list" data-bind="foreach: replies">
                            <li class="media">
                                <a href="#" class="pull-left">
                                    <img class="media-object" src="http://placehold.it/40x40" />
                                </a>
                                <div class="media-body">
                                    <p>
                                        <span data-bind="text: commenterNick"></span>
                                        <span data-bind="html: content"></span>
                                    </p>
                                    <p>
                                        <span data-bind="text: postedTimeUtc"></span>
                                    </p>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <div data-bind="if: replying" class="cmt-comment-box-container">
                        <div data-bind="template: { name: 'CommentBoxTemplate', data: { submitHandler: $root.submitReply, model: $root.activeComment.commentBox, isReply: true } }"></div>
                    </div>
                </div>
            </li>
        </ul>
        <div data-bind="visible: hasMore">
            <button type="button" class="btn btn-default btn-block" data-bind="click: more">加载更多...</button>
        </div>
    </div>
</div>
<div id="cmt-comment-box">
    <div data-bind="template: { name: 'CommentBoxTemplate', data: { model: $data, isReply: false } }"></div>
</div>

<script type="text/html" id="CommentBoxTemplate">
    <div class="cmt-comment-box">
        <form data-bind="submit: model.submit">
            <div class="form-group">
                <textarea class="form-control" name="Content" 
                    data-bind="value: model.content, attr: { 'data-val-required': '请填写' + (isReply ? '回复' : '评论') }"
                    data-val="true"></textarea>
                <span data-valmsg-for="Content" data-valmsg-replace="true"></span>
            </div>
            <div class="form-group">
                <button type="submit" class="btn btn-primary" data-bind="text: isReply ? '发表回复' : '发表评论'"></button>
            </div>
        </form>
    </div>
</script>

<script src="/Plugins/Seeger.Plugins.Comments/Scripts/comments.js"></script>
<script>
    $(function () {
        var subjectId = '<%= Subject.Id %>';
        var subjectTitle = '<%= Subject.Title %>';

        var commentList = new sig.cmt.CommentList({
            container: $('#cmt-comment-list'),
            subjectId: subjectId
        });

        commentList.applyBindings();
        commentList.load();

        var commentBox = new sig.cmt.CommentBox({
            container: $('#cmt-comment-box'),
            subjectId: subjectId,
            subjectTitle: subjectTitle,
            onSubmitted: function (comment) {
                commentList.prepend(comment);
            }
        });

        commentBox.applyBindings();
        commentBox.initValidation();
    });
</script>
