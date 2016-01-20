using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.Enum;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.SystemModule.SysMenu
{
    public partial class SysMenuList : BasePage
    {
        private SysMenuBiz _biz;

        private SysMenuBiz Biz
        {
            get { return _biz ?? (_biz = new SysMenuBiz()); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindData();
            lMessage.Text = "执行成功！";
        }

        protected void gvMenu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                var menuId = int.Parse(e.CommandArgument.ToString());
                var er = Biz.DeleteSysMenu(menuId);
                lMessage.Text = er.GetDescription();
                if (er != ExcuteResultEnum.Success) return;
                BindData();
            }
        }

        protected void gvMenu_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvMenu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMenu.PageIndex = e.NewPageIndex;
        }

        private void BindData()
        {
            gvMenu.DataSource = Biz.GetSysMenuList();
            gvMenu.DataBind();
        }
    }
}