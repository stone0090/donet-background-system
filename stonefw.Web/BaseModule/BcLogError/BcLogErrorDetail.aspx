<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BcLogErrorDetail.aspx.cs" Inherits="stonefw.Web.BaseModule.BcLogError.BcLogErrorDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form">
            <table cellpadding="5">
                <tr>
                    <td>Id：</td>
                    <td>
                        <asp:TextBox ID="txtId" runat="server" ReadOnly="True" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>User_Id：</td>
                    <td>
                        <asp:TextBox ID="txtUserId" runat="server" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>User_Name：</td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server" MaxLength="25" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Op_Url：</td>
                    <td>
                        <asp:TextBox ID="txtOpUrl" runat="server" MaxLength="250" TextMode="MultiLine" Rows="3" Width="300" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Op_Time：</td>
                    <td>
                        <asp:TextBox ID="txtOpTime" runat="server" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Op_HostAddress：</td>
                    <td>
                        <asp:TextBox ID="txtOpHostAddress" runat="server" MaxLength="25" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Op_HostName：</td>
                    <td>
                        <asp:TextBox ID="txtOpHostName" runat="server" MaxLength="25" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Op_UserAgent：</td>
                    <td>
                        <asp:TextBox ID="txtOpUserAgent" runat="server" TextMode="MultiLine" Rows="5" Width="300" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Op_QueryString：</td>
                    <td>
                        <asp:TextBox ID="txtOpQueryString" runat="server" MaxLength="0" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Op_HttpMethod：</td>
                    <td>
                        <asp:TextBox ID="txtOpHttpMethod" runat="server" MaxLength="5" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Message：</td>
                    <td>
                        <asp:TextBox ID="txtMessage" TextMode="MultiLine" Rows="20" Width="300" runat="server" M Enabled="False"></asp:TextBox></td>
                </tr>
            </table>
            <div class="error">
                <asp:Label ID="lMessage" runat="server"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
