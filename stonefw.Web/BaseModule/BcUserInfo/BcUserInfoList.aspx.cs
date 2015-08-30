using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.BaseModule;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.Enum;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcUserInfo
{
    public partial class BcUserInfoList : BasePage
    {
        private BcUserInfoBiz _biz;

        private BcUserInfoBiz Biz
        {
            get { return _biz ?? (_biz = new BcUserInfoBiz()); }
        }

        protected override bool InitPermission()
        {
            btnAddNew.Visible = LoadPermission(SysPermsPointEnum.Add);
            gvUserInfo.Columns[0].Visible = LoadPermission(SysPermsPointEnum.Delete);
            gvUserInfo.Columns[1].Visible = LoadPermission(SysPermsPointEnum.Edit);
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
            lMessage.Text = "ִ�гɹ���";
        }

        protected void gvUserInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                var arg = e.CommandArgument.ToString().Split('|');
                var er = Biz.DeleteBcUserInfo(int.Parse(arg[0]));
                lMessage.Text = er.GetDescription();
                if (er != ExcuteResultEnum.Success) return;
                BindData();
            }
        }

        protected void gvUserInfo_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvUserInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserInfo.PageIndex = e.NewPageIndex;
        }

        private void BindControlData()
        {
            ddlGroup.DataSource = new BcGroupBiz().GetBcGroupList();
            ddlGroup.DataValueField = "GroupId";
            ddlGroup.DataTextField = "GroupName";
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new ListItem("*全部*", "0"));
        }

        private void BindData()
        {
            gvUserInfo.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvUserInfo.DataSource = Biz.GetBcUserInfoList(
                int.Parse(ddlGroup.SelectedValue),
                0,
                txtUserName.Text.Trim());
            gvUserInfo.DataBind();
        }
    }
}