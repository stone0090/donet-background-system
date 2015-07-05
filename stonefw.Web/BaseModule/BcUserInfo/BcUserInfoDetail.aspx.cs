using System;
using System.Linq;
using stonefw.Biz.BaseModule;
using stonefw.Biz.SystemModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Utility;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.BaseModule.BcUserInfo
{
    public partial class BcUserInfoDetail : BasePage
    {
        private BcUserInfoBiz _biz;
        private BcUserInfoBiz Biz { get { return _biz ?? (_biz = new BcUserInfoBiz()); } }

        public override bool InitPermission()
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
                BcUserInfoEntity entity = PrepareFormData();
                ExcuteResultEnum er = this.hdUserId.Value == "-1"
                    ? Biz.AddNewBcUserInfo(entity, this.hdRoleIds.Value)
                    : Biz.UpdateBcUserInfo(entity, this.hdRoleIds.Value);
                if (er != ExcuteResultEnum.Success)
                {
                    this.lMessage.Text = er.GetDescription();
                    return;
                }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }
        private void BindControlData()
        {
            this.ddlRole.DataSource = new BcRoleBiz().GetBcRoleList();
            this.ddlRole.DataValueField = "RoleId";
            this.ddlRole.DataTextField = "RoleName";
            this.ddlRole.DataBind();


            this.ddlGroup.DataSource = new BcGroupBiz().GetBcGroupList();
            this.ddlGroup.DataValueField = "GroupId";
            this.ddlGroup.DataTextField = "GroupName";
            this.ddlGroup.DataBind();
        }
        private void FillFormData()
        {
            try
            {
                this.hdUserId.Value = Request["id"];
                BcUserInfoEntity entity = Biz.GetSingleBcUserInfo(int.Parse(this.hdUserId.Value));
                if (entity != null)
                {
                    this.hdUserId.Value = entity.UserId.ToString();
                    this.txtUserAccount.Text = entity.UserAccount.ToString();
                    this.txtPassword.Text = entity.Password.ToString();
                    this.txtUserName.Text = entity.UserName.ToString();
                    this.rMale.Checked = (bool)entity.Sex;
                    this.rFemale.Checked = !(bool)entity.Sex;
                    this.txtOfficePhone.Text = entity.OfficePhone.ToString();
                    this.txtMobilePhone.Text = entity.MobilePhone.ToString();
                    this.txtEmail.Text = entity.Email.ToString();
                    this.rEnable.Checked = (bool)entity.ActivityFlag;
                    this.rDisable.Checked = !(bool)entity.ActivityFlag;
                    this.ddlGroup.SelectedValue = entity.GroupId.ToString();
                    this.txtUserAccount.Enabled = false;
                    var list = new BcUserRoleBiz().GetBcUserRoleList(int.Parse(this.hdUserId.Value));
                    if (list != null && list.Count > 0)
                    {
                        this.hdRoleIds.Value = string.Join(",", list.Select(n => n.RoleId.ToString()).ToArray());
                    }
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private BcUserInfoEntity PrepareFormData()
        {
            //校验参数的合法性
            this.txtUserAccount.Text.InitValidation("用户名").NotEmpty().ShorterThan(25);
            this.txtPassword.Text.InitValidation("密码").NotEmpty().LongerThan(3).ShorterThan(15);
            this.txtUserName.Text.InitValidation("姓名").NotEmpty().ShorterThan(25);
            this.txtOfficePhone.Text.InitValidation("座机").ShorterThan(25);
            this.txtMobilePhone.Text.InitValidation("手机").ShorterThan(25);
            this.txtEmail.Text.InitValidation("邮件").ShorterThan(50);
            this.ddlGroup.SelectedValue.InitValidation("组别").NotEmpty();

            var entity = new BcUserInfoEntity();
            entity.UserId = int.Parse(this.hdUserId.Value);
            entity.UserAccount = this.txtUserAccount.Text;
            entity.Password = this.txtPassword.Text;
            entity.UserName = this.txtUserName.Text;
            entity.Sex = this.rMale.Checked;
            entity.OfficePhone = this.txtOfficePhone.Text;
            entity.MobilePhone = this.txtMobilePhone.Text;
            entity.Email = this.txtEmail.Text;
            entity.ActivityFlag = this.rEnable.Checked;
            entity.GroupId = int.Parse(this.ddlGroup.SelectedValue);
            return entity;
        }
    }
}
