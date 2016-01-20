using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.SystemModule;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.SystemModule.SysPermsPointEnum
{
    public partial class SysPermsPointEnumList : BasePage
    {
        private SysPermsPointEnumBiz _biz;

        private SysPermsPointEnumBiz Biz
        {
            get { return _biz ?? (_biz = new SysPermsPointEnumBiz()); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindData();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindData();
            lMessage.Text = "执行成功！";
        }

        protected void gvSysPermsPointEnum_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvSysPermsPointEnum_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSysPermsPointEnum.PageIndex = e.NewPageIndex;
        }

        private void BindData()
        {
            gvSysPermsPointEnum.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvSysPermsPointEnum.DataSource = Biz.GetSysPermsPointEnumList();
            gvSysPermsPointEnum.DataBind();
        }
    }
}