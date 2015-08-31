using System;
using System.Collections.Generic;
using System.Linq;
using Stonefw.Entity.SystemModule;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.MainPage
{
    public partial class Menu : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Write(GetMenu());
            Response.End();
        }

        private string GetMenu()
        {
            try
            {
                var strMenu = string.Empty;

                var list = CurrentUserInfo.MenuList;
                var listMain = list.Where(n => n.FatherNode == 0).ToList();
                if (listMain.Count == 0)
                    return "没有定义菜单...";

                strMenu = listMain.Aggregate(strMenu,
                    (current, main) => current + GetMainMenu(main.MenuId, main.MenuName, GetSubMenu(main.MenuId, list)));
                strMenu =
                    string.Format("<div class=\"easyui-accordion\" data-options=\"fit:true,border:false\">{0}</div>",
                        strMenu);
                return strMenu;
            }
            catch (Exception)
            {
                return "菜单加载失败...";
            }
        }

        private string GetMainMenu(int? id, string title, string content)
        {
            return string.Format("<div id=\"div{0}\" title=\"{1}\" style=\"padding:0;\">{2}</div>", id, title, content);
        }

        private string GetSubMenu(int? id, List<SysMenuEntity> list)
        {
            var subMenu = GetSubMenuRecursion(id, list);
            return "<ul class=\"easyui-tree\" " +
                   "data-options=\"" +
                   "animate: true," +
                   "onClick: function(node){addNewTab(node.text,node.id)}\">" + subMenu + "</ul>";
        }

        private string GetSubMenuRecursion(int? id, List<SysMenuEntity> list)
        {
            var subMenu = string.Empty;

            var listSub = list.Where(n => n.FatherNode == id).ToList();
            if (listSub.Count > 0)
            {
                foreach (var sub in listSub)
                {
                    var subSubMenu = GetSubMenuRecursion(sub.MenuId, list);
                    if (string.IsNullOrEmpty(subSubMenu))
                        subMenu += string.Format("<li id='{0}'><span>{1}</span></li>", sub.PageUrl, sub.MenuName);
                    else
                        subMenu += string.Format("<li id='{0}'><span>{1}</span><ul>{2}</ul></li>", sub.PageUrl,
                            sub.MenuName, subSubMenu);
                }
            }
            return subMenu;
        }
    }
}