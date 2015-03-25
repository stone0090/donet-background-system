<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BcPermissionDetail.aspx.cs" Inherits="stonefw.Web.BaseModule.BcPermission.BcPermissionDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hdPermissionType" />
        <asp:HiddenField runat="server" ID="hdPermissionId" />
        <div class="form">
            <asp:GridView ID="gvBcPermission" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="gridview" OnDataBound="gvBcPermission_DataBound">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <RowStyle HorizontalAlign="Center"></RowStyle>
                <Columns>
                    <asp:BoundField DataField="ModuleName" HeaderText="模块名称" />
                    <asp:BoundField DataField="FuncPointName" HeaderText="功能点名称" />
                    <asp:TemplateField HeaderText="包含权限点">
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="hdModuleId" Value='<%# Eval("ModuleId") %>' />
                            <asp:HiddenField runat="server" ID="hdFuncPointId" Value='<%# Eval("FuncPointId") %>' />
                            <asp:HiddenField runat="server" ID="hdPermissions" Value='<%# Eval("Permissions") %>' />
                            <asp:CheckBoxList runat="server" ID="cblPermissions" RepeatDirection="Horizontal" RepeatLayout="Flow" RepeatColumns="0" CellPadding="-1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="error">
                <asp:Label ID="lMessage" runat="server"></asp:Label>
            </div>
            <div>
                <asp:LinkButton ID="btnSave" runat="server" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" OnClientClick="return saveForm(this);" OnClick="btnSave_Click">保存</asp:LinkButton>
                <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="#" onclick="window.parent.closeDialog();">取消</a>
            </div>
        </div>
    </form>
</body>
</html>
