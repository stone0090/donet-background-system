using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcPermission
{
    public partial class BcPermissionList : BasePage
    {
        private BcPermissionBiz _biz;

        private BcPermissionBiz Biz
        {
            get { return _biz ?? (_biz = new BcPermissionBiz()); }
        }

        protected override bool InitPermission()
        {
            btnAddNew.Visible = LoadPermission(SysPermsPointEnum.Add);
            gvBcPermission.Columns[0].Visible = LoadPermission(SysPermsPointEnum.Delete);
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

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindData();
            lMessage.Text = "执行成功！";
        }

        protected void gvBcPermission_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                var arg = e.CommandArgument.ToString().Split('|');
                Biz.DeleteBcPermission(int.Parse(arg[0]), int.Parse(arg[1]), arg[2], arg[3]);
                BindData();
            }
        }

        protected void gvBcPermission_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvBcPermission_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBcPermission.PageIndex = e.NewPageIndex;
        }

        protected void SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindControlData()
        {
            ddlRole.DataSource = new BcRoleBiz().GetBcRoleList();
            ddlRole.DataValueField = "RoleId";
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataBind();
            ddlRole.Items.Insert(0, new ListItem("*全部*", "0"));

            ddlUser.DataSource = new BcUserInfoBiz().GetEnabledBcUserInfoList();
            ddlUser.DataValueField = "UserId";
            ddlUser.DataTextField = "UserName";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("*全部*", "0"));
        }

        private void BindData()
        {
            gvBcPermission.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvBcPermission.DataSource = ddlPermissionType.SelectedValue == "1"
                ? Biz.GetEnabledBcPermissionList(1, int.Parse(ddlRole.SelectedValue))
                : Biz.GetEnabledBcPermissionList(2, int.Parse(ddlUser.SelectedValue));
            gvBcPermission.DataBind();

            ddlRole.Visible = ddlPermissionType.SelectedValue == "1";
            ddlUser.Visible = ddlPermissionType.SelectedValue == "2";

            if (ddlPermissionType.SelectedValue == "2")
                gvBcPermission.Columns[1].HeaderText = "角色名称";

            if ((ddlPermissionType.SelectedValue == "1" && ddlRole.SelectedValue == "0") ||
                (ddlPermissionType.SelectedValue == "2" && ddlUser.SelectedValue == "0"))
                btnAddNew.Visible = false;
            else
                btnAddNew.Visible = true;
        }
    }
}