﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Default.ascx.cs" Inherits="Seeger.Plugins.ImageSlider.Widgets.ImageSlider.Default" %>

<div id="slide-<%= ClientID %>" class="sig-slider-widget">
    <% foreach (var item in Items)
       { %>
        <div>
            <a href="<%= item.NavigateUrl %>" title="<%= item.Caption %>" target="_blank">
                <img src="<%= item.ImageUrl %>" alt="<%= item.Caption %>" style="width:<%= Slider.Width == null ? "auto" : Slider.Width.Value.ToString() %>px;height:<%= Slider.Height == null ? "auto" : Slider.Height.Value.ToString() %>px;" />
            </a>
            <% if (!String.IsNullOrEmpty(item.Caption)) { %>
                <div class="caption" style="bottom: 0">
                    <p><%= item.Caption %></p>
                </div>
            <% } %>
        </div>
    <% } %>
</div>

<sig:ScriptReference ID="ScriptReference1" runat="server" Path="/Scripts/jquery/jquery.min.js" />
<script type="text/javascript">
    (function ($) {

        $(function () {
            var $widget = $('#slide-<%= ClientID %>');

            $widget.slidesjs({
                width: <%= Slider.Width %>,
                height: <%= Slider.Height %>,
                navigation: {
                    active: <%= Slider.ShowNavigation ? "true" : "false" %>
                },
                pagination: {
                    active: <%= Slider.ShowPagination ? "true" : "false" %>
                }
            });
        });
    })(jQuery);
</script>
