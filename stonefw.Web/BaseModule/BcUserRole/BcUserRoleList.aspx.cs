using System;
using System.Web.UI.WebControls;
using stonefw.Biz.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.BaseModule.BcUserRole
{
    public partial class BcUserRoleList : BasePage
    {
        private BcUserRoleBiz _biz;
        private BcUserRoleBiz Biz
        { get { return _biz ?? (_biz = new BcUserRoleBiz()); } }

        public override bool InitPermission()
        {
            this.btnAddNew.Visible = LoadPermission(SysPermsPointEnum.Add);
            this.gvBcUserRole.Columns[0].Visible = LoadPermission(SysPermsPointEnum.Delete);
            return LoadPermission(SysPermsPointEnum.View);
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
        protected void gvBcUserRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                string[] arg = e.CommandArgument.ToString().Split('|');
                Biz.DeleteBcUserRole(int.Parse(arg[0]), int.Parse(arg[1]));
                BindData();
            }
        }
        protected void gvBcUserRole_PageIndexChanged(object sender, EventArgs e) { BindData(); }
        protected void gvBcUserRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { this.gvBcUserRole.PageIndex = e.NewPageIndex; }

        private void BindControlData()
        {
            this.ddlUser.DataSource = new BcUserInfoBiz().GetEnabledBcUserInfoList();
            this.ddlUser.DataValueField = "UserId";
            this.ddlUser.DataTextField = "UserName";
            this.ddlUser.DataBind();
            this.ddlUser.Items.Insert(0, new ListItem("*全部*", "0"));

            this.ddlRole.DataSource = new BcRoleBiz().GetBcRoleList();
            this.ddlRole.DataValueField = "RoleId";
            this.ddlRole.DataTextField = "RoleName";
            this.ddlRole.DataBind();
            this.ddlRole.Items.Insert(0, new ListItem("*全部*", "0"));
        }
        private void BindData()
        {
            gvBcUserRole.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvBcUserRole.DataSource = Biz.GetBcUserRoleList(int.Parse(this.ddlUser.SelectedValue), int.Parse(this.ddlRole.SelectedValue));
            gvBcUserRole.DataBind();
        }


    }
}
