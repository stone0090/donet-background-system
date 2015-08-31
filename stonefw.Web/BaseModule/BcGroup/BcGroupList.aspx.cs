using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.BaseModule;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.Enum;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcGroup
{
    public partial class BcGroupList : BasePage
    {
        private BcGroupBiz _biz;

        private BcGroupBiz Biz
        {
            get { return _biz ?? (_biz = new BcGroupBiz()); }
        }

        protected override bool InitPermission()
        {
            btnAddNew.Visible = LoadPermission(SysPermsPointEnum.Add);
            gvUserGroup.Columns[0].Visible = LoadPermission(SysPermsPointEnum.Delete);
            gvUserGroup.Columns[1].Visible = LoadPermission(SysPermsPointEnum.Edit);
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

        protected void gvUserGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                var arg = e.CommandArgument.ToString().Split('|');
                var er = Biz.DeleteBcGroup(int.Parse(arg[0]));
                lMessage.Text = er.GetDescription();
                if (er != ExcuteResultEnum.Success) return;
                BindData();
            }
        }

        protected void gvUserGroup_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvUserGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserGroup.PageIndex = e.NewPageIndex;
        }

        private void BindData()
        {
            gvUserGroup.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvUserGroup.DataSource = Biz.GetBcGroupList();
            gvUserGroup.DataBind();
        }
    }
}