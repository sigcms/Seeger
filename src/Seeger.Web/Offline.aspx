<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Offline.aspx.cs" Inherits="Seeger.Web.UI.Offline" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .offline-hint
        {
            font-size: 24px;
            text-align: center;
            margin: auto;
            padding: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="offline-hint">
    <%= OfflineHint %>
    </div>
    </form>
</body>
</html>
