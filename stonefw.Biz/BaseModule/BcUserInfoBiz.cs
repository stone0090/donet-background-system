using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Transactions;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Entity.Extension;
using Stonefw.Entity.SystemModule;
using Stonefw.Utility;
using Stonefw.Utility.EntitySql;

namespace Stonefw.Biz.BaseModule
{
    public class BcUserInfoBiz
    {
        private Database _db;

        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public ExcuteResultEnum DeleteBcUserInfo(int userId)
        {
            if (EntityExecution.Count<BcUserRoleEntity>(n => n.UserId == userId) > 0)
                return ExcuteResultEnum.IsOccupied;

            var entity = GetSingleBcUserInfo(userId);
            if (new SysGlobalSettingBiz().IsSuperAdmin(entity.UserAccount))
                return ExcuteResultEnum.IsSuperAdmin;

            entity.DeleteFlag = true;
            return UpdateBcUserInfo(entity);
        }

        public ExcuteResultEnum AddNewBcUserInfo(BcUserInfoEntity entity, string roleIds = null)
        {
            if (
                EntityExecution.Count<BcUserInfoEntity>(
                    n => n.UserAccount == entity.UserAccount && n.DeleteFlag == false) > 0)
                return ExcuteResultEnum.IsExist;

            using (TransactionScope ts = new TransactionScope())
            {
                entity.UserId = null;
                entity.DeleteFlag = false;
                entity.Password = Encryption.Encrypt(entity.Password);
                var id = EntityExecution.InsertWithIdentity(entity);
                if (!string.IsNullOrEmpty(roleIds))
                {
                    foreach (string roleId in roleIds.Split(','))
                    {
                        var userRoleEntity = new BcUserRoleEntity {UserId = (int?) id, RoleId = int.Parse(roleId)};
                        EntityExecution.Insert(userRoleEntity);
                    }
                }
                ts.Complete();
            }
            return ExcuteResultEnum.Success;
        }

        public ExcuteResultEnum UpdateBcUserInfo(BcUserInfoEntity entity, string roleIds = null)
        {
            entity.Password = Encryption.Encrypt(entity.Password);
            using (TransactionScope ts = new TransactionScope())
            {
                entity.Update();
                EntityExecution.Delete<BcUserRoleEntity>(n => n.UserId == entity.UserId);
                if (!string.IsNullOrEmpty(roleIds))
                {
                    foreach (string roleId in roleIds.Split(','))
                    {
                        var userRoleEntity = new BcUserRoleEntity
                        {
                            UserId = entity.UserId,
                            RoleId = int.Parse(roleId)
                        };
                        EntityExecution.Insert(userRoleEntity);
                    }
                }
                ts.Complete();
            }
            return ExcuteResultEnum.Success;
        }

        public BcUserInfoEntity GetSingleBcUserInfo(int userId)
        {
            var entity = EntityExecution.SelectOne<BcUserInfoEntity>(n => n.UserId == userId);
            if (entity != null) entity.Password = Encryption.Decrypt(entity.Password);
            return entity;
        }

        public List<BcUserInfoEntity> GetEnabledBcUserInfoList()
        {
            return EntityExecution.SelectAll<BcUserInfoEntity>(n => n.DeleteFlag == false && n.ActivityFlag == true);
        }

        public LoginStatusEnum DoLogin(string userAccount, string password)
        {
            var entity =
                EntityExecution.SelectOne<BcUserInfoEntity>(n => n.UserAccount == userAccount && n.DeleteFlag == false);

            if (entity == null)
                return LoginStatusEnum.UserNotExist;

            if (entity.Password != Encryption.Encrypt(password))
                return LoginStatusEnum.PasswordError;

            if ((bool) !entity.ActivityFlag)
                return LoginStatusEnum.UserDisabled;

            return LoginStatusEnum.Success;
        }

