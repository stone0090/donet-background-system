<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysGlobalSettingDetail.aspx.cs" Inherits="Stonefw.Web.SystemModule.SysGlobalSetting.SysGlobalSettingDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
<form id="form1" runat="server">
    <div class="form">
        <table cellpadding="5">
            <tr>
                <td>系统名称:</td>
                <td>
                    <asp:TextBox ID="txtSysName" runat="server"
                                 class="easyui-validatebox" data-options="required:true" Width="350">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>系统说明:</td>
                <td>
                    <asp:TextBox ID="txtSysDescription" runat="server"
                                 class="easyui-validatebox" data-options="required:true" Width="350">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>错误页面:</td>
                <td>
                    <asp:TextBox ID="txtErrorPage" runat="server"
                                 class="easyui-validatebox" data-options="required:true" Width="350">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>建设页面:</td>
                <td>
                    <asp:TextBox ID="txtBuildingPage" runat="server"
                                 class="easyui-validatebox" data-options="required:true" Width="350">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>错误日志路径:</td>
                <td>
                    <asp:TextBox ID="txtErrorLogPath" runat="server"
                                 class="easyui-validatebox" data-options="required:true" Width="350">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>超级管理员:</td>
                <td>
                    <asp:TextBox ID="txtSuperAdmins" runat="server" placeholder="请用逗号分隔"
                                 class="easyui-validatebox" data-options="required:true" Width="350">
                    </asp:TextBox>
                    如需设置多个超级管理员，请用英文逗号分隔！
                </td>
            </tr>
            <tr>
                <td>默认分页数:</td>
                <td>
                    <asp:TextBox ID="txtGridviewPageSize" runat="server"
                                 class="easyui-validatebox" data-options="required:true" Width="350">
                    </asp:TextBox>
                </td>
            </tr>
        </table>
        <div class="error">
            <asp:Label ID="lMessage" runat="server"></asp:Label>
        </div>
        <div>
            <asp:LinkButton ID="btnSave" runat="server" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" OnClientClick="return saveForm(this);" OnClick="btnSave_Click">保存</asp:LinkButton>
        </div>
    </div>
</form>
</body>
</html>