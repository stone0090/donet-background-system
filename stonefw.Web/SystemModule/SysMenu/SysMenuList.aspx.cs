using System;
using System.Web.UI.WebControls;
using stonefw.Biz.SystemModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysMenu
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
            this.lMessage.Text = "执行成功！";
        }
        protected void gvMenu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                int menuId = int.Parse(e.CommandArgument.ToString());
                var er = Biz.DeleteSysMenu(menuId);
                this.lMessage.Text = er.GetName();
                if (er != ExcuteResult.Success) return;
                BindData();
            }
        }
        protected void gvMenu_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        protected void gvMenu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvMenu.PageIndex = e.NewPageIndex;
        }

        private void BindData()
        {
            gvMenu.DataSource = Biz.GetSysMenuList();
            gvMenu.DataBind();
        }
    }
}