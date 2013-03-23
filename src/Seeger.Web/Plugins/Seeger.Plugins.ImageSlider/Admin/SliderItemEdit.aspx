<%@ Page Title="Add/Edit slider item" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SliderItemEdit.aspx.cs" Inherits="Seeger.Plugins.ImageSlider.Admin.SliderItemEdit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

    <table class="formtable">
        <tr>
            <th><%= T("Slider name") %></th>
            <td>
                <%= Slider.Name %>
            </td>
        </tr>
        <tr>
            <th><%= T("Caption") %></th>
            <td>
                <asp:TextBox runat="server" ID="Caption" MaxLength="300" Width="300" />
            </td>
        </tr>
        <tr>
            <th><%= T("Image") %></th>
            <td>
                <asp:TextBox runat="server" ID="ImageUrl" Width="300" />
                <button type="button" id="btn-select-image"><%= T("Select image") %></button>
                <asp:RequiredFieldValidator ErrorMessage="<%$ T: Please enter the image url or select an image %>" ControlToValidate="ImageUrl" runat="server" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <th><%= T("NavigateUrl") %></th>
            <td>
                <asp:TextBox runat="server" ID="NavigateUrl" Width="300" />
            </td>
        </tr>
        <tr>
            <th><%= T("DisplayOrder") %></th>
            <td>
                <asp:TextBox runat="server" ID="DisplayOrder" Width="100" Text="0" />
                <asp:RequiredFieldValidator ErrorMessage="<%$ T: Display order is required %>" ControlToValidate="DisplayOrder" runat="server" Display="Dynamic" />
                <asp:RegularExpressionValidator ErrorMessage="<%$ T: Display order should be an integer %>" ControlToValidate="DisplayOrder" runat="server" Display="Dynamic" ValidationExpression="^\d+$" />
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:Button runat="server" ID="SaveButton" CssClass="button primary" Text="<%$ T: Save %>" OnClick="SaveButton_Click" />
                <a href="SliderItems.aspx?sliderId=<%= SliderId %>" class="button secondary"><%= T("Cancel") %></a>
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        $(function () {
            var dialog = new sig.ui.SelectFileDialog({
                filter: '.jpg;.jpeg;.png;.gif',
                allowMultiSelect: false,
                onOK: function (files) {
                    $('#<%= ImageUrl.ClientID %>').val(files[0].virtualPath);
                    dialog.close();
                }
            });

            $('#btn-select-image').click(function () {
                dialog.open();
                return false;
            });
        });
    </script>

</asp:Content>
