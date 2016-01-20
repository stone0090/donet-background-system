using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.SystemModule;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.SystemModule.SysModuleEnum
{
    public partial class SysModuleEnumList : BasePage
    {
        private SysModuleEnumBiz _biz;

        private SysModuleEnumBiz Biz
        {
            get { return _biz ?? (_biz = new SysModuleEnumBiz()); }
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

        protected void gvSysModuleEnum_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvSysModuleEnum_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSysModuleEnum.PageIndex = e.NewPageIndex;
        }

        private void BindData()
        {
            gvSysModuleEnum.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvSysModuleEnum.DataSource = Biz.GetSysModuleEnumList();
            gvSysModuleEnum.DataBind();
        }
    }
}