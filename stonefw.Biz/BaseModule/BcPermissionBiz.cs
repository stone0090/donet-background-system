using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Utility.EntitySql;

namespace Stonefw.Biz.BaseModule
{
    public class BcPermissionBiz
    {
        public void DeleteBcPermission(int? permissionId, int? permissionType)
        {
            EntityExecution.Delete<BcPermissionEntity>(
                n => n.UserRoleId == permissionId && n.PermissionType == permissionType);
        }

        public void DeleteBcPermission(int permissionId, int permissionType, string moduleId, string funcPointId)
        {
            var entity = new BcPermissionEntity();
            entity.UserRoleId = permissionId;
            entity.PermissionType = permissionType;
            entity.ModuleId = moduleId;
            entity.FuncPointId = funcPointId;
            EntityExecution.Delete(entity);
        }

        public void AddNewBcPermission(List<BcPermissionEntity> list)
        {
            using (var ts = new TransactionScope())
            {
                foreach (BcPermissionEntity entity in list)
                {
                    if (!string.IsNullOrEmpty(entity.Permissions))
                        entity.Insert();
                }
                ts.Complete();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissionType">1=角色权限，2=用户权限</param>
        /// <param name="userRoleId"></param>
        public List<BcPermissionEntity> GetEnabledBcPermissionList(int? permissionType, int? userRoleId = 0)
        {
            //获取可用的权限列表
            var allBcPermissionList = EntityExecution.SelectAll<BcPermissionEntity>();
            var enabledBcPermissionList = new List<BcPermissionEntity>();
            if (userRoleId == 0)
            {
                enabledBcPermissionList.AddRange(allBcPermissionList.Where(n =>
                    n.PermissionType == permissionType));
            }
            else
            {
                enabledBcPermissionList.AddRange(allBcPermissionList.Where(n =>
                    n.UserRoleId == userRoleId && n.PermissionType == permissionType));
            }

            //加载可用的菜单列表
            var listEnabledSysMenuEntity = new SysMenuBiz().GetEnabledSysMenuList();

            //根据可用的菜单列表，去掉没有起到作用的功能点
            for (int i = enabledBcPermissionList.Count - 1; i >= 0; i--)
            {
                var permisionEntity = enabledBcPermissionList[i];
                var list =
                    listEnabledSysMenuEntity.Where(
                        n => n.ModuleId == permisionEntity.ModuleId && n.FuncPointId == permisionEntity.FuncPointId)
                        .ToList();
                if (list.Count <= 0) enabledBcPermissionList.Remove(permisionEntity);
            }

            //补充所有id的name
            var allBcRoleList = new BcRoleBiz().GetBcRoleList();
            var allBcUserInfoList = new BcUserInfoBiz().GetBcUserInfoList();

            foreach (BcPermissionEntity bcPermissionEntity in enabledBcPermissionList)
            {
                bcPermissionEntity.ModuleName =
                    SysEnumNameExBiz.GetDescription<SysModuleEnum>(bcPermissionEntity.ModuleId);
                bcPermissionEntity.FuncPointName =
                    SysEnumNameExBiz.GetDescription<SysFuncPointEnum>(bcPermissionEntity.FuncPointId);
                if (permissionType == 1)
                {
                    var list = allBcRoleList.Where(n => n.RoleId == bcPermissionEntity.UserRoleId).ToList();
                    if (list.Count > 0) bcPermissionEntity.UserRoleName = list[0].RoleName;
                }
                else if (permissionType == 2)
                {
                    var list = allBcUserInfoList.Where(n => n.UserId == bcPermissionEntity.UserRoleId).ToList();
                    if (list.Count > 0) bcPermissionEntity.UserRoleName = list[0].UserName;
                }

                if (!string.IsNullOrEmpty(bcPermissionEntity.Permissions))
                {
                    bcPermissionEntity.PermissionList = new List<string>();
                    bcPermissionEntity.PermissionNameList = new List<string>();
                    var list = bcPermissionEntity.Permissions.Split(',').ToList();
                    foreach (string s in list)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            bcPermissionEntity.PermissionList.Add(s);
                            bcPermissionEntity.PermissionNameList.Add(
                                SysEnumNameExBiz.GetDescription<SysFuncPointEnum>(s));
                        }
                    }
                    if (bcPermissionEntity.PermissionNameList.Count > 0)
                        bcPermissionEntity.PermissionNames = string.Join(",",
                            bcPermissionEntity.PermissionNameList.ToArray());
                }
            }
            return
                enabledBcPermissionList.OrderBy(n => n.UserRoleId)
                    .ThenBy(n => n.ModuleId)
                    .ThenBy(n => n.FuncPointId)
                    .ToList();
        }
    }
}