<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BcUserInfoList.aspx.cs" Inherits="Stonefw.Web.BaseModule.BcUserInfo.BcUserInfoList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
<form id="form1" runat="server">
    <div class="query">
        <table>
            <tr>
                <td>组别列表：</td>
                <td>
                    <asp:DropDownList ID="ddlGroup" runat="server" class="easyui-combobox" data-options="required:true,panelHeight:'auto',editable:false"></asp:DropDownList>
                </td>
                <td>姓名： </td>
                <td>
                    <asp:TextBox runat="server" ID="txtUserName" class="easyui-textbox"></asp:TextBox>
                </td>
                <td>
                    <asp:LinkButton runat="server" ID="btnQuery" Text="查询" class="easyui-linkbutton" data-options="iconCls:'icon-search'" OnClick="btnQuery_Click"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnAddNew" Text="新增" class="easyui-linkbutton" data-options="iconCls:'icon-add'" OnClientClick="return showDialog('新增', 'BcUserInfoDetail.aspx?id=-1', this);"></asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <asp:GridView ID="gvUserInfo" runat="server" AutoGenerateColumns="False" AllowPaging="True" Width="100%" CssClass="gridview" OnRowCommand="gvUserInfo_RowCommand" OnPageIndexChanged="gvUserInfo_PageIndexChanged" OnPageIndexChanging="gvUserInfo_PageIndexChanging">
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
        <RowStyle HorizontalAlign="Center"></RowStyle>
        <Columns>
            <asp:TemplateField HeaderText="删除" ItemStyle-Width="45px">
                <ItemTemplate>
                    <asp:LinkButton runat="server" class="easyui-linkbutton" CommandName="Row_Delete" CommandArgument='<%# Eval("UserId") %>' OnClientClick="return deleteWarning(this);">删除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改" ItemStyle-Width="45px">
                <ItemTemplate>
                    <a href="#" class="easyui-linkbutton" onclick='<%# "showDialog(\"修改\", \"BcUserInfoDetail.aspx?id=" + Eval("UserId") + "\");" %>'>修改</a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="GroupName" HeaderText="组别"/>
            <asp:BoundField DataField="UserAccount" HeaderText="用户名"/>
            <asp:BoundField DataField="UserName" HeaderText="姓名"/>
            <asp:BoundField DataField="MobilePhone" HeaderText="手机"/>
            <asp:TemplateField HeaderText="状态" ItemStyle-Width="60px">
                <ItemTemplate>
                    <%# (bool) Eval("ActivityFlag") ? "启用" : "停用" %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div class="error">
        <asp:Label ID="lMessage" runat="server"></asp:Label>
    </div>
    <div id="dlg" class="easyui-dialog" data-options="closed:'false'"></div>
</form>
</body>
</html>