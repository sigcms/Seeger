<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Seeger.Web.UI.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta name="robots" content="noindex" />
    <style type="text/css">
        body
        {
            padding: 0;
            margin: 0;
            color: #333;
            background: url(Images/login-bg.jpg) repeat-y center;
            font-size: 62.5%;
            font-family: Arial, Verdana, Sans-Serif;
        }
        input
        {
            font-family: Arial, Verdana, Sans-Serif;
        }
        a
        {
            color: #666;
            text-decoration: none;
        }
        a:hover
        {
            text-decoration: underline;
        }
        
        .container
        {
            font-size: 1.1em;
            margin: auto;
            width: 960px;
        }
        .container.zh-CN
        {
            font-size: 1.3em;
        }
        
        .text-input
        {
            padding: 4px;
            border: #999 1px solid;
        }
        .button {
	        display: inline-block;
	        zoom: 1; /* zoom and *display = ie7 hack for display:inline-block */
	        *display: inline;
	        vertical-align: baseline;
	        outline: none;
	        cursor: pointer;
	        text-align: center;
	        text-decoration: none;
	        padding: .3em 1em .3em;
        }
        .button:hover 
        {
	        text-decoration: none;	
        }
        .button:active {
	        position: relative;
	        top: 1px;
        }

        /* primary */
        .primary 
        {
            font-weight: bold;
	        color: #e8f0de;
	        border: solid 1px #538312;
	        background: #64991e;
        }
        .primary:hover 
        {
            color: #fff;
	        background: #538018;
        }
 
        .secondary 
        {
	        color: #606060;
	        border: solid 1px #b7b7b7;
	        background: #fff;
        }
        .secondary:hover
        {
	        background: #ededed;
        }
        
        .main-panel
        {
            padding: 145px 0 60px;
            background: url(images/login-panel-bg.jpg) no-repeat 10px 30px;
        }
        .login-form
        {
            width: 330px;
            float: right;
            margin-right: 140px;
            padding: 30px 10px;
            border: #999 1px solid;
            background-color: #fff;
	        box-shadow: 1px 1px 2px rgba(0,0,0,.3);
	        -moz-box-shadow: 1px 1px 2px rgba(0,0,0,.3);
	        -webkit-box-shadow: 1px 1px 2px rgba(0,0,0,.3);
        }
        .login-table
        {
            width: 100%;
            border-collapse: collapse;
        }
        .login-table th, td
        {
            font-weight: normal;
            padding: 5px;
        }
        .login-table th
        {
            text-align: right;
        }
        .footer
        {
            text-align: center;
            padding: 80px 0 20px;
            clear: both;
        }
        .footer .copyright
        {
            padding-top: 10px;
            color: #999;
            line-height: 2em;
            font-size: 12px;
        }
        .footer .copyright a
        {
            color: #999;
        }
        
        .message 
        {
            padding: 5px;
            text-align: center;
        }
        .error
        {
            color: red;
        }
   </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container <%= System.Globalization.CultureInfo.CurrentUICulture.Name %>">
        <div class="main-panel">
            <div class="login-form">
                <sig:MessagePanel runat="server" ID="Message" Visible="false" MessageType="Error" />
                <table class="login-table">
                    <tr>
                        <th>
                            <%= Localize("Login.UserName") %>
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="Name" CssClass="text-input"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="NameRequiredValidator"
                                ControlToValidate="Name" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <%= Localize("Login.Password") %>
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="text-input"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="PasswordRequiredValidator"
                                ControlToValidate="Password" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="LoginButton" CssClass="button primary" Text="<%$ Resources: Login.Login %>"
                                OnClick="LoginButton_Click" />

                            <a href="/" class="button secondary"><%= Localize("Login.BackToHomepage") %></a>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="clear:both"></div>
        </div>
        <div class="footer">
            <div class="links">
                <a href="<%= SeegerUrls.Homepage %>" target="_blank"><%= Localize("Seeger.Homepage")%></a>
                |
                <a href="<%= SeegerUrls.Purchase %>", target="_blank"><%= Localize("Seeger.Purchase") %></a>
                |
                <a href="<%= SeegerUrls.Help %>" target="_blank"><%= Localize("Seeger.Help") %></a>
                |
                <a href="<%= SeegerUrls.Contact %>" target="_blank"><%= Localize("Seeger.Contact") %></a>
                |
                <a href="<%= SeegerUrls.About %>" target="_blank"><%= Localize("Seeger.About") %></a>
            </div>
            <div class="copyright">
                &copy; 2011<%= DateTime.Today.Year == 2011 ? "" : " - " + DateTime.Today.Year %> <a href="<%= SeegerUrls.Homepage %>" target="_blank" title='<%= Localize("Seeger.ShortName") %>'><%= Localize("Seeger.ShortName") %></a>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
