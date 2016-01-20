<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CuCustomerDetail.aspx.cs" Inherits="Stonefw.Web.CustomerModule.CuCustomer.CuCustomerDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript">
        function getAddNewUrl(self) {
            return showDialog('新增', 'CuContactPersonDetail.aspx?cpid=-1&cuid=' + $('#txtCuId').val(), self);
        }

        function getUpdateUrl(cpid) {
            return showDialog('修改', 'CuContactPersonDetail.aspx?cpid=' + cpid + '&cuid=' + $('#txtCuId').val());
        }
    </script>
</head>
<body>
<form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="hdCuId"/>
    <div class="query">
        <table>
            <tr>
                <td>
                    <asp:LinkButton runat="server" ID="btnQuery" Text="查询" OnClick="btnQuery_Click" Style="display: none;" class="easyui-linkbutton" data-options="iconCls:'icon-search'"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnAddNew" class="easyui-linkbutton" data-options="iconCls:'icon-add'" OnClientClick="return getAddNewUrl(this);">新增联系人</asp:LinkButton>
                    <asp:LinkButton ID="btnSave" runat="server" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" OnClientClick="return saveForm(this);" OnClick="btnSave_Click">保存</asp:LinkButton>
                    <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="#" onclick="window.parent.closeSelectedTab();">取消</a>
                </td>
            </tr>
        </table>
    </div>
    <div class="form">
        <table cellpadding="5">
            <tr>
                <td>编号：</td>
                <td>
                    <asp:TextBox ID="txtCuId" runat="server" MaxLength="25" class="easyui-textbox" data-options="required:true"></asp:TextBox>
                </td>
                <td>地区：</td>
                <td>
                    <asp:TextBox ID="txtDistrict" runat="server" MaxLength="25" class="easyui-textbox"></asp:TextBox>
                </td>
                <td>姓名：</td>
                <td>
                    <asp:TextBox ID="txtCuName" runat="server" MaxLength="25" class="easyui-textbox" data-options="required:true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>地址：</td>
                <td colspan="5">
                    <asp:TextBox ID="txtAddress" runat="server" MaxLength="250" Width="100%" class="easyui-textbox"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>备注：</td>
                <td colspan="5">
                    <asp:TextBox ID="txtRemark" runat="server" Width="100%" TextMode="MultiLine" class="easyui-textbox" data-options="multiline:true" Style="height: 100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>状态：</td>
                <td colspan="5">
                    <asp:RadioButton ID="rEnabled" runat="server" Text="启用" GroupName="activityFlag" Checked="True"/>
                    <asp:RadioButton ID="rDisabled" runat="server" Text="停用" GroupName="activityFlag"/>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:GridView ID="gvCuContactPerson" runat="server" AutoGenerateColumns="False" AllowPaging="True" Width="100%" CssClass="gridview" OnRowCommand="gvCuContactPerson_RowCommand" OnPageIndexChanged="gvCuContactPerson_PageIndexChanged" OnPageIndexChanging="gvCuContactPerson_PageIndexChanging">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="删除" ItemStyle-Width="45px">
                                <ItemTemplate>
                                    <asp:LinkButton class="easyui-linkbutton" runat="server" CommandName="Row_Delete" CommandArgument='<%# Eval("CpId") %>' OnClientClick="return deleteWarning(this);">删除</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="修改" ItemStyle-Width="45px">
                                <ItemTemplate>
                                    <a href="#" class="easyui-linkbutton" onclick='<%# "getUpdateUrl(\"" + Eval("CpId") + "\");" %>'>修改</a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CpName" HeaderText="姓名"/>
                            <asp:BoundField DataField="Mobile" HeaderText="手机"/>
                            <asp:BoundField DataField="Phone" HeaderText="电话"/>
                            <asp:TemplateField HeaderText="是否默认">
                                <ItemTemplate>
                                    <%# (bool) Eval("IsDefault") ? "是" : "否" %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>

    <div class="error">
        <asp:Label ID="lMessage" runat="server"></asp:Label>
    </div>
    <div id="dlg" class="easyui-dialog" data-options="closed:'false'"></div>
</form>
</body>
</html>