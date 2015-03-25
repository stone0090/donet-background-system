using System;
using System.Web.UI.WebControls;
using stonefw.Biz.SystemModule;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysMfpRelation
{
    public partial class SysMfpRelationList : BasePage
    {
        private SysMfpRelationBiz _biz;
        private SysMfpRelationBiz Biz
        { get { return _biz ?? (_biz = new SysMfpRelationBiz()); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControlData();
                BindData();
            }
        }
        protected void btnQuery_Click(object sender, EventArgs e) { BindData(); this.lMessage.Text = "执行成功！"; }
        protected void gvSysMfpRelation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                string[] arg = e.CommandArgument.ToString().Split('|');
                Biz.DeleteSysMfpRelation(arg[0], arg[1]);
                BindData();
            }
        }
        protected void gvSysMfpRelation_PageIndexChanged(object sender, EventArgs e) { BindData(); }
        protected void gvSysMfpRelation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { this.gvSysMfpRelation.PageIndex = e.NewPageIndex; }
        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindControlData()
        {
            this.ddlModule.DataSource = new SysModuleBiz().GetSysModuleList();
            this.ddlModule.DataValueField = "ModuleId";
            this.ddlModule.DataTextField = "ModuleName";
            this.ddlModule.DataBind();
            this.ddlModule.Items.Insert(0, new ListItem("全部", ""));
        }
        private void BindData()
        {
            gvSysMfpRelation.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvSysMfpRelation.DataSource = string.IsNullOrEmpty(this.ddlModule.SelectedValue)
                ? Biz.GetSysMfpRelationList()
                : Biz.GetSysMfpRelationList(this.ddlModule.SelectedValue);
            gvSysMfpRelation.DataBind();
        }
    }
}
