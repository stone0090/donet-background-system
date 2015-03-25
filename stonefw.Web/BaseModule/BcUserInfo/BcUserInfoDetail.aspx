<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BcUserInfoDetail.aspx.cs" Inherits="stonefw.Web.BaseModule.BcUserInfo.BcUserInfoDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        //页面加载时，初始化ddlRole控件的值
        $(function () {
            if ($.isEmpty($("#hdRoleIds").val()))
                $("#ddlRole").combobox("clear");
            else
                $("#ddlRole").combobox("setValues", $("#hdRoleIds").val().split(","));
        });
        //页面保存时，把ddlRole控件的值保存到hdRoleIds中
        function setHdRoleIdsValue() {
            if ($.isEmpty($("#ddlRole").combobox("getText")))
                $("#hdRoleIds").val("");
            else
                $("#hdRoleIds").val($("#ddlRole").combobox("getValues"));
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hdUserId" />
        <asp:HiddenField runat="server" ID="hdRoleIds" />
        <div class="form">
            <table cellpadding="5">
                <tr>
                    <td>用户名：</td>
                    <td>
                        <asp:TextBox ID="txtUserAccount" runat="server" MaxLength="25" class="easyui-textbox" data-options="required:true"></asp:TextBox>
                    </td>
                    <td></td>
                    <td>密码：</td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" MaxLength="25" class="easyui-textbox" data-options="required:true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>姓名：</td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server" MaxLength="25" class="easyui-textbox" data-options="required:true"></asp:TextBox></td>
                    <td></td>
                    <td>组别：</td>
                    <td>
                        <asp:DropDownList ID="ddlGroup" runat="server" Width="100%" class="easyui-combobox" data-options="required:true,panelHeight:'auto',editable:false" />
                    </td>

                </tr>
                <tr>
                    <td>手机：</td>
                    <td>
                        <asp:TextBox ID="txtMobilePhone" runat="server" MaxLength="25" class="easyui-textbox"></asp:TextBox></td>
                    <td></td>
                    <td>座机：</td>
                    <td>
                        <asp:TextBox ID="txtOfficePhone" runat="server" MaxLength="25" class="easyui-textbox"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>邮件：</td>
                    <td colspan="4">
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="100%" class="easyui-textbox"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>角色：</td>
                    <td colspan="4">
                        <asp:DropDownList ID="ddlRole" runat="server" Width="100%" class="easyui-combobox" data-options="panelHeight:'auto',multiple:true,editable:false" /></td>
                </tr>
                <tr>
                    <td>性别：</td>
                    <td>
                        <asp:RadioButton runat="server" Text="男" GroupName="sex" Checked="True" ID="rMale" />
                        <asp:RadioButton runat="server" Text="女" GroupName="sex" ID="rFemale" />
                    </td>
                    <td></td>
                    <td>状态：</td>
                    <td>
                        <asp:RadioButton runat="server" Text="启用" GroupName="activityFlag" Checked="True" ID="rEnable" />
                        <asp:RadioButton runat="server" Text="停用" GroupName="activityFlag" ID="rDisable" />
                    </td>
                </tr>
            </table>
            <div class="error">
                <asp:Label ID="lMessage" runat="server"></asp:Label>
            </div>
            <div>
                <asp:LinkButton ID="btnSave" runat="server" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" OnClientClick="setHdRoleIdsValue();return saveForm(this);" OnClick="btnSave_Click">保存</asp:LinkButton>
                <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="#" onclick="window.parent.closeDialog();">取消</a>
            </div>
        </div>
    </form>
</body>
</html>
