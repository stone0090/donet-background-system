using System;
using System.Web.UI.WebControls;
using stonefw.Biz.SystemModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysModule
{
    public partial class SysModuleList : BasePage
    {
        private SysModuleBiz _biz;
        private SysModuleBiz Biz
        { get { return _biz ?? (_biz = new SysModuleBiz()); } }

        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack)BindData(); }
        protected void btnQuery_Click(object sender, EventArgs e) { BindData(); this.lMessage.Text = "执行成功！"; }
        protected void gvSysModule_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                string[] arg = e.CommandArgument.ToString().Split('|');
                var er = Biz.DeleteSysModule(arg[0]);
                this.lMessage.Text = er.GetDescription();
                if (er != ExcuteResultEnum.Success) return;
                BindData();
            }
        }
        protected void gvSysModule_PageIndexChanged(object sender, EventArgs e) { BindData(); }
        protected void gvSysModule_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { this.gvSysModule.PageIndex = e.NewPageIndex; }
        private void BindData()
        {
            gvSysModule.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvSysModule.DataSource = Biz.GetSysModuleList();
            gvSysModule.DataBind();
        }
    }
}
