<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Stonefw.Web.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title><%= SysGlobalSetting.SysName %></title>
    <link rel="stylesheet" type="text/css" href="/Resource/login/css/style.css"/>
    <style type="text/css">
        .download {
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            background: #e6e6e6;
            border: 1px dashed #df0031;
            border-radius: 3px;
            color: #555;
            font-family: Comic Sans MS;
            font-size: 14px;
            font-weight: bolder;
            margin: 20px 33px 10px;
            *margin-bottom: 30px;
            padding: 5px;
        }

        .download a {
            color: #555;
            font-size: 14px;
            font-weight: normal;
            letter-spacing: 1px;
            padding-left: 5px;
            text-decoration: none;
        }

        .download a:hover {
            color: #36F;
            text-decoration: underline;
        }

        .download span { float: right; }
    </style>
    <script>

        if (top.location != self.location) {
            top.location = self.location;
        }

        function login() {
            var usId = $("#name").val().trim();
            var pswd = $("#password").val().trim();

            if (usId == "") {
                $("#divError").removeClass("noShow");
                $("#errormsg").text("登录失败，用户名不能为空！");
                this.errormsg.Text = "登录失败，用户名不能为空！";
                return false;
            }

            if (pswd == "") {
                $("#divError").removeClass("noShow");
                $("#errormsg").text("登录失败，密码不能为空！");
                return false;
            }

            return true;
        }
    </script>
</head>
<body>
<form id="form1" runat="server">
    <div class="main">
        <div class="header"><%= SysGlobalSetting.SysName %></div>
        <div class="content">
            <div class="title"> 用户登录</div>
            <fieldset>
                <div class="input">
                    <asp:TextBox runat="server" ID="name" MaxLength="24" placeholder="用户名" class="input_all name" onfocus="this.className='input_all name_now';" onblur="this.className='input_all name'"> </asp:TextBox>
                </div>
                <div class="input">
                    <asp:TextBox runat="server" TextMode="Password" ID="password" MaxLength="24" Text="密码" placeholder="密码" class="input_all password" onfocus="this.className='input_all password_now';" onblur="this.className='input_all password'"></asp:TextBox>
                </div>
                <div runat="server" id="divError" class="error noShow">
                    <span class="error-icon"></span>
                    <asp:Label runat="server" ID="errormsg" Text="123"></asp:Label>
                </div>
                <div class="checkbox">
                    <asp:CheckBox runat="server" Text="记住密码" ID="remember"/>
                </div>
                <div class="enter">
                    <asp:Button runat="server" ID="submit" Text="登陆" class="button hide" OnClick="submit_Click" OnClientClick="return login();"/>
                </div>
            </fieldset>
        </div>
    </div>
    <script type="text/javascript" src="/Resource/login/js/placeholder.js"></script>
    <!--[if IE 6]>
        <script type="text/javascript" src="/Resource/login/js/belatedpng.js"></script>
        <script type="text/javascript">DD_belatedPNG.fix("*");</script>
    <![endif]-->
</form>
</body>
</html>