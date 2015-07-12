using System;
using System.Web.UI.WebControls;
using stonefw.Biz.SystemModule;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysRelation
{
    public partial class SysRelationList : BasePage
    {
        private SysRelationBiz _biz;
        private SysRelationBiz Biz
        { get { return _biz ?? (_biz = new SysRelationBiz()); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControlData();
                BindData();
            }
        }
        protected void btnQuery_Click(object sender, EventArgs e) { BindData(); this.lMessage.Text = "执行成功！"; }
        protected void gvSysRelation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                string[] arg = e.CommandArgument.ToString().Split('|');
                Biz.DeleteSysRelation(arg[0], arg[1]);
                BindData();
            }
        }
        protected void gvSysRelation_PageIndexChanged(object sender, EventArgs e) { BindData(); }
        protected void gvSysRelation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { this.gvSysRelation.PageIndex = e.NewPageIndex; }
        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindControlData()
        {
            this.ddlModule.DataSource = new SysModuleEnumBiz().GetSysModuleEnumList();
            this.ddlModule.DataValueField = "Name";
            this.ddlModule.DataTextField = "Description";
            this.ddlModule.DataBind();
            this.ddlModule.Items.Insert(0, new ListItem("全部", ""));
        }
        private void BindData()
        {
            gvSysRelation.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvSysRelation.DataSource = string.IsNullOrEmpty(this.ddlModule.SelectedValue)
                ? Biz.GetSysRelationList()
                : Biz.GetSysRelationList(this.ddlModule.SelectedValue);
            gvSysRelation.DataBind();
        }
    }
}
