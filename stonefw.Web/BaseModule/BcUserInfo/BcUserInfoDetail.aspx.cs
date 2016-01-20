using System;
using System.Linq;
using Stonefw.Biz.BaseModule;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Utility;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcUserInfo
{
    public partial class BcUserInfoDetail : BasePage
    {
        private BcUserInfoBiz _biz;

        private BcUserInfoBiz Biz
        {
            get { return _biz ?? (_biz = new BcUserInfoBiz()); }
        }

        protected override bool InitPermission()
        {
            return LoadPermission(SysPermsPointEnum.Add) || LoadPermission(SysPermsPointEnum.Edit);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControlData();
                FillFormData();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = PrepareFormData();
                var er = hdUserId.Value == "-1"
                    ? Biz.AddNewBcUserInfo(entity, hdRoleIds.Value)
                    : Biz.UpdateBcUserInfo(entity, hdRoleIds.Value);
                if (er != ExcuteResultEnum.Success)
                {
                    lMessage.Text = er.GetDescription();
                    return;
                }
                FatherQuery();
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message);
            }
        }

        private void BindControlData()
        {
            ddlRole.DataSource = new BcRoleBiz().GetBcRoleList();
            ddlRole.DataValueField = "RoleId";
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataBind();


            ddlGroup.DataSource = new BcGroupBiz().GetBcGroupList();
            ddlGroup.DataValueField = "GroupId";
            ddlGroup.DataTextField = "GroupName";
            ddlGroup.DataBind();
        }

        private void FillFormData()
        {
            try
            {
                hdUserId.Value = Request["id"];
                var entity = Biz.GetSingleBcUserInfo(int.Parse(hdUserId.Value));
                if (entity != null)
                {
                    hdUserId.Value = entity.UserId.ToString();
                    txtUserAccount.Text = entity.UserAccount;
                    txtPassword.Text = entity.Password;
                    txtUserName.Text = entity.UserName;
                    rMale.Checked = (bool) entity.Sex;
                    rFemale.Checked = !(bool) entity.Sex;
                    txtOfficePhone.Text = entity.OfficePhone;
                    txtMobilePhone.Text = entity.MobilePhone;
                    txtEmail.Text = entity.Email;
                    rEnable.Checked = (bool) entity.ActivityFlag;
                    rDisable.Checked = !(bool) entity.ActivityFlag;
                    ddlGroup.SelectedValue = entity.GroupId.ToString();
                    txtUserAccount.Enabled = false;
                    var list = new BcUserRoleBiz().GetBcUserRoleList(int.Parse(hdUserId.Value));
                    if (list != null && list.Count > 0)
                    {
                        hdRoleIds.Value = string.Join(",", list.Select(n => n.RoleId.ToString()).ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }

        private BcUserInfoEntity PrepareFormData()
        {
            //校验参数的合法性
            txtUserAccount.Text.InitValidation("用户名").NotEmpty().ShorterThan(25);
            txtPassword.Text.InitValidation("密码").NotEmpty().LongerThan(3).ShorterThan(15);
            txtUserName.Text.InitValidation("姓名").NotEmpty().ShorterThan(25);
            txtOfficePhone.Text.InitValidation("座机").ShorterThan(25);
            txtMobilePhone.Text.InitValidation("手机").ShorterThan(25);
            txtEmail.Text.InitValidation("邮件").ShorterThan(50);
            ddlGroup.SelectedValue.InitValidation("组别").NotEmpty();

            var entity = new BcUserInfoEntity();
            entity.UserId = int.Parse(hdUserId.Value);
            entity.UserAccount = txtUserAccount.Text;
            entity.Password = txtPassword.Text;
            entity.UserName = txtUserName.Text;
            entity.Sex = rMale.Checked;
            entity.OfficePhone = txtOfficePhone.Text;
            entity.MobilePhone = txtMobilePhone.Text;
            entity.Email = txtEmail.Text;
            entity.ActivityFlag = rEnable.Checked;
            entity.GroupId = int.Parse(ddlGroup.SelectedValue);
            return entity;
        }
    }
}