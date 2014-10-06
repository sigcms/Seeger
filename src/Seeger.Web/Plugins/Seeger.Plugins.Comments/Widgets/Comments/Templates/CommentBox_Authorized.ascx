<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentBox_Authorized.ascx.cs" Inherits="Seeger.Plugins.Comments.Widgets.Comments.Templates.CommentBox_Authorized" %>

<div class="cmt-comment-box">
    <form data-bind="submit: model.submit">
        <div class="form-group">
            <textarea class="form-control" name="Content"
                placeholder="请自觉遵守互联网相关的政策法规，严禁发布色情、暴力、反动的言论"
                data-bind="value: model.content, attr: { 'data-val-required': '请填写' + (isReply ? '回复' : '评论') }"
                data-val="true"></textarea>
            <span data-valmsg-for="Content" data-valmsg-replace="true"></span>
        </div>
        <div class="form-group clearfix cmt-comment-box-actions">
            <div class="dropdown pull-left">
                <button class="btn btn-default dropdown-toggle" data-toggle="dropdown">表情</button>
                <div class="dropdown-menu">
                    <div class="cmt-emotions"><a>(⌒▽⌒)</a><a>（￣▽￣）</a><a>(=・ω・=)</a><a>(｀・ω・´)</a><a>(〜￣△￣)〜</a><a>(･∀･)</a><a>(°∀°)ﾉ</a><a>(￣3￣)</a><a>╮(￣▽￣)╭</a><a>( ´_ゝ｀)</a><a>←_←</a><a>→_→</a><a>(&lt;_&lt;)</a><a>(&gt;_&gt;)</a><a>(;¬_¬)</a><a>("▔□▔)/</a><a>(ﾟДﾟ≡ﾟдﾟ)!?</a><a>Σ(ﾟдﾟ;)</a><a>Σ( ￣□￣||)</a><a>(´；ω；`)</a><a>（/TДT)/</a><a>(^・ω・^ )</a><a>(｡･ω･｡)</a><a>(●￣(ｴ)￣●)</a><a>ε=ε=(ノ≧∇≦)ノ</a><a>(´･_･`)</a><a>(-_-#)</a><a>（￣へ￣）</a><a>(￣ε(#￣) Σ</a><a>ヽ(`Д´)ﾉ</a><a>(╯°口°)╯(┴—┴</a><a>（#-_-)┯━┯</a><a>_(:3」∠)_</a><a>(笑)</a><a>(汗)</a><a>(泣)</a><a>(苦笑)</a></div>
                </div>
            </div>
            <span class="pull-right">
                <a href="#" class="btn btn-link cmt-btn-cancel-comment" data-bind="visible: isReply, click: model.cancel">取消</a>
                <button type="submit" class="btn btn-primary cmt-btn-post-comment" data-bind="text: isReply ? '发表回复' : '发表评论'"></button>
            </span>
        </div>
    </form>
</div>
