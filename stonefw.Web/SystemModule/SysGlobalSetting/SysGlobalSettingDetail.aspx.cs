using System;
using stonefw.Biz.SystemModule;
using stonefw.Entity.Enum;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysGlobalSetting
{
    public partial class SysGlobalSettingDetail : BasePage
    {
        #region 业务逻辑实体

        private SysGlobalSettingBiz _biz;
        private SysGlobalSettingBiz Biz { get { return _biz ?? (_biz = new SysGlobalSettingBiz()); } }

        #endregion

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
                ExcuteResultEnum er = Biz.UpdateSysSettingEntity(entity);
                this.lMessage.Text = er.GetDescription();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }

        private void BindControlData()
        {
            
        }
        private void FillFormData()
        {
            try
            {
                var entity = Biz.GetSysSettingEntity();
                this.txtSysName.Text = entity.SysName;
                this.txtSysDescription.Text = entity.SysDescription;
                this.txtErrorPage.Text = entity.ErrorPage;
                this.txtBuildingPage.Text = entity.BuildingPage;
                this.txtErrorLogPath.Text = entity.ErrorLogPath;
                this.txtSuperAdmins.Text = entity.SuperAdmins;
                this.txtGridviewPageSize.Text = entity.GridViewPageSize;
            }
            catch (Exception ex)
            {
                this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }
        private SysGlobalSettingEntity PrepareFormData()
        {
            //校验参数的合法性
            this.txtSysName.Text.InitValidation("系统名称").NotEmpty();
            this.txtSysDescription.Text.InitValidation("系统说明").NotEmpty();
            this.txtErrorPage.Text.InitValidation("错误页面").NotEmpty();
            this.txtBuildingPage.Text.InitValidation("建设页面").NotEmpty();
            this.txtSuperAdmins.Text.InitValidation("超级管理员").NotEmpty();
            this.txtErrorLogPath.Text.InitValidation("错误日志路径").NotEmpty();
            this.txtGridviewPageSize.Text.InitValidation("默认分页数").IsNum().LargerThan(5);

            var entity = new SysGlobalSettingEntity();
            entity.SysName = this.txtSysName.Text;
            entity.SysDescription = this.txtSysDescription.Text;
            entity.ErrorPage = this.txtErrorPage.Text;
            entity.BuildingPage = this.txtBuildingPage.Text;
            entity.ErrorLogPath = this.txtErrorLogPath.Text;
            entity.SuperAdmins = this.txtSuperAdmins.Text;
            entity.GridViewPageSize = this.txtGridviewPageSize.Text;
            return entity;
        }

    }
}