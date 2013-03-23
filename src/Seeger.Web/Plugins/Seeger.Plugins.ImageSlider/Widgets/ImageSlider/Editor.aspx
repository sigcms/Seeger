<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Editor.aspx.cs" Inherits="Seeger.Plugins.ImageSlider.Widgets.ImageSlider.Editor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                    <th><%= T("Image") %>:</th>
                    <td>
                        <div>
                            <input type="file" id="ImageUpload" />
                        </div>
                        <table class="datatable">
                            <thead>
                                <tr>
                                    <th><%= T("Image") %></th>
                                    <th><%= T("Caption") %></th>
                                    <th><%= T("NavigateUrl") %></th>
                                    <th><%= T("Common.Operations") %></th>
                                </tr>
                            </thead>
                            <tbody>
                                <!-- ko foreach: items -->
                                <tr data-bind="visible: $root.editedItemClientId() !== clientId()" style="display:none">
                                    <td>
                                        <img data-bind="attr: { src: imageThumbUrl }" />
                                    </td>
                                    <td data-bind="html: caption" style="vertical-align:middle"></td>
                                    <td data-bind="html: navigateUrl" style="vertical-align:middle"></td>
                                    <td style="text-align:center;vertical-align:middle">
                                        <a href="#" data-bind="click: $root.editItem"><%= T("Common.Edit") %></a>
                                        <a href="#" data-bind="click: $root.deleteItem"><%= T("Common.Delete") %></a>
                                    </td>
                                </tr>
                                <tr data-bind="visible: $root.editedItemClientId() == clientId()" style="display:none">
                                    <td colspan="4" data-bind="with: $root.editedItemClone">
                                        <table class="formtable">
                                            <tr>
                                                <th><%= T("Image") %></th>
                                                <td>
                                                    <img data-bind="attr: { src: imageThumbUrl }" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th><%= T("Caption") %></th>
                                                <td>
                                                    <input type="text" data-bind="value: caption" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th><%= T("Description") %></th>
                                                <td>
                                                    <input type="text" data-bind="value: description" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th><%= T("NavigateUrl") %></th>
                                                <td>
                                                    <input type="text" data-bind="value: navigateUrl" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th></th>
                                                <td>
                                                    <button type="button" class="button primary" data-bind="click: $root.acceptEditedItem"><%= T("Common.OK") %></button>
                                                    <button type="button" class="button secondary" data-bind="click: $root.cancelEditItem"><%= T("Common.Cancel") %></button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <!-- /ko -->
                                <tr data-bind="visible: items().length == 0" class="no-record">
                                    <td colspan="4">
                                        <%= T("Message.NoRecordToDisplay") %>
                                    </td>
                                </tr>
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
    <script type="text/javascript" src="/Scripts/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="/Scripts/sig.core.js"></script>
    <script type="text/javascript" src="/Scripts/Resources.ashx"></script>
    <script type="text/javascript">
        $(function () {

            var aspNetAuth = '<%= AspNetAuth %>';

            var viewModel = new ViewModel();
            var sliderId = getSliderIdFromEditorContext() || <%= SliderId %>;
            var $uploader = $('#ImageUpload');

            initImageUploader();

            var json = getViewModelJsonFromEditorContext();
            if (json) {
                viewModel = ko.mapping.fromJS(JSON.parse(json), {}, viewModel);
                bindViewModelToView(viewModel);
            } else {
                PageMethods.GetViewModel(sliderId, function (json) {
                    var model = JSON.parse(json);
                    // assign item client ids
                    $.each(model.slider.items, function (i) {
                        this.clientId = i + 1;
                    });

                    viewModel = ko.mapping.fromJS(model, {}, viewModel);
                    bindViewModelToView(viewModel);
                });
            }

            function ViewModel() {
                var _this = this;

                this.editedItemClientId = ko.observable(-1);
                this.editedItem = ko.observable();
                this.editedItemClone = ko.observable();

                this.maxDisplayOrder = function () {
                    var max = 0;
                    $.each(_this.slider.items(), function () {
                        var order = parseInt(this.displayOrder(), 10);
                        if (order > max) max = order;
                    });
                    return max;
                }

                this.maxClientId = function () {
                    var max = 0;
                    $.each(_this.slider.items(), function () {
                        var id = parseInt(this.clientId(), 10);
                        if (id > max) max = id;
                    });
                    return max;
                }

                this.findItemByClientId = function (clientId) {
                    var item = null;
                    $.each(_this.items(), function () {
                        if (this.clientId() == clientId) {
                            item = this;
                            return false;
                        }
                    });

                    return item;
                }

                this.editItem = function (item) {
                    _this.editedItem(item);
                    _this.editedItemClone(ko.mapping.fromJS(ko.mapping.toJS(item)));
                    _this.editedItemClientId(item.clientId());
                }

                this.acceptEditedItem = function () {
                    var editedItem = _this.editedItem();
                    ko.mapping.fromJS(ko.mapping.toJS(_this.editedItemClone), {}, editedItem);
                    _this.editedItemClientId(-1);
                    _this.editedItem(null);
                    _this.editedItemClone(null);
               }

                this.cancelEditItem = function () {
                    _this.editedItemClientId(-1);
                    _this.editedItem(null);
                    _this.editedItemClone(null);
                }

                this.deleteItem = function (item) {
                    if (!confirm(sig.Resources.get('Message.DeleteConfirm'))) return;
                    _this.slider.items.remove(item);
                }

                this.save = function () {
                    var stateItem = editorContext.get_stateItem();
                    var customData = stateItem.get_customData();
                    customData['ViewModelJson'] = ko.mapping.toJSON(_this);
                    editorContext.accept();
                }
            }

            function bindViewModelToView(model) {
                ko.applyBindings(model);
            }

            function getSliderIdFromEditorContext() {
                var attrManager = editorContext.get_attributeManager();
                var sliderId = attrManager.getValue('SliderId') || 0;
                return sliderId;
            }

            function getViewModelJsonFromEditorContext() {
                var stateItem = editorContext.get_stateItem();
                return stateItem.get_customData()['ViewModelJson'];
            }

            function initImageUploader() {
                $uploader.uploadify({
                    swf: '/Scripts/uploadify/uploadify.swf',
                    uploader: '/Plugins/Seeger.Plugins.ImageSlider/Handlers/UploadImage.ashx',
                    fileTypeExts: '*.jpg;*.jpeg;*.png;',
                    fileTypeDesc: '<%= T("Image") %>',
                    multi: false,
                    queueSizeLimit: 1,
                    fileSizeLimit: '5MB',
                    removeTimeout: 0,
                    formData: { AspNetAuth: aspNetAuth },
                    buttonText: '<%= T("UploadImage") %>...',
                    onUploadSuccess: function (file, data, response) {
                        var result = JSON.parse(data);
                        if (result.Success) {
                            var jsModel = JSON.parse(result.SliderItemViewModel);
                            jsModel.clientId = viewModel.maxClientId() + 1;
                            jsModel.displayOrder = viewModel.maxDisplayOrder() + 1;
                            var item = ko.mapping.fromJS(jsModel);
                            viewModel.slider.items.push(item);
                            viewModel.editItem(item);
                        } else {
                            sig.ui.Message.error(result.Message);
                        }
                    }
                });
            }
        });
    </script>
</body>
</html>
