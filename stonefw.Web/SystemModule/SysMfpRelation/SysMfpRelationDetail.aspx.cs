using System;
using System.Web.UI.WebControls;
using stonefw.Biz.SystemModule;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysMfpRelation
{
    public partial class SysMfpRelationDetail : BasePage
    {
        private SysMfpRelationBiz _biz;
        private SysMfpRelationBiz Biz { get { return _biz ?? (_biz = new SysMfpRelationBiz()); } }
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
                SysMfpRelationEntity entity = PrepareFormData();
                if (Request["moduleid"] == "-1" && Request["funcpointid"] == "-1")
                {
                    Biz.AddNewSysMfpRelation(entity);
                }
                else
                {
                    Biz.UpdateSysMfpRelation(entity);
                }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }

        private void BindControlData()
        {
            this.ddlModule.DataSource = new SysModuleBiz().GetSysModuleList();
            this.ddlModule.DataValueField = "ModuleId";
            this.ddlModule.DataTextField = "ModuleName";
            this.ddlModule.DataBind();

            if (Request["moduleid"] == "-1" && Request["funcpointid"] == "-1")
                this.ddlFuncPoint.DataSource = new SysFuncPointBiz().GetNotUsedSysFuncPointList();
            else
                this.ddlFuncPoint.DataSource = new SysFuncPointBiz().GetSysFuncPointList();
            this.ddlFuncPoint.DataValueField = "FuncPointId";
            this.ddlFuncPoint.DataTextField = "FuncPointName";
            this.ddlFuncPoint.DataBind();

            this.cblPermission.DataSource = new SysPermsPointBiz().GetSysPermsPointList();
            this.cblPermission.DataValueField = "PermsPointId";
            this.cblPermission.DataTextField = "PermsPointName";
            this.cblPermission.DataBind();
        }
        private void FillFormData()
        {
            try
            {
                if (Request["moduleid"] == "-1" && Request["funcpointid"] == "-1")
                    return;

                this.ddlModule.Enabled = false;
                this.ddlFuncPoint.Enabled = false;
                this.ddlModule.SelectedValue = Request["moduleid"];
                this.ddlFuncPoint.SelectedValue = Request["funcpointid"];

                var pcs = Request["permissions"].Split(',');
                foreach (ListItem li in this.cblPermission.Items)
                {
                    foreach (string pc in pcs)
                    {
                        if (li.Value == pc)
                        {
                            li.Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private SysMfpRelationEntity PrepareFormData()
        {
            this.ddlModule.SelectedValue.InitValidation("模块编号").NotEmpty();
            this.ddlFuncPoint.SelectedValue.InitValidation("功能点编号").NotEmpty();

            var entity = new SysMfpRelationEntity();
            entity.ModuleId = this.ddlModule.SelectedValue;
            entity.FuncPointId = this.ddlFuncPoint.SelectedValue;
            foreach (ListItem li in this.cblPermission.Items)
            { if (li.Selected) entity.Permissions += li.Value + ","; }
            if (string.IsNullOrEmpty(entity.Permissions)) throw new ArgumentException("至少选择一个权限点！");
            else entity.Permissions = entity.Permissions.Remove(entity.Permissions.Length - 1);
            return entity;
        }
    }
}
