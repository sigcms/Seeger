<%@ Page Title="App Keep-Alive Settings" Language="C#" MasterPageFile="Admin.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="Seeger.Plugins.AppKeepAlive.Admin.Settings" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

    <div class="page-header">
        <h1>App Keep-Alive Settings</h1>
    </div>

    <div class="alert alert-info">
        <p>
            <%= T("Current status") %>:
            <% if (IsWorkerThreadRunning) { %>
                <%= T("Running") %>
            <% } else { %>
                <%= T("Stopped") %>
            <% } %>
        </p>
        <p>
            <% if (IsWorkerThreadRunning) { %>
                <button type="button" class="btn-stop btn btn-danger"><%= T("Stop") %></button>
            <% } else { %>
                <button type="button" class="btn-start btn btn-success"><%= T("Start") %></button>
            <% } %>
        </p>
    </div>

    <table class="formtable">
        <tr>
            <th><%= T("Url") %></th>
            <td>
                <asp:TextBox runat="server" ID="Url" Width="300" MaxLength="255" />
                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="Url" runat="server" />
            </td>
        </tr>
        <tr>
            <th><%= T("Interval") %></th>
            <td>
                <asp:TextBox runat="server" ID="Interval" Width="60" MaxLength="10" />
                <span><%= T("minutes") %></span>
                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="Interval" runat="server" Display="Dynamic" />
                <asp:RegularExpressionValidator ErrorMessage="<%$ T: Interval should be integer %>" ControlToValidate="Interval" runat="server"
                     ValidationExpression="^\d+$" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:Button runat="server" ID="SaveButton" CssClass="button primary" OnClick="SaveButton_Click" Text="<%$ T: Common.Save %>" />
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        $(function () {
            $('.btn-start').click(function () {
                sig.ui.Message.show(sig.Resources.get('Starting') + '...');
                PageMethods.StartWorker(ajaxSuccess, ajaxError);
                return false;
            });

            $('.btn-stop').click(function () {
                sig.ui.Message.show(sig.Resources.get('Stopping') + '...');
                PageMethods.StopWorker(ajaxSuccess, ajaxError);
                return false;
            });

            function ajaxSuccess() {
                sig.ui.Message.success(sig.Resources.get('OperationSuccess'));
                setTimeout(function () {
                    location.href = location.href;
                }, 500);
            }

            function ajaxError(e) {
                sig.ui.Message.error(e.get_message());
            }
        });
    </script>

</asp:Content>
