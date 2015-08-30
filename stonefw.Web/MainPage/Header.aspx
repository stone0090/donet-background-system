<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Header.aspx.cs" Inherits="Stonefw.Web.MainPage.Header" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header1">
            <div class="header2">
                <div class="logo">
                    <strong><%= SysGlobalSetting.SysName %></strong>
                </div>
                <div class="contact">
                    <div class="prompt">
                        <span><%= Today %></span>
                    </div>
                    <div class="headerMenu">
                        <ul>
                            <li>
                                <asp:LinkButton class="easyui-linkbutton" data-options="plain:true" ID="btnLogout" runat="server" OnClick="btnLogout_Click" OnClientClick="if (confirm('您是否要退出系统？')) {return true; } else {$(this).unbind('click');return false;}">安全退出</asp:LinkButton>
                            </li>
                            <li runat="server" id="liSysManage">
                                <a href="#" class="easyui-menubutton" data-options="menu:'#mm2'">系统维护</a>
                            </li>
                            <!--<li><a href="#" class="easyui-linkbutton" data-options="plain:true">我的代办</a></li>-->
                            <li>
                                <a href="#" class="easyui-linkbutton" data-options="plain:true">您好，<%= CurrentUserInfo.UserName %></a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div id="mm1" style="width: 100px;">
            <div onclick="addNewTab('个人设置', '');">个人设置</div>
            <div onclick="addNewTab('个人菜单', '');">个人菜单</div>
        </div>
        <div id="mm2" style="width: 100px;">
            <div onclick="addNewTab('菜单管理', '/SystemModule/SysMenu/SysMenuList.aspx');">菜单管理</div>
            <div onclick="addNewTab('全局设置', '/SystemModule/SysGlobalSetting/SysGlobalSettingDetail.aspx');">全局设置</div>
            <div class="menu-sep"></div>
            <div onclick="addNewTab('模块点枚举', '/SystemModule/SysModuleEnum/SysModuleEnumList.aspx');">模块点枚举</div>
            <div onclick="addNewTab('功能点枚举', '/SystemModule/SysFuncPointEnum/SysFuncPointEnumList.aspx');">功能点枚举</div>
            <div onclick="addNewTab('权限点枚举', '/SystemModule/SysPermsPointEnum/SysPermsPointEnumList.aspx');">权限点枚举</div>
            <div class="menu-sep"></div>
            <div onclick="addNewTab('枚举名称映射', '/SystemModule/SysEnumName/SysEnumNameList.aspx');">枚举名称映射</div>
            <div onclick="addNewTab('权限关系映射', '/SystemModule/SysRelation/SysRelationList.aspx');">权限关系映射</div>
            <div onclick="addNewTab('页面功能映射', '/SystemModule/SysPageFuncPoint/SysPageFuncPointList.aspx');">页面功能映射</div>
        </div>
        <script src="/Resource/js/common.js"></script>
    </form>
</body>
</html>
