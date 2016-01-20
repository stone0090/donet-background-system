using System;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.SystemModule;
using Stonefw.Utility;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.SystemModule.SysGlobalSetting
{
    public partial class SysGlobalSettingDetail : BasePage
    {
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
                var er = Biz.UpdateSysSettingEntity(entity);
                lMessage.Text = er.GetDescription();
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message);
            }
        }

        private void BindControlData()
        {
        }

        private void FillFormData()
        {
            try
            {
                var entity = Biz.GetSysSettingEntity();
                txtSysName.Text = entity.SysName;
                txtSysDescription.Text = entity.SysDescription;
                txtErrorPage.Text = entity.ErrorPage;
                txtBuildingPage.Text = entity.BuildingPage;
                txtErrorLogPath.Text = entity.ErrorLogPath;
                txtSuperAdmins.Text = entity.SuperAdmins;
                txtGridviewPageSize.Text = entity.GridViewPageSize;
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }

        private SysGlobalSettingEntity PrepareFormData()
        {
            //校验参数的合法性
            txtSysName.Text.InitValidation("系统名称").NotEmpty();
            txtSysDescription.Text.InitValidation("系统说明").NotEmpty();
            txtErrorPage.Text.InitValidation("错误页面").NotEmpty();
            txtBuildingPage.Text.InitValidation("建设页面").NotEmpty();
            txtSuperAdmins.Text.InitValidation("超级管理员").NotEmpty();
            txtErrorLogPath.Text.InitValidation("错误日志路径").NotEmpty();
            txtGridviewPageSize.Text.InitValidation("默认分页数").IsNum().LargerThan(5);

            var entity = new SysGlobalSettingEntity();
            entity.SysName = txtSysName.Text;
            entity.SysDescription = txtSysDescription.Text;
            entity.ErrorPage = txtErrorPage.Text;
            entity.BuildingPage = txtBuildingPage.Text;
            entity.ErrorLogPath = txtErrorLogPath.Text;
            entity.SuperAdmins = txtSuperAdmins.Text;
            entity.GridViewPageSize = txtGridviewPageSize.Text;
            return entity;
        }

        #region 业务逻辑实体

        private SysGlobalSettingBiz _biz;

        private SysGlobalSettingBiz Biz
        {
            get { return _biz ?? (_biz = new SysGlobalSettingBiz()); }
        }

        #endregion
    }
}