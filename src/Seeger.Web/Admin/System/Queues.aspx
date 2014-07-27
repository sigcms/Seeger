<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../Admin.master" CodeBehind="Queues.aspx.cs" Inherits="Seeger.Web.UI.Admin._System.Queues" %>

<asp:Content runat="server" ContentPlaceHolderID="MainHolder">
    <div class="page-header">
        <h1>
            <%= T("Task Queues") %>
        </h1>
    </div>
    <div id="task-queues" data-bind="foreach: queues">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">
                    <a href="#" data-bind="text: displayName"></a>
                </h3>
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-sm-3">
                        <div class="num-box">
                            <label class="num-label"><%= T("Pending") %></label>
                            <span class="value text-primary" data-bind="text: statistics.statusCounts.pending"></span>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="num-box">
                            <label class="num-label"><%= T("In Progress") %></label>
                            <span class="value text-success" data-bind="text: statistics.statusCounts.inProgress"></span>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="num-box">
                            <label class="num-label"><%= T("Failed") %></label>
                            <span class="value text-danger" data-bind="text: statistics.statusCounts.failed"></span>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="num-box">
                            <label class="num-label"><%= T("Aborted") %></label>
                            <span class="value text-warning" data-bind="text: statistics.statusCounts.aborted"></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>

        $(function () {

            function TaskQueuesModel() {
                var self = this;

                self.queues = ko.observableArray();
            }

            $.get('/api/admin/queues')
             .done(function (data) {
                 var model = {
                     queues: data
                 };

                 var vm = ko.mapping.fromJS(model, {}, new TaskQueuesModel());
                 ko.applyBindings(vm, document.getElementById('task-queues'));
             });
        })

    </script>

</asp:Content>