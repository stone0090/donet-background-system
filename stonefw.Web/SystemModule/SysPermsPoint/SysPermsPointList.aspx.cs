using System;
using System.Web.UI.WebControls;
using stonefw.Biz.SystemModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysPermsPoint
{
    public partial class SysPermsPointList : BasePage
    {
        private SysPermsPointBiz _biz;
        private SysPermsPointBiz Biz
        { get { return _biz ?? (_biz = new SysPermsPointBiz()); } }

        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack)BindData(); }
        protected void btnQuery_Click(object sender, EventArgs e) { BindData(); this.lMessage.Text = "执行成功！"; }
        protected void gvSysPermsPoint_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                string[] arg = e.CommandArgument.ToString().Split('|');
                var er = Biz.DeleteSysPermsPoint(arg[0]);
                this.lMessage.Text = er.GetDescription();
                if (er != ExcuteResultEnum.Success) return;
                BindData();
            }
        }
        protected void gvSysPermsPoint_PageIndexChanged(object sender, EventArgs e) { BindData(); }
        protected void gvSysPermsPoint_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { this.gvSysPermsPoint.PageIndex = e.NewPageIndex; }
        private void BindData()
        {
            gvSysPermsPoint.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvSysPermsPoint.DataSource = Biz.GetSysPermsPointList();
            gvSysPermsPoint.DataBind();
        }
    }
}