        public BcUserInfoEntity GetBcUserInfoWithPermission(string userAccount)
        {
            var entity =
                EntityExecution.SelectOne<BcUserInfoEntity>(
                    n => n.UserAccount == userAccount && n.ActivityFlag == true && n.DeleteFlag == false);

            //获取用户的角色
            var userRoleList = new BcUserRoleBiz().GetBcUserRoleList(entity.UserId);
            entity.RoleList = new List<BcRoleEntity>();
            foreach (BcUserRoleEntity userRoleEntity in userRoleList)
            {
                entity.RoleList.Add(new BcRoleEntity()
                {
                    RoleId = userRoleEntity.RoleId,
                    RoleName = userRoleEntity.RoleName
                });
            }

            //获取用户是否为超级管理员
            entity.IsSuperAdmin = new SysGlobalSettingBiz().IsSuperAdmin(entity.UserAccount);

            //获取用户的权限
            if (entity.IsSuperAdmin)
            {
                //如果是超级管理员，获取所有权限
                entity.PermisionList = new List<PermissionEntity>();
                var sysRelationList = new SysRelationBiz().GetEnabledSysRelationList();
                foreach (SysRelationEntity sysRelationEntity in sysRelationList)
                {
                    entity.PermisionList.Add(new PermissionEntity()
                    {
                        ModuleId = sysRelationEntity.ModuleId,
                        FuncPointId = sysRelationEntity.FuncPointId,
                        PermissionList = sysRelationEntity.PermissionList
                    });
                }
            }
            else
            {
                //如果不是超级管理员，获取用户权限和角色权限
                var bcPermissionBiz = new BcPermissionBiz();

                //1、获取用户权限
                entity.PermisionList = new List<PermissionEntity>();
                var bcUserPermissionList = bcPermissionBiz.GetEnabledBcPermissionList(2, entity.UserId);
                foreach (BcPermissionEntity bcPermissionEntity in bcUserPermissionList)
                {
                    entity.PermisionList.Add(new PermissionEntity()
                    {
                        ModuleId = bcPermissionEntity.ModuleId,
                        FuncPointId = bcPermissionEntity.FuncPointId,
                        PermissionList = bcPermissionEntity.Permissions.Split(',').ToList()
                    });
                }

                //2、获取角色权限
                foreach (BcRoleEntity bcRoleEntity in entity.RoleList)
                {
                    var bcRolePermissionList = bcPermissionBiz.GetEnabledBcPermissionList(1, bcRoleEntity.RoleId);
                    foreach (BcPermissionEntity bcPermissionEntity in bcRolePermissionList)
                    {
                        var list =
                            entity.PermisionList.Where(
                                n =>
                                    n.ModuleId == bcPermissionEntity.ModuleId &&
                                    n.FuncPointId == bcPermissionEntity.FuncPointId).ToList();
                        if (list.Count > 0)
                        {
                            var pcList = bcPermissionEntity.Permissions.Split(',').ToList();
                            foreach (string s in pcList)
                            {
                                if (!list[0].PermissionList.Contains(s))
                                    list[0].PermissionList.Add(s);
                            }
                        }
                        else
                        {
                            entity.PermisionList.Add(new PermissionEntity()
                            {
                                ModuleId = bcPermissionEntity.ModuleId,
                                FuncPointId = bcPermissionEntity.FuncPointId,
                                PermissionList = bcPermissionEntity.Permissions.Split(',').ToList()
                            });
                        }
                    }
                }
            }

            //根据用户权限，获取用户的菜单列表
            entity.MenuList = new SysMenuBiz().GetEnabledSysMenuListByPermission(entity.PermisionList);

            return entity;
        }

        public List<BcUserInfoEntity> GetBcUserInfoList(int? groupId = 0, int? roleId = 0, string userName = "")
        {
            string sql = @"SELECT DISTINCT a.*,d.GroupName FROM Bc_UserInfo a
                            LEFT JOIN Bc_UserRole b ON a.UserId = b.UserId
                            LEFT JOIN Bc_Role c ON b.RoleId = c.RoleId 
                            LEFT JOIN Bc_Group d ON a.GroupId = d.GroupId
                            WHERE a.DeleteFlag = 0 ";
            if (roleId != 0) sql += " AND c.RoleId = @RoleId ";
            if (groupId != 0) sql += " AND a.GroupId = @GroupId ";
            if (!string.IsNullOrEmpty(userName)) sql += " AND a.UserName like @UserName ";
            using (DbCommand dm = Db.GetSqlStringCommand(sql))
            {
                if (roleId != 0) Db.AddInParameter(dm, "@RoleId", DbType.Int32, roleId);
                if (groupId != 0) Db.AddInParameter(dm, "@GroupId", DbType.Int32, groupId);
                if (!string.IsNullOrEmpty(userName))
                    Db.AddInParameter(dm, "@UserName", DbType.AnsiString, "%" + userName + "%");
                return DataTableHepler.DataTableToList<BcUserInfoEntity>(Db.ExecuteDataTable(dm));
            }
        }
    }
}