using System;
using System.Web.UI.WebControls;
using stonefw.Biz.SystemModule;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysRelation
{
    public partial class SysRelationDetail : BasePage
    {
        private SysRelationBiz _biz;
        private SysRelationBiz Biz { get { return _biz ?? (_biz = new SysRelationBiz()); } }
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
                SysRelationEntity entity = PrepareFormData();
                if (Request["moduleid"] == "-1" && Request["funcpointid"] == "-1")
                {
                    Biz.AddNewSysRelation(entity);
                }
                else
                {
                    Biz.UpdateSysRelation(entity);
                }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }

        private void BindControlData()
        {
            this.ddlModule.DataSource = new SysModuleEnumBiz().GetSysModuleEnumList();
            this.ddlModule.DataValueField = "Name";
            this.ddlModule.DataTextField = "Description";
            this.ddlModule.DataBind();

            //TODO
            //if (Request["moduleid"] == "-1" && Request["funcpointid"] == "-1")
            //    this.ddlFuncPoint.DataSource = new SysFuncPointBiz().GetNotUsedSysFuncPointList();
            //else
            this.ddlFuncPoint.DataSource = new SysFuncPointEnumBiz().GetSysFuncPointEnumList();
            this.ddlFuncPoint.DataValueField = "Name";
            this.ddlFuncPoint.DataTextField = "Description";
            this.ddlFuncPoint.DataBind();

            this.cblPermission.DataSource = new SysPermsPointEnumBiz().GetSysPermsPointEnumList();
            this.cblPermission.DataValueField = "Name";
            this.cblPermission.DataTextField = "Description";
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
        private SysRelationEntity PrepareFormData()
        {
            this.ddlModule.SelectedValue.InitValidation("模块编号").NotEmpty();
            this.ddlFuncPoint.SelectedValue.InitValidation("功能点编号").NotEmpty();

            var entity = new SysRelationEntity();
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
