using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using stonefw.Biz.BaseModule;
using stonefw.Biz.SystemModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.BaseModule.BcPermission
{
    public partial class BcPermissionDetail : BasePage
    {
        private BcPermissionBiz _biz;
        private BcPermissionBiz Biz { get { return _biz ?? (_biz = new BcPermissionBiz()); } }

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
                List<BcPermissionEntity> list = PrepareFormData();
                if (list.Count > 0)
                {
                    Biz.DeleteBcPermission(list[0].UserRoleId, list[0].PermissionType);
                    Biz.AddNewBcPermission(list);
                }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }
        protected void gvBcPermission_DataBound(object sender, EventArgs e)
        {
            foreach (TableRow row in this.gvBcPermission.Rows)
            {
                var hdPermissions = (HiddenField)row.Cells[2].FindControl("hdPermissions");
                var cblPermissions = (CheckBoxList)row.Cells[2].FindControl("cblPermissions");
                string[] permissionList = hdPermissions.Value.Split(',');
                cblPermissions.DataSource = permissionList;
                cblPermissions.DataBind();
            }
        }

        private void BindControlData()
        {
            this.gvBcPermission.DataSource = new SysRelationBiz().GetEnabledSysRelationList();
            this.gvBcPermission.DataBind();
        }
        private void FillFormData()
        {
            try
            {
                if (Request["permissionid"] == "-1" && Request["permissiontype"] == "-1")
                    return;

                this.hdPermissionId.Value = Request["permissionid"];
                this.hdPermissionType.Value = Request["permissiontype"];

                var list = Biz.GetEnabledBcPermissionList(int.Parse(this.hdPermissionType.Value), int.Parse(this.hdPermissionId.Value));
                if (list != null && list.Count > 0)
                {
                    foreach (TableRow row in this.gvBcPermission.Rows)
                    {
                        var hdModuleId = (HiddenField)row.Cells[2].FindControl("hdModuleId");
                        var hdFuncPointId = (HiddenField)row.Cells[2].FindControl("hdFuncPointId");
                        var cblPermissions = (CheckBoxList)row.Cells[2].FindControl("cblPermissions");
                        var result = list.Where(n => n.UserRoleId == int.Parse(Request["permissionid"]) &&
                                        n.PermissionType == int.Parse(Request["permissiontype"]) &&
                                        n.ModuleId == hdModuleId.Value &&
                                        n.FuncPointId == hdFuncPointId.Value).ToList();
                        if (result.Count > 0)
                        {
                            var pcs = result[0].Permissions.Split(',');
                            foreach (ListItem li in cblPermissions.Items)
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
                    }
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private List<BcPermissionEntity> PrepareFormData()
        {
            var list = new List<BcPermissionEntity>();
            foreach (TableRow row in this.gvBcPermission.Rows)
            {
                var hdModuleId = (HiddenField)row.Cells[0].FindControl("hdModuleId");
                var hdFuncPointId = (HiddenField)row.Cells[1].FindControl("hdFuncPointId");
                var cblPermissions = (CheckBoxList)row.Cells[2].FindControl("cblPermissions");
                var entity = new BcPermissionEntity();
                entity.PermissionType = int.Parse(this.hdPermissionType.Value);
                entity.UserRoleId = int.Parse(this.hdPermissionId.Value);
                entity.ModuleId = hdModuleId.Value;
                entity.FuncPointId = hdFuncPointId.Value;
                foreach (ListItem li in cblPermissions.Items)
                {
                    if (li.Selected)
                        entity.Permissions += li.Value + ",";
                }
                entity.Permissions = string.IsNullOrEmpty(entity.Permissions) ?
                    string.Empty :
                    entity.Permissions.Remove(entity.Permissions.Length - 1);
                list.Add(entity);
            }
            return list;
        }
    }
}
