using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcUserRole
{
    public partial class BcUserRoleList : BasePage
    {
        private BcUserRoleBiz _biz;

        private BcUserRoleBiz Biz
        {
            get { return _biz ?? (_biz = new BcUserRoleBiz()); }
        }

        protected override bool InitPermission()
        {
            btnAddNew.Visible = LoadPermission(SysPermsPointEnum.Add);
            gvBcUserRole.Columns[0].Visible = LoadPermission(SysPermsPointEnum.Delete);
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

        protected void gvBcUserRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                var arg = e.CommandArgument.ToString().Split('|');
                Biz.DeleteBcUserRole(int.Parse(arg[0]), int.Parse(arg[1]));
                BindData();
            }
        }

        protected void gvBcUserRole_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvBcUserRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBcUserRole.PageIndex = e.NewPageIndex;
        }

        private void BindControlData()
        {
            ddlUser.DataSource = new BcUserInfoBiz().GetEnabledBcUserInfoList();
            ddlUser.DataValueField = "UserId";
            ddlUser.DataTextField = "UserName";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("*全部*", "0"));

            ddlRole.DataSource = new BcRoleBiz().GetBcRoleList();
            ddlRole.DataValueField = "RoleId";
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataBind();
            ddlRole.Items.Insert(0, new ListItem("*全部*", "0"));
        }

        private void BindData()
        {
            gvBcUserRole.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvBcUserRole.DataSource = Biz.GetBcUserRoleList(int.Parse(ddlUser.SelectedValue),
                int.Parse(ddlRole.SelectedValue));
            gvBcUserRole.DataBind();
        }
    }
}