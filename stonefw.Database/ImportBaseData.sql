insert Sys_EnumName (Type,Value,Name)  values ( 'ExcuteResultEnum','Failed','执行失败，请重试！')
insert Sys_EnumName (Type,Value,Name)  values ( 'ExcuteResultEnum','IsExist','保存失败，原因：该记录已存在！')
insert Sys_EnumName (Type,Value,Name)  values ( 'ExcuteResultEnum','IsOccupied','删除失败，原因：该记录被占用，请先删除关联项！')
insert Sys_EnumName (Type,Value,Name)  values ( 'ExcuteResultEnum','IsSuperAdmin','删除失败，原因：不能删除超级管理员！')
insert Sys_EnumName (Type,Value,Name)  values ( 'ExcuteResultEnum','Success','执行成功！')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Ac_Test','*会计模块测试*')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_AutoCode','自动编号管理')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_ErrorLog','错误日志管理')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_Group','组别管理')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_Permission','用户角色权限')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_Role','角色管理')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_Test','*基础模块测试*')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_User','用户管理')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_UserRole','用户角色管理')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Cu_Customer','客户管理')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Cu_Test','*客户模块测试*')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','In_Test','*仓库模块测试*')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Mf_Test','"生产模块测试"')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Pd_Test','*产品模块测试*')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Td_Test','*贸易模块测试*')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Tk_Test','"任务模块测试"')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Account','会计模块')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Base','基础模块')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Customer','客户模块')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Inventory','仓库模块')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Manufacture','生产模块')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Production','产品模块')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Task','任务模块')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Trade','贸易模块')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysPermsPointEnum','Add','新增')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysPermsPointEnum','Delete','删除')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysPermsPointEnum','Edit','修改')
insert Sys_EnumName (Type,Value,Name)  values ( 'SysPermsPointEnum','View','查看')


insert Sys_GlobalSetting (SysKey,SysValue)  values ( 'BuildingPage','/CustomPage/building.html')
insert Sys_GlobalSetting (SysKey,SysValue)  values ( 'ErrorLogPath','/TempFile/ErrorLogPath')
insert Sys_GlobalSetting (SysKey,SysValue)  values ( 'ErrorPage','/CustomPage/error.html')
insert Sys_GlobalSetting (SysKey,SysValue)  values ( 'GridViewPageSize','8')
insert Sys_GlobalSetting (SysKey,SysValue)  values ( 'SuperAdmins','admin')
insert Sys_GlobalSetting (SysKey,SysValue)  values ( 'SysDescription','个人工作平台')
insert Sys_GlobalSetting (SysKey,SysValue)  values ( 'SysName','个人工作平台')


SET IDENTITY_INSERT Sys_Menu ON
insert Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 1,'系统设置',1,6,0,'','','',1,0)
insert Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 2,'角色管理',3,3,7,'','/BaseModule/BcRole/BcRoleList.aspx','',1,0)
insert Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 3,'用户管理',3,1,7,'','/BaseModule/BcUserInfo/BcUserInfoList.aspx','',1,0)
insert Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 4,'组别管理',3,2,7,'','/BaseModule/BcGroup/BcGroupList.aspx','',1,0)
insert Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 5,'用户角色权限',3,5,7,'','/BaseModule/BcPermission/BcPermissionList.aspx','',1,0)
insert Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 6,'客户管理',1,1,0,'','','',1,0)
insert Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 7,'用户管理',2,1,1,'','','',1,0)
insert Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 8,'错误日志管理',3,2,11,'','/BaseModule/BcLogError/BcLogErrorList.aspx','',1,0)
insert Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 9,'客户管理',2,1,6,'','/CustomerModule/CuCustomer/CuCustomerList.aspx','',1,0)
insert Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 10,'自动编号管理',3,1,11,'','/BaseModule/BcAutoCode/BcAutoCodeList.aspx','',1,0)
insert Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 11,'系统管理',2,5,1,'','','',1,0)
insert Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 12,'用户角色关系',3,4,7,'','/BaseModule/BcUserRole/BcUserRoleList.aspx','',1,0)
SET IDENTITY_INSERT Sys_Menu OFF


insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcAutoCode/BcAutoCodeDetail.aspx','Bc_AutoCode')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcAutoCode/BcAutoCodeList.aspx','Bc_AutoCode')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcGroup/BcGroupDetail.aspx','Bc_Group')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcGroup/BcGroupList.aspx','Bc_Group')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcLogError/BcLogErrorDetail.aspx','Bc_ErrorLog')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcLogError/BcLogErrorList.aspx','Bc_ErrorLog')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcPermission/BcPermissionDetail.aspx','Bc_Permission')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcPermission/BcPermissionList.aspx','Bc_Permission')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcRole/BcRoleDetail.aspx','Bc_Role')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcRole/BcRoleList.aspx','Bc_Role')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcUserInfo/BcUserInfoDetail.aspx','Bc_User')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcUserInfo/BcUserInfoList.aspx','Bc_User')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcUserRole/BcUserRoleDetail.aspx','Bc_UserRole')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcUserRole/BcUserRoleList.aspx','Bc_UserRole')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/CustomerModule/CuCustomer/CuContactPersonDetail.aspx','Cu_Customer')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/CustomerModule/CuCustomer/CuContactPersonList.aspx','Cu_Customer')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/CustomerModule/CuCustomer/CuCustomerDetail.aspx','Cu_Customer')
insert Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/CustomerModule/CuCustomer/CuCustomerList.aspx','Cu_Customer')


