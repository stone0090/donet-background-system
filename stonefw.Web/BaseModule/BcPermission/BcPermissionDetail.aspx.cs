using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Stonefw.Biz.BaseModule;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcPermission
{
    public partial class BcPermissionDetail : BasePage
    {
        private BcPermissionBiz _biz;

        private BcPermissionBiz Biz
        {
            get { return _biz ?? (_biz = new BcPermissionBiz()); }
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
                var list = PrepareFormData();
                if (list.Count > 0)
                {
                    Biz.DeleteBcPermission(list[0].UserRoleId, list[0].PermissionType);
                    Biz.AddNewBcPermission(list);
                }
                FatherQuery();
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message);
            }
        }

        protected void gvBcPermission_DataBound(object sender, EventArgs e)
        {
            foreach (TableRow row in gvBcPermission.Rows)
            {
                var hdPermissions = (HiddenField) row.Cells[2].FindControl("hdPermissions");
                var cblPermissions = (CheckBoxList) row.Cells[2].FindControl("cblPermissions");
                var permissionList = hdPermissions.Value.Split(',');
                cblPermissions.DataSource = permissionList;
                cblPermissions.DataBind();
            }
        }

        private void BindControlData()
        {
            gvBcPermission.DataSource = new SysRelationBiz().GetEnabledSysRelationList();
            gvBcPermission.DataBind();
        }

        private void FillFormData()
        {
            try
            {
                if (Request["permissionid"] == "-1" && Request["permissiontype"] == "-1")
                    return;

                hdPermissionId.Value = Request["permissionid"];
                hdPermissionType.Value = Request["permissiontype"];

                var list = Biz.GetEnabledBcPermissionList(int.Parse(hdPermissionType.Value),
                    int.Parse(hdPermissionId.Value));
                if (list != null && list.Count > 0)
                {
                    foreach (TableRow row in gvBcPermission.Rows)
                    {
                        var hdModuleId = (HiddenField) row.Cells[2].FindControl("hdModuleId");
                        var hdFuncPointId = (HiddenField) row.Cells[2].FindControl("hdFuncPointId");
                        var cblPermissions = (CheckBoxList) row.Cells[2].FindControl("cblPermissions");
                        var result = list.Where(n => n.UserRoleId == int.Parse(Request["permissionid"]) &&
                                                     n.PermissionType == int.Parse(Request["permissiontype"]) &&
                                                     n.ModuleId == hdModuleId.Value &&
                                                     n.FuncPointId == hdFuncPointId.Value).ToList();
                        if (result.Count > 0)
                        {
                            var pcs = result[0].Permissions.Split(',');
                            foreach (ListItem li in cblPermissions.Items)
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
                    }
                }
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }

        private List<BcPermissionEntity> PrepareFormData()
        {
            var list = new List<BcPermissionEntity>();
            foreach (TableRow row in gvBcPermission.Rows)
            {
                var hdModuleId = (HiddenField) row.Cells[0].FindControl("hdModuleId");
                var hdFuncPointId = (HiddenField) row.Cells[1].FindControl("hdFuncPointId");
                var cblPermissions = (CheckBoxList) row.Cells[2].FindControl("cblPermissions");
                var entity = new BcPermissionEntity();
                entity.PermissionType = int.Parse(hdPermissionType.Value);
                entity.UserRoleId = int.Parse(hdPermissionId.Value);
                entity.ModuleId = hdModuleId.Value;
                entity.FuncPointId = hdFuncPointId.Value;
                foreach (ListItem li in cblPermissions.Items)
                {
                    if (li.Selected)
                        entity.Permissions += li.Value + ",";
                }
                entity.Permissions = string.IsNullOrEmpty(entity.Permissions)
                    ? string.Empty
                    : entity.Permissions.Remove(entity.Permissions.Length - 1);
                list.Add(entity);
            }
            return list;
        }
    }
}