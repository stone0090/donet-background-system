using System;
using System.Web.UI.WebControls;
using stonefw.Biz.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.BaseModule.BcPermission
{
    public partial class BcPermissionList : BasePage
    {
        private BcPermissionBiz _biz;
        private BcPermissionBiz Biz
        { get { return _biz ?? (_biz = new BcPermissionBiz()); } }

        public override bool InitPermission()
        {
            this.btnAddNew.Visible = LoadPermission(PermsPointEnum.Add);
            this.gvBcPermission.Columns[0].Visible = LoadPermission(PermsPointEnum.Delete);
            return LoadPermission(PermsPointEnum.View);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControlData();
                BindData();
            }
        }
        protected void btnQuery_Click(object sender, EventArgs e) { BindData(); this.lMessage.Text = "执行成功！"; }
        protected void gvBcPermission_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                string[] arg = e.CommandArgument.ToString().Split('|');
                Biz.DeleteBcPermission(int.Parse(arg[0]), int.Parse(arg[1]), arg[2], arg[3]);
                BindData();
            }
        }
        protected void gvBcPermission_PageIndexChanged(object sender, EventArgs e) { BindData(); }
        protected void gvBcPermission_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { this.gvBcPermission.PageIndex = e.NewPageIndex; }
        protected void SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindControlData()
        {
            this.ddlRole.DataSource = new BcRoleBiz().GetBcRoleList();
            this.ddlRole.DataValueField = "RoleId";
            this.ddlRole.DataTextField = "RoleName";
            this.ddlRole.DataBind();

            this.ddlUser.DataSource = new BcUserInfoBiz().GetEnabledBcUserInfoList();
            this.ddlUser.DataValueField = "UserId";
            this.ddlUser.DataTextField = "UserName";
            this.ddlUser.DataBind();
        }
        private void BindData()
        {
            gvBcPermission.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvBcPermission.DataSource = this.ddlPermissionType.SelectedValue == "1" ?
                Biz.GetEnabledBcPermissionList(1, int.Parse(this.ddlRole.SelectedValue)) :
                Biz.GetEnabledBcPermissionList(2, int.Parse(this.ddlUser.SelectedValue));
            gvBcPermission.DataBind();
        }

    }
}
