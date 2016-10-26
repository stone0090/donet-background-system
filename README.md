
博主在业余时间开发了一个简单的后台管理系统，其中用到了 jQuery EasyUI 框架，上次分享过系统布局，参考文章：[jQuery EasyUI 后台管理系统布局分享](http://shijiajie.com/2014/10/27/frontend-jquery-easyui-layout-demo/)，目前已完成系统的整体框架的搭建，再次分享给大家。

- [系统演示地址](http://stonefw.shijiajie.com)，账户名和密码均为：admin。
- [GitHub Clone 地址](https://github.com/stone0090/stonefw)
- [系统源码 百度网盘 下载地址](http://pan.baidu.com/s/1qX0zAeK)

### 基础功能介绍：
- 菜单管理、用户管理、组别管理、角色管理、权限管理、错误日志

### 特色功能介绍：
- 独创的EntitySql语法，极大的简化了Sql操作。
- 灵活的权限设计，任意页面、任意按钮的权限都可随意控制。
- 高效的代码生成器，指定任意数据表，便可生成CRUD基础代码。

### 系统部署方法：
- 在 Sql Server 中新建一个数据库 `STONEFW`。
- 在 `STONEFW` 数据库中执行脚本 `stonefw.sql`。
- 在 IIS 中新建一个网站 `stonefw`，物理路径指向 stonefw 文件夹。
- 修改 stonefw 文件夹中 `Web.config` 中的链接字符串。
- 网站部署完毕，请启动您的网站。

### 系统演示截图：
<!-- more --> 
![](http://7xkhp9.com1.z0.glb.clouddn.com/blog/donet-opensource-stonefw-introduction/1.png)

本系统仅完成了整体框架，如果大家感兴趣，可以通过以下方式联系我，我再继续更新、维护这个项目。
- 在文章[《DoNet开源项目-基于jQuery EasyUI的后台管理系统》](http://shijiajie.com/2015/08/30/donet-opensource-stonefw-introduction/#ds-thread)的评论区给我留言。
- 发送邮件到 <a href="mailto:stone0090@hotmail.com">stone0090@hotmail.com</a> 给我留言。
- 如有bug可到 [issues页面](https://github.com/stone0090/stonefw/issues/new) 进行反馈。

## LICENSE

This project is released under the [GPL](https://github.com/stone0090/donet-background-system/blob/master/LICENSE).