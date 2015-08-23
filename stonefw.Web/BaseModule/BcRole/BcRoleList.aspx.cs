using System;
using System.Web.UI.WebControls;
using stonefw.Biz.BaseModule;
using stonefw.Biz.SystemModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.BaseModule.BcRole
{
    public partial class BcRoleList : BasePage
    {
        private BcRoleBiz _biz;
        private BcRoleBiz Biz
        { get { return _biz ?? (_biz = new BcRoleBiz()); } }

        protected override bool InitPermission()
        {
            this.btnAddNew.Visible = LoadPermission(SysPermsPointEnum.Add);
            this.gvRole.Columns[0].Visible = LoadPermission(SysPermsPointEnum.Delete);
            this.gvRole.Columns[1].Visible = LoadPermission(SysPermsPointEnum.Edit);
            return LoadPermission(SysPermsPointEnum.View);
        }

        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack)BindData(); }
        protected void btnQuery_Click(object sender, EventArgs e) { BindData(); this.lMessage.Text = "Ö´ÐÐ³É¹¦£¡"; }
        protected void gvRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                string[] arg = e.CommandArgument.ToString().Split('|');
                var er = Biz.DeleteBcRole(int.Parse(arg[0]));
                this.lMessage.Text = er.GetDescription();
                if (er != ExcuteResultEnum.Success) return;
                BindData();
            }
        }
        protected void gvRole_PageIndexChanged(object sender, EventArgs e) { BindData(); }
        protected void gvRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { this.gvRole.PageIndex = e.NewPageIndex; }
        private void BindData()
        {
            gvRole.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvRole.DataSource = Biz.GetBcRoleList();
            gvRole.DataBind();
        }
    }
}
