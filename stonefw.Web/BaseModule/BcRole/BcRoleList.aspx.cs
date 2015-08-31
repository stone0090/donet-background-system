using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.BaseModule;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.Enum;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcRole
{
    public partial class BcRoleList : BasePage
    {
        private BcRoleBiz _biz;

        private BcRoleBiz Biz
        {
            get { return _biz ?? (_biz = new BcRoleBiz()); }
        }

        protected override bool InitPermission()
        {
            btnAddNew.Visible = LoadPermission(SysPermsPointEnum.Add);
            gvRole.Columns[0].Visible = LoadPermission(SysPermsPointEnum.Delete);
            gvRole.Columns[1].Visible = LoadPermission(SysPermsPointEnum.Edit);
            return LoadPermission(SysPermsPointEnum.View);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindData();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindData();
            lMessage.Text = "ִ�гɹ���";
        }

        protected void gvRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                var arg = e.CommandArgument.ToString().Split('|');
                var er = Biz.DeleteBcRole(int.Parse(arg[0]));
                lMessage.Text = er.GetDescription();
                if (er != ExcuteResultEnum.Success) return;
                BindData();
            }
        }

        protected void gvRole_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRole.PageIndex = e.NewPageIndex;
        }

        private void BindData()
        {
            gvRole.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvRole.DataSource = Biz.GetBcRoleList();
            gvRole.DataBind();
        }
    }
}