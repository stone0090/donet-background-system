using System;
using System.Web.UI.WebControls;
using stonefw.Biz.SystemModule;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysEnumName
{
    public partial class SysEnumNameList : BasePage
    {
        private SysEnumNameBiz _biz;
        private SysEnumNameBiz Biz
        { get { return _biz ?? (_biz = new SysEnumNameBiz()); } }
        
        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack)BindData(); }
        protected void btnQuery_Click(object sender, EventArgs e) { BindData(); this.lMessage.Text = "执行成功！"; }
        protected void gvSysEnumName_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                string[] arg = e.CommandArgument.ToString().Split('|');
                Biz.DeleteSysEnumName(arg[0], arg[1]);
                BindData();
            }
        }
        protected void gvSysEnumName_PageIndexChanged(object sender, EventArgs e) { BindData(); }
        protected void gvSysEnumName_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { this.gvSysEnumName.PageIndex = e.NewPageIndex; }
        private void BindData()
        {
            gvSysEnumName.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvSysEnumName.DataSource = Biz.GetSysEnumNameList();
            gvSysEnumName.DataBind();
        }
    }
}
