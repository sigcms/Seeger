<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Editor.aspx.cs" Inherits="Seeger.Plugins.ImageSlider.Widgets.ImageSlider.Editor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <sig:ScriptReference runat="server" Path="/Scripts/jquery/jquery.min.js" />
    <sig:ScriptReference runat="server" Path="/Scripts/jquery/jquery-ui.min.js" />
    <style type="text/css">
        body {
            font-family: '微软雅黑', sans-serif !important;
        }
        .slide-items-table {
            border-collapse: collapse;
            border: #ddd 1px solid;
            width: 100%;
        }
        .slide-items-table td {
            border: 0;
            border-right: #ddd 1px solid;
            border-bottom: #ddd 1px solid;
        }
        .form-group {
            margin-bottom: 5px;
        }
        .form-group label {
            font-weight: bold;
            display: inline-block;
            margin-bottom: 2px;
        }
        .form-group input {
            width: 95%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" EnablePageMethods="true" />
        <div class="mgnt" style="padding:0">
            <table class="formtable" data-bind="with: slider">
                <tr>
                    <th><%= T("Name") %>:</th>
                    <td>
                        <input type="text" data-bind="value: name" />
                    </td>
                </tr>
                <tr>
                    <th><%= T("Size") %>:</th>
                    <td>
                        <input type="text" style="width:40px" data-bind="value: width" />
                        &times;
                        <input type="text" style="width:40px" data-bind="value: height" /> px
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <label>
                            <input type="checkbox" data-bind="checked: showNavigation" />
                            <%= T("Show navigation") %>
                        </label>
                        <label>
                            <input type="checkbox" data-bind="checked: showPagination" />
                            <%= T("Show pagination") %>
                        </label>
                    </td>
                </tr>
                <tr>
                    <th><%= T("Image") %>:</th>
                    <td>
                        <div style="margin-bottom:10px">
                            <button type="button" class="button secondary btn-add-item" data-filter=".jpg;.png">添加图片</button>
                        </div>
                        <table class="slide-items-table" data-bind="visible: items().length > 0">
                            <tbody>
                                <!-- ko foreach: items -->
                                <tr>
                                    <td style="width:150px;">
                                        <div>
                                            <img data-bind="attr: { src: imageUrl }" style="max-height:100px;max-width:150px" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="form-group">
                                            <label><%= T("Caption") %>:</label>
                                            <div>
                                                <input type="text" data-bind="value: caption" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label><%= T("Description") %>:</label>
                                            <div>
                                                <input type="text" data-bind="value: description" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label><%= T("NavigateUrl") %>:</label>
                                            <div>
                                                <input type="text" data-bind="value: navigateUrl" />
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <button type="button" class="button secondary" data-bind="click: $root.deleteItem">删除</button>
                                    </td>
                                </tr>
                                <!-- /ko -->
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <a href="#" class="button primary" data-bind="click: $root.save"><%= T("Common.OK") %></a>
                        <a href="javascript:editorContext.cancel();" class="button secondary"><%= T("Common.Cancel") %></a>
                    </td>
                </tr>
            </table>
        </div>
    </form>
    <script type="text/javascript" src="/Scripts/sig.core.js"></script>
    <script type="text/javascript" src="/Scripts/Resources.ashx"></script>
    <script type="text/javascript">
        window.aspNetAuth = '<%= AspNetAuth %>';

        $(function () {
            var widget = editorContext.widget();

            var defaultName = '<%= DefaultName %>';
            var viewModel = new ViewModel();
            var sliderId = widget.attribute('SliderId') || 0;

            var json = widget.customData['ViewModel'];
            if (json) {
                viewModel = ko.mapping.fromJS(JSON.parse(json), {}, viewModel);
                bindViewModelToView(viewModel);
            } else {
                PageMethods.GetViewModel(sliderId, function (json) {
                    var model = JSON.parse(json);
                    if (!model.slider.name) {
                        model.slider.name = defaultName;
                    }
                    viewModel = ko.mapping.fromJS(model, {}, viewModel);
                    bindViewModelToView(viewModel);
                });
            }

            function ViewModel() {
                var _this = this;

                this.maxDisplayOrder = function () {
                    var max = 0;
                    $.each(_this.slider.items(), function () {
                        var order = parseInt(this.displayOrder(), 10);
                        if (order > max) max = order;
                    });
                    return max;
                }

                this.deleteItem = function (item) {
                    if (!confirm(sig.Resources.get('Message.DeleteConfirm'))) return;
                    _this.slider.items.remove(item);
                }

                this.save = function () {
                    widget.customData['ViewModel'] = ko.mapping.toJSON(_this);
                    widget.markDirty();
                    editorContext.accept();
                }
            }

            function bindViewModelToView(model) {
                ko.applyBindings(model);
            }

            sig.ui.SelectFileButton.init($('.btn-add-item'), {
                allowMultiSelect: true,
                onOK: function (files) {
                    $.each(files, function () {
                        var url = this.publicUri;
                        var item = {
                            caption: ko.observable(''),
                            description: ko.observable(''),
                            imageUrl: ko.observable(url),
                            navigateUrl: ko.observable(''),
                            displayOrder: ko.observable(viewModel.maxDisplayOrder() + 1)
                        };

                        viewModel.slider.items.push(item);
                    });

                    this.close();
                }
            });
        });
    </script>
</body>
</html>