insert Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Account','Acc_Test','View')
insert Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_AutoCode','Add,Delete,Edit,View')
insert Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_ErrorLog','View')
insert Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_Group','Add,Delete,Edit,View')
insert Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_Permission','Add,Delete,View')
insert Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_Role','Add,Delete,Edit,View')
insert Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_User','View,Add,Edit,Delete')
insert Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_UserRole','Add,Delete,View')
insert Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Customer','Cu_Customer','Add,Delete,Edit,View')
insert Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Inventory','In_Test','View')
insert Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Manufacture','Mf_Test','View')
insert Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Production','Pd_Test','View')
insert Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Trade','Td_Test','View')


SET IDENTITY_INSERT Bc_Group ON
insert Bc_Group (GroupId,GroupName)  values (1, '销售部')
insert Bc_Group (GroupId,GroupName)  values (2, '采购部')
insert Bc_Group (GroupId,GroupName)  values (3, '生产部')
insert Bc_Group (GroupId,GroupName)  values (4, '人事部')
insert Bc_Group (GroupId,GroupName)  values (5, '市场部')
insert Bc_Group (GroupId,GroupName)  values (6, '行政部')
SET IDENTITY_INSERT Bc_Group OFF


SET IDENTITY_INSERT Bc_Role ON
insert Bc_Role (RoleId,RoleName)  values ( 1,'普通用户')
insert Bc_Role (RoleId,RoleName)  values ( 2,'系统管理员')
SET IDENTITY_INSERT Bc_Role OFF


SET IDENTITY_INSERT Bc_UserInfo ON
insert Bc_UserInfo (UserId,GroupId,UserAccount,UserName,Password,Sex,OfficePhone,MobilePhone,Email,ActivityFlag,DeleteFlag)  values ( 1,1,'admin','管理员','eedc08f62509c304',1,'','123','',1,0)
insert Bc_UserInfo (UserId,GroupId,UserAccount,UserName,Password,Sex,OfficePhone,MobilePhone,Email,ActivityFlag,DeleteFlag)  values ( 2,2,'shi0090','石佳劼','1cef6c965a29cdbfadcc69046d4ddc21',1,'','123','',1,0)
insert Bc_UserInfo (UserId,GroupId,UserAccount,UserName,Password,Sex,OfficePhone,MobilePhone,Email,ActivityFlag,DeleteFlag)  values ( 3,3,'hujingtao','胡锦涛','3fe4c3733606ea70',1,'123','123','123',1,0)
insert Bc_UserInfo (UserId,GroupId,UserAccount,UserName,Password,Sex,OfficePhone,MobilePhone,Email,ActivityFlag,DeleteFlag)  values ( 4,4,'aaa','aaa','ef3a300883fb464f',1,'','','',1,0)
insert Bc_UserInfo (UserId,GroupId,UserAccount,UserName,Password,Sex,OfficePhone,MobilePhone,Email,ActivityFlag,DeleteFlag)  values ( 5,5,'bbb','bbb','628a02da7b5e4db0',1,'123','123','123',1,0)
insert Bc_UserInfo (UserId,GroupId,UserAccount,UserName,Password,Sex,OfficePhone,MobilePhone,Email,ActivityFlag,DeleteFlag)  values ( 6,6,'ccc','ccc','083d2e81543235d5',1,'333','333','333',1,0)
SET IDENTITY_INSERT Bc_UserInfo OFF


insert Bc_UserRole (UserId,RoleId)  values ( 2,2)
insert Bc_UserRole (UserId,RoleId)  values ( 3,2)


insert Bc_Permission (UserRoleId,PermissionType,ModuleId,FuncPointId,Permissions)  values ( 2,1,'Base','Bc_AutoCode','Add,Delete,Edit,View')
insert Bc_Permission (UserRoleId,PermissionType,ModuleId,FuncPointId,Permissions)  values ( 2,1,'Base','Bc_Group','Add,Delete,Edit,View')
insert Bc_Permission (UserRoleId,PermissionType,ModuleId,FuncPointId,Permissions)  values ( 2,1,'Base','Bc_Permission','Add,Delete,View')
insert Bc_Permission (UserRoleId,PermissionType,ModuleId,FuncPointId,Permissions)  values ( 2,1,'Base','Bc_Role','Add,Delete,Edit,View')
insert Bc_Permission (UserRoleId,PermissionType,ModuleId,FuncPointId,Permissions)  values ( 2,1,'Base','Bc_User','View,Add,Edit,Delete')
insert Bc_Permission (UserRoleId,PermissionType,ModuleId,FuncPointId,Permissions)  values ( 2,1,'Base','Bc_UserRole','Add,Delete,View')