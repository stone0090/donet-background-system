using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.SystemModule;
using Stonefw.Utility;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.SystemModule.SysRelation
{
    public partial class SysRelationDetail : BasePage
    {
        private SysRelationBiz _biz;

        private SysRelationBiz Biz
        {
            get { return _biz ?? (_biz = new SysRelationBiz()); }
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
                if (Request["moduleid"] == "-1" && Request["funcpointid"] == "-1")
                {
                    Biz.AddNewSysRelation(entity);
                }
                else
                {
                    Biz.UpdateSysRelation(entity);
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
            ddlModule.DataSource = new SysModuleEnumBiz().GetSysModuleEnumList();
            ddlModule.DataValueField = "Name";
            ddlModule.DataTextField = "Description";
            ddlModule.DataBind();

            //TODO
            //if (Request["moduleid"] == "-1" && Request["funcpointid"] == "-1")
            //    this.ddlFuncPoint.DataSource = new SysFuncPointBiz().GetNotUsedSysFuncPointList();
            //else
            ddlFuncPoint.DataSource = new SysFuncPointEnumBiz().GetSysFuncPointEnumList();
            ddlFuncPoint.DataValueField = "Name";
            ddlFuncPoint.DataTextField = "Description";
            ddlFuncPoint.DataBind();

            cblPermission.DataSource = new SysPermsPointEnumBiz().GetSysPermsPointEnumList();
            cblPermission.DataValueField = "Name";
            cblPermission.DataTextField = "Description";
            cblPermission.DataBind();
        }

        private void FillFormData()
        {
            try
            {
                if (Request["moduleid"] == "-1" && Request["funcpointid"] == "-1")
                    return;

                ddlModule.Enabled = false;
                ddlFuncPoint.Enabled = false;
                ddlModule.SelectedValue = Request["moduleid"];
                ddlFuncPoint.SelectedValue = Request["funcpointid"];

                var pcs = Request["permissions"].Split(',');
                foreach (ListItem li in cblPermission.Items)
                {
                    foreach (var pc in pcs)
                    {
                        if (li.Value == pc)
                        {
                            li.Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }

        private SysRelationEntity PrepareFormData()
        {
            ddlModule.SelectedValue.InitValidation("模块编号").NotEmpty();
            ddlFuncPoint.SelectedValue.InitValidation("功能点编号").NotEmpty();

            var entity = new SysRelationEntity();
            entity.ModuleId = ddlModule.SelectedValue;
            entity.FuncPointId = ddlFuncPoint.SelectedValue;
            foreach (ListItem li in cblPermission.Items)
            {
                if (li.Selected) entity.Permissions += li.Value + ",";
            }
            if (string.IsNullOrEmpty(entity.Permissions)) throw new ArgumentException("至少选择一个权限点！");
            entity.Permissions = entity.Permissions.Remove(entity.Permissions.Length - 1);
            return entity;
        }
    }
}