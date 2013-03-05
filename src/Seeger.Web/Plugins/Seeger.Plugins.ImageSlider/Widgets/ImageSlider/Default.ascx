<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Default.ascx.cs" Inherits="Seeger.Plugins.ImageSlider.Widgets.ImageSlider.Default" %>

<div id="slide-<%= ClientID %>" class="slider-widget">
    <div class="slides_container">
        <% foreach (var item in Items) { %>
            <div class="slide">
                <a href="<%= item.NavigateUrl %>" title="<%= item.Caption %>" target="_blank">
                    <img src="<%= item.ImageUrl %>" alt="<%= item.Caption %>" />
                </a>
                <% if (!String.IsNullOrEmpty(item.Caption)) { %>
                    <div class="caption" style="bottom:0">
                        <p><%= item.Caption %></p>
                    </div>
                <% } %>
            </div>
        <% } %>
    </div>
</div>

<sig:ScriptReference ID="ScriptReference1" runat="server" Path="/Scripts/jquery/jquery.min.js" />
<script type="text/javascript">
    (function ($) {

        $(function () {
            var $widget = $('#slide-<%= ClientID %>');

            $widget.slides({
                preload: true,
                preloadImage: '/Plugins/Seeger.Plugins.ImageSlider/Scripts/loading.gif',
                play: 5000,
                pause: 2500,
                hoverPause: true,
                effect: 'fade',
                crossfade: true,
                slidesLoaded: function () {
                    var $firstImage = $widget.find('img:first');
                    var width = $firstImage.width();
                    var height = $firstImage.height();
                    $widget.width(width).height(height);
                    $widget.find('.slides_control .slide').width(width).height(height);
                    $widget.find('.caption').width(width).fadeIn();
                    $widget.find('.pagination').fadeIn();
                }
            });
        });
    })(jQuery);
</script>