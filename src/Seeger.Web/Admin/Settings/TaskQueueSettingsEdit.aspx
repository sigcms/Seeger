<%@ Page Title="{ Menu.TaskQueueSettings }" Language="C#" MasterPageFile="~/Admin/Shared/Management.Master" AutoEventWireup="true" CodeBehind="TaskQueueSettingsEdit.aspx.cs" Inherits="Seeger.Web.UI.Admin.Settings.TaskQueueSettingsEdit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

<table class="formtable">
    <tr>
        <th><%= T("TaskQueue.IntervalInMinutes")%></th>
        <td>
            <asp:TextBox runat="server" ID="Interval" Width="50" />
            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="Interval"
                runat="server" Display="Dynamic" />
            <asp:RegularExpressionValidator ErrorMessage="<%$ T: TaskQueue.IntervalInMinuesErrorMessage %>" ControlToValidate="Interval"
                runat="server" Display="Dynamic" ValidationExpression="^\d*[1-9]\d*$" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:CheckBox runat="server" ID="Enable" Text="<%$ T: Common.Enable %>" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td>
            <asp:Button runat="server" ID="SaveButton" Text="<%$ T: Common.Save %>" CssClass="button primary"
                 OnClick="SaveButton_Click" />
        </td>
    </tr>
</table>

</asp:Content>

