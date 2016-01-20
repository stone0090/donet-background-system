USE [stonefw]
GO

/****** Object:  Table [dbo].[Bc_AutoCode]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Bc_AutoCode](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Prefix] [varchar](20) NOT NULL,
	[DateFormat] [varchar](20) NOT NULL,
	[FuncPointId] [varchar](50) NOT NULL,
	[Digit] [int] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[CurrentDate] [date] NOT NULL,
	[CurrentCode] [int] NOT NULL,
 CONSTRAINT [PK_AutoCodeSetting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Bc_Group]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Bc_Group](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Bc_LogError]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Bc_LogError](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[OpUrl] [varchar](500) NOT NULL,
	[OpTime] [datetime] NOT NULL,
	[OpHostAddress] [varchar](50) NOT NULL,
	[OpHostName] [varchar](50) NOT NULL,
	[OpUserAgent] [varchar](max) NOT NULL,
	[OpQueryString] [varchar](max) NOT NULL,
	[OpHttpMethod] [varchar](10) NOT NULL,
	[Message] [varchar](max) NOT NULL,
 CONSTRAINT [PK_LogError] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Bc_Permission]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Bc_Permission](
	[UserRoleId] [int] NOT NULL,
	[PermissionType] [int] NOT NULL,
	[ModuleId] [varchar](50) NOT NULL,
	[FuncPointId] [varchar](50) NOT NULL,
	[Permissions] [varchar](500) NOT NULL,
 CONSTRAINT [PK_Bc_Permission] PRIMARY KEY CLUSTERED 
(
	[UserRoleId] ASC,
	[PermissionType] ASC,
	[ModuleId] ASC,
	[FuncPointId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Bc_Role]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Bc_Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Bc_UserInfo]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Bc_UserInfo](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NOT NULL,
	[UserAccount] [varchar](50) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [varchar](500) NOT NULL,
	[Sex] [bit] NOT NULL,
	[OfficePhone] [varchar](50) NOT NULL,
	[MobilePhone] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[ActivityFlag] [bit] NOT NULL,
	[DeleteFlag] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Bc_UserRole]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bc_UserRole](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_Bc_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Cu_ContactPerson]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cu_ContactPerson](
	[CuId] [varchar](50) NOT NULL,
	[CpId] [int] IDENTITY(1,1) NOT NULL,
	[CpName] [varchar](50) NOT NULL,
	[Mobile] [varchar](50) NOT NULL,
	[Phone] [varchar](50) NOT NULL,
	[QQ] [varchar](50) NOT NULL,
	[WeChat] [varchar](50) NOT NULL,
	[Weibo] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Other] [varchar](50) NOT NULL,
	[Remark] [varchar](50) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[DeleteFlag] [bit] NOT NULL,
 CONSTRAINT [PK_Cu_ContactPerson] PRIMARY KEY CLUSTERED 
(
	[CpId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Cu_Customer]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cu_Customer](
	[CuId] [varchar](50) NOT NULL,
	[CuName] [varchar](50) NOT NULL,
	[District] [varchar](50) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[Remark] [varchar](max) NOT NULL,
	[ActivityFlag] [bit] NOT NULL,
	[DeleteFlag] [bit] NOT NULL,
 CONSTRAINT [PK_Cu_Customer] PRIMARY KEY CLUSTERED 
(
	[CuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Sys_EnumName]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sys_EnumName](
	[Type] [varchar](50) NOT NULL,
	[Value] [varchar](50) NOT NULL,
	[Name] [varchar](500) NOT NULL,
 CONSTRAINT [PK_Sys_NameList] PRIMARY KEY CLUSTERED 
(
	[Type] ASC,
	[Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Sys_GlobalSetting]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sys_GlobalSetting](
	[SysKey] [varchar](50) NOT NULL,
	[SysValue] [varchar](max) NOT NULL,
 CONSTRAINT [PK_SysSetting] PRIMARY KEY CLUSTERED 
(
	[SysKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Sys_Menu]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sys_Menu](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[MenuName] [varchar](50) NOT NULL,
	[MenuLevel] [int] NOT NULL,
	[Seq] [int] NOT NULL,
	[FatherNode] [int] NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[PageUrl] [varchar](500) NOT NULL,
	[UrlParameter] [varchar](500) NOT NULL,
	[ActivityFlag] [bit] NOT NULL,
	[DeleteFlag] [bit] NOT NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Sys_PageFuncPoint]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sys_PageFuncPoint](
	[PageUrl] [varchar](500) NOT NULL,
	[FuncPointId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Sys_PageFuncPoint] PRIMARY KEY CLUSTERED 
(
	[PageUrl] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Sys_Relation]    Script Date: 2016/1/20 5:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sys_Relation](
	[ModuleId] [varchar](50) NOT NULL,
	[FuncPointId] [varchar](50) NOT NULL,
	[Permissions] [varchar](500) NOT NULL,
 CONSTRAINT [PK_Sys_MfpRelation] PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC,
	[FuncPointId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Bc_Group]  WITH CHECK ADD  CONSTRAINT [FK_Bc_Group_Bc_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Bc_Group] ([GroupId])
GO
ALTER TABLE [dbo].[Bc_Group] CHECK CONSTRAINT [FK_Bc_Group_Bc_Group]
GO
ALTER TABLE [dbo].[Bc_UserInfo]  WITH CHECK ADD  CONSTRAINT [FK_Bc_UserInfo_Bc_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Bc_Group] ([GroupId])
GO
ALTER TABLE [dbo].[Bc_UserInfo] CHECK CONSTRAINT [FK_Bc_UserInfo_Bc_Group]
GO
ALTER TABLE [dbo].[Bc_UserRole]  WITH CHECK ADD  CONSTRAINT [FK_Bc_UserRole_Bc_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Bc_Role] ([RoleId])
GO
ALTER TABLE [dbo].[Bc_UserRole] CHECK CONSTRAINT [FK_Bc_UserRole_Bc_Role]
GO
ALTER TABLE [dbo].[Bc_UserRole]  WITH CHECK ADD  CONSTRAINT [FK_Bc_UserRole_Bc_UserInfo] FOREIGN KEY([UserId])
REFERENCES [dbo].[Bc_UserInfo] ([UserId])
GO
ALTER TABLE [dbo].[Bc_UserRole] CHECK CONSTRAINT [FK_Bc_UserRole_Bc_UserInfo]
GO
ALTER TABLE [dbo].[Cu_ContactPerson]  WITH CHECK ADD  CONSTRAINT [FK_Cu_ContactPerson_Cu_Customer1] FOREIGN KEY([CuId])
REFERENCES [dbo].[Cu_Customer] ([CuId])
GO
ALTER TABLE [dbo].[Cu_ContactPerson] CHECK CONSTRAINT [FK_Cu_ContactPerson_Cu_Customer1]
GO

USE [stonefw]
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'ExcuteResultEnum','Failed','执行失败，请重试！')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'ExcuteResultEnum','IsExist','保存失败，原因：该记录已存在！')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'ExcuteResultEnum','IsOccupied','删除失败，原因：该记录被占用，请先删除关联项！')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'ExcuteResultEnum','IsSuperAdmin','删除失败，原因：不能删除超级管理员！')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'ExcuteResultEnum','Success','执行成功！')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Ac_Test','*会计模块测试*')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_AutoCode','自动编号管理')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_ErrorLog','错误日志管理')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_Group','组别管理')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_Permission','用户角色权限')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_Role','角色管理')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_Test','*基础模块测试*')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_User','用户管理')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Bc_UserRole','用户角色管理')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Cu_Customer','客户管理')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Cu_Test','*客户模块测试*')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','In_Test','*仓库模块测试*')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Mf_Test','"生产模块测试"')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Pd_Test','*产品模块测试*')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Td_Test','*贸易模块测试*')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysFuncPointEnum','Tk_Test','"任务模块测试"')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Account','会计模块')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Base','基础模块')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Customer','客户模块')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Inventory','仓库模块')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Manufacture','生产模块')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Production','产品模块')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Task','任务模块')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysModuleEnum','Trade','贸易模块')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysPermsPointEnum','Add','新增')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysPermsPointEnum','Delete','删除')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysPermsPointEnum','Edit','修改')
INSERT INTO Sys_EnumName (Type,Value,Name)  values ( 'SysPermsPointEnum','View','查看')


INSERT INTO Sys_GlobalSetting (SysKey,SysValue)  values ( 'BuildingPage','/CustomPage/building.html')
INSERT INTO Sys_GlobalSetting (SysKey,SysValue)  values ( 'ErrorLogPath','/TempFile/ErrorLogPath')
INSERT INTO Sys_GlobalSetting (SysKey,SysValue)  values ( 'ErrorPage','/CustomPage/error.html')
INSERT INTO Sys_GlobalSetting (SysKey,SysValue)  values ( 'GridViewPageSize','8')
INSERT INTO Sys_GlobalSetting (SysKey,SysValue)  values ( 'SuperAdmins','admin')
INSERT INTO Sys_GlobalSetting (SysKey,SysValue)  values ( 'SysDescription','个人工作平台')
INSERT INTO Sys_GlobalSetting (SysKey,SysValue)  values ( 'SysName','个人工作平台')


SET IDENTITY_INSERT  Sys_Menu ON
INSERT INTO Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 1,'系统设置',1,6,0,'','','',1,0)
INSERT INTO Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 2,'角色管理',3,3,7,'','/BaseModule/BcRole/BcRoleList.aspx','',1,0)
INSERT INTO Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 3,'用户管理',3,1,7,'','/BaseModule/BcUserInfo/BcUserInfoList.aspx','',1,0)
INSERT INTO Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 4,'组别管理',3,2,7,'','/BaseModule/BcGroup/BcGroupList.aspx','',1,0)
INSERT INTO Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 5,'用户角色权限',3,5,7,'','/BaseModule/BcPermission/BcPermissionList.aspx','',1,0)
INSERT INTO Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 6,'客户管理',1,1,0,'','','',1,0)
INSERT INTO Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 7,'用户管理',2,1,1,'','','',1,0)
INSERT INTO Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 8,'错误日志管理',3,2,11,'','/BaseModule/BcLogError/BcLogErrorList.aspx','',1,0)
INSERT INTO Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 9,'客户管理',2,1,6,'','/CustomerModule/CuCustomer/CuCustomerList.aspx','',1,0)
INSERT INTO Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 10,'自动编号管理',3,1,11,'','/BaseModule/BcAutoCode/BcAutoCodeList.aspx','',1,0)
INSERT INTO Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 11,'系统管理',2,5,1,'','','',1,0)
INSERT INTO Sys_Menu (MenuId,MenuName,MenuLevel,Seq,FatherNode,Description,PageUrl,UrlParameter,ActivityFlag,DeleteFlag)  values ( 12,'用户角色关系',3,4,7,'','/BaseModule/BcUserRole/BcUserRoleList.aspx','',1,0)
SET IDENTITY_INSERT  Sys_Menu OFF


INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcAutoCode/BcAutoCodeDetail.aspx','Bc_AutoCode')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcAutoCode/BcAutoCodeList.aspx','Bc_AutoCode')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcGroup/BcGroupDetail.aspx','Bc_Group')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcGroup/BcGroupList.aspx','Bc_Group')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcLogError/BcLogErrorDetail.aspx','Bc_ErrorLog')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcLogError/BcLogErrorList.aspx','Bc_ErrorLog')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcPermission/BcPermissionDetail.aspx','Bc_Permission')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcPermission/BcPermissionList.aspx','Bc_Permission')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcRole/BcRoleDetail.aspx','Bc_Role')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcRole/BcRoleList.aspx','Bc_Role')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcUserInfo/BcUserInfoDetail.aspx','Bc_User')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcUserInfo/BcUserInfoList.aspx','Bc_User')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcUserRole/BcUserRoleDetail.aspx','Bc_UserRole')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/BaseModule/BcUserRole/BcUserRoleList.aspx','Bc_UserRole')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/CustomerModule/CuCustomer/CuContactPersonDetail.aspx','Cu_Customer')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/CustomerModule/CuCustomer/CuContactPersonList.aspx','Cu_Customer')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/CustomerModule/CuCustomer/CuCustomerDetail.aspx','Cu_Customer')
INSERT INTO Sys_PageFuncPoint (PageUrl,FuncPointId)  values ( '/CustomerModule/CuCustomer/CuCustomerList.aspx','Cu_Customer')


INSERT INTO Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Account','Acc_Test','View')
INSERT INTO Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_AutoCode','Add,Delete,Edit,View')
INSERT INTO Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_ErrorLog','View')
INSERT INTO Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_Group','Add,Delete,Edit,View')
INSERT INTO Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_Permission','Add,Delete,View')
INSERT INTO Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_Role','Add,Delete,Edit,View')
INSERT INTO Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_User','View,Add,Edit,Delete')
INSERT INTO Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Base','Bc_UserRole','Add,Delete,View')
INSERT INTO Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Customer','Cu_Customer','Add,Delete,Edit,View')
INSERT INTO Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Inventory','In_Test','View')
INSERT INTO Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Manufacture','Mf_Test','View')
INSERT INTO Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Production','Pd_Test','View')
INSERT INTO Sys_Relation (ModuleId,FuncPointId,Permissions)  values ( 'Trade','Td_Test','View')


SET IDENTITY_INSERT  Bc_Group ON
INSERT INTO Bc_Group (GroupId,GroupName)  values (1, '销售部')
INSERT INTO Bc_Group (GroupId,GroupName)  values (2, '采购部')
INSERT INTO Bc_Group (GroupId,GroupName)  values (3, '生产部')
INSERT INTO Bc_Group (GroupId,GroupName)  values (4, '人事部')
INSERT INTO Bc_Group (GroupId,GroupName)  values (5, '市场部')
INSERT INTO Bc_Group (GroupId,GroupName)  values (6, '行政部')
SET IDENTITY_INSERT  Bc_Group OFF


SET IDENTITY_INSERT  Bc_Role ON
INSERT INTO Bc_Role (RoleId,RoleName)  values ( 1,'普通用户')
INSERT INTO Bc_Role (RoleId,RoleName)  values ( 2,'系统管理员')
SET IDENTITY_INSERT  Bc_Role OFF


SET IDENTITY_INSERT  Bc_UserInfo ON
INSERT INTO Bc_UserInfo (UserId,GroupId,UserAccount,UserName,Password,Sex,OfficePhone,MobilePhone,Email,ActivityFlag,DeleteFlag)  values ( 1,1,'admin','管理员','eedc08f62509c304',1,'','123','',1,0)
INSERT INTO Bc_UserInfo (UserId,GroupId,UserAccount,UserName,Password,Sex,OfficePhone,MobilePhone,Email,ActivityFlag,DeleteFlag)  values ( 2,2,'shi0090','石佳劼','1cef6c965a29cdbfadcc69046d4ddc21',1,'','123','',1,0)
INSERT INTO Bc_UserInfo (UserId,GroupId,UserAccount,UserName,Password,Sex,OfficePhone,MobilePhone,Email,ActivityFlag,DeleteFlag)  values ( 3,3,'hujingtao','胡锦涛','3fe4c3733606ea70',1,'123','123','123',1,0)
INSERT INTO Bc_UserInfo (UserId,GroupId,UserAccount,UserName,Password,Sex,OfficePhone,MobilePhone,Email,ActivityFlag,DeleteFlag)  values ( 4,4,'aaa','aaa','ef3a300883fb464f',1,'222','222','222',1,0)
INSERT INTO Bc_UserInfo (UserId,GroupId,UserAccount,UserName,Password,Sex,OfficePhone,MobilePhone,Email,ActivityFlag,DeleteFlag)  values ( 5,5,'bbb','bbb','628a02da7b5e4db0',1,'123','123','123',1,0)
INSERT INTO Bc_UserInfo (UserId,GroupId,UserAccount,UserName,Password,Sex,OfficePhone,MobilePhone,Email,ActivityFlag,DeleteFlag)  values ( 6,6,'ccc','ccc','083d2e81543235d5',1,'333','333','333',1,0)
SET IDENTITY_INSERT  Bc_UserInfo OFF


INSERT INTO Bc_UserRole (UserId,RoleId)  values ( 2,2)
INSERT INTO Bc_UserRole (UserId,RoleId)  values ( 3,2)


INSERT INTO Bc_Permission (UserRoleId,PermissionType,ModuleId,FuncPointId,Permissions)  values ( 2,1,'Base','Bc_AutoCode','Add,Delete,Edit,View')
INSERT INTO Bc_Permission (UserRoleId,PermissionType,ModuleId,FuncPointId,Permissions)  values ( 2,1,'Base','Bc_Group','Add,Delete,Edit,View')
INSERT INTO Bc_Permission (UserRoleId,PermissionType,ModuleId,FuncPointId,Permissions)  values ( 2,1,'Base','Bc_Permission','Add,Delete,View')
INSERT INTO Bc_Permission (UserRoleId,PermissionType,ModuleId,FuncPointId,Permissions)  values ( 2,1,'Base','Bc_Role','Add,Delete,Edit,View')
INSERT INTO Bc_Permission (UserRoleId,PermissionType,ModuleId,FuncPointId,Permissions)  values ( 2,1,'Base','Bc_User','View,Add,Edit,Delete')
INSERT INTO Bc_Permission (UserRoleId,PermissionType,ModuleId,FuncPointId,Permissions)  values ( 2,1,'Base','Bc_UserRole','Add,Delete,View')

SET IDENTITY_INSERT  Bc_AutoCode ON
INSERT INTO Bc_AutoCode (Id,Prefix,[DateFormat],FuncPointId,Digit,IsDefault,CurrentDate,CurrentCode) VALUES (1,'CM-','yyMMdd','Cu_Customer',4,1,'2016-01-21',1)
SET IDENTITY_INSERT  Bc_AutoCode ON

INSERT INTO Cu_Customer (CuId,CuName,District,[Address],Remark,ActivityFlag,DeleteFlag) VALUES ('CM-1601210001','习大大','北京','','',1,0)

