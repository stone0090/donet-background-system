using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using stonefw.Biz.SystemModule;
using stonefw.Utility;
using stonefw.Utility.EntityExpressions;
using stonefw.Entity.Enum;
using stonefw.Dao.BaseModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.Extension;
using stonefw.Entity.SystemModule;


namespace stonefw.Biz.BaseModule
{
    public class BcUserInfoBiz
    {
        private BcUserInfoDao _dao;
        private BcUserInfoDao Dao
        {
            get { return _dao ?? (_dao = new BcUserInfoDao()); }
        }

        public List<BcUserInfoEntity> GetBcUserInfoList(int? groupId = 0, int? roleId = 0, string userName = "")
        {
            return Dao.GetBcUserInfoList(roleId, groupId, userName);
        }
        public ExcuteResult DeleteBcUserInfo(int userId)
        {
            if (EntityExecution.GetEntityCount2<BcUserRoleEntity>(n => n.UserId == userId) > 0)
                return ExcuteResult.IsOccupied;

            var entity = GetSingleBcUserInfo(userId);
            if (new SysGlobalSettingBiz().IsSuperAdmin(entity.UserAccount))
                return ExcuteResult.IsSuperAdmin;

            entity.DeleteFlag = true;
            return UpdateBcUserInfo(entity);
        }
        public ExcuteResult AddNewBcUserInfo(BcUserInfoEntity entity, string roleIds = null)
        {
            if (EntityExecution.GetEntityCount2<BcUserInfoEntity>(n => n.UserAccount == entity.UserAccount && n.DeleteFlag == false) > 0)
                return ExcuteResult.IsExist;

            using (TransactionScope ts = new TransactionScope())
            {
                entity.UserId = null;
                entity.DeleteFlag = false;
                entity.Password = Encryption.Encrypt(entity.Password);
                var id = EntityExecution.InsertEntityWithIdentity(entity);
                if (!string.IsNullOrEmpty(roleIds))
                {
                    foreach (string roleId in roleIds.Split(','))
                    {
                        var userRoleEntity = new BcUserRoleEntity { UserId = (int?)id, RoleId = int.Parse(roleId) };
                        EntityExecution.InsertEntity(userRoleEntity);
                    }
                }
                ts.Complete();
            }
            return ExcuteResult.Success;
        }
        public ExcuteResult UpdateBcUserInfo(BcUserInfoEntity entity, string roleIds = null)
        {
            entity.Password = Encryption.Encrypt(entity.Password);
            using (TransactionScope ts = new TransactionScope())
            {
                EntityExecution.UpdateEntity(entity);
                EntityExecution.DeleteEntity2<BcUserRoleEntity>(n => n.UserId == entity.UserId);
                if (!string.IsNullOrEmpty(roleIds))
                {
                    foreach (string roleId in roleIds.Split(','))
                    {
                        var userRoleEntity = new BcUserRoleEntity
                        {
                            UserId = entity.UserId,
                            RoleId = int.Parse(roleId)
                        };
                        EntityExecution.InsertEntity(userRoleEntity);
                    }
                }
                ts.Complete();
            }
            return ExcuteResult.Success;
        }
        public BcUserInfoEntity GetSingleBcUserInfo(int userId)
        {
            var entity = EntityExecution.ReadEntity2<BcUserInfoEntity>(n => n.UserId == userId);
            if (entity != null) entity.Password = Encryption.Decrypt(entity.Password);
            return entity;
        }

        public List<BcUserInfoEntity> GetEnabledBcUserInfoList()
        {
            return EntityExecution.ReadEntityList2<BcUserInfoEntity>(n => n.DeleteFlag == false && n.ActivityFlag == true);
        }
        public LoginStatus DoLogin(string userAccount, string password)
        {
            var entity = EntityExecution.ReadEntity2<BcUserInfoEntity>(n => n.UserAccount == userAccount && n.DeleteFlag == false);

            if (entity == null)
                return LoginStatus.UserNotExist;

            if (entity.Password != Encryption.Encrypt(password))
                return LoginStatus.PasswordError;

            if ((bool)!entity.ActivityFlag)
                return LoginStatus.UserDisabled;

            return LoginStatus.Success;
        }
        public BcUserInfoEntity GetBcUserInfoWithPermission(string userAccount)
        {
            var entity = EntityExecution.ReadEntity2<BcUserInfoEntity>(n => n.UserAccount == userAccount && n.ActivityFlag == true && n.DeleteFlag == false);

            //获取用户的角色
            var userRoleList = new BcUserRoleBiz().GetBcUserRoleList(entity.UserId);
            entity.RoleList = new List<BcRoleEntity>();
            foreach (BcUserRoleEntity userRoleEntity in userRoleList)
            {
                entity.RoleList.Add(new BcRoleEntity() { RoleId = userRoleEntity.RoleId, RoleName = userRoleEntity.RoleName });
            }

            //获取用户是否为超级管理员
            entity.IsSuperAdmin = new SysGlobalSettingBiz().IsSuperAdmin(entity.UserAccount);

            //获取用户的权限
            if (entity.IsSuperAdmin)
            {
                //如果是超级管理员，获取所有权限
                entity.PermisionList = new List<PermissionEntity>();
                var sysRelationList = new SysMfpRelationBiz().GetEnabledSysMfpRelationList();
                foreach (SysMfpRelationEntity sysMfpRelationEntity in sysRelationList)
                {
                    entity.PermisionList.Add(new PermissionEntity()
                    {
                        ModuleId = sysMfpRelationEntity.ModuleId,
                        FuncPointId = sysMfpRelationEntity.FuncPointId,
                        PermissionList = sysMfpRelationEntity.PermissionList
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
                        var list = entity.PermisionList.Where(n => n.ModuleId == bcPermissionEntity.ModuleId && n.FuncPointId == bcPermissionEntity.FuncPointId).ToList();
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

    }
}
