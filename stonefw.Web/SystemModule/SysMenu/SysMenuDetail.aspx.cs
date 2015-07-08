using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using stonefw.Biz.SystemModule;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysMenu
{
    public partial class SysMenuDetail : BasePage
    {
        private SysMenuBiz _biz;
        private SysMenuBiz Biz
        {
            get
            {
                return _biz ?? (_biz = new SysMenuBiz());
            }
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
                SysMenuEntity entity = PrepareFormData();
                if (this.hdMenuId.Value == "-1")
                {
                    Biz.AddNewSysMenu(entity);
                }
                else
                {
                    Biz.UpdateSysMenu(entity, int.Parse(this.hdFatherNode.Value));
                }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }
        protected void ddlFuncPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindddlPageUrlData(this.ddlFuncPoint.SelectedValue);
            this.txtMenuName.Text = this.ddlFuncPoint.SelectedItem.Text;
        }

        private void BindControlData()
        {
            this.ddlFuncPoint.DataSource = new SysFuncPointEnumBiz().GetSysFuncPointEnumList();
            this.ddlFuncPoint.DataValueField = "Name";
            this.ddlFuncPoint.DataTextField = "Description";
            this.ddlFuncPoint.DataBind();
            this.ddlFuncPoint.Items.Insert(0, new ListItem("*请选择功能点*", "0"));

            this.ddlMenuTree.DataSource = Biz.GetSysMenuList();
            this.ddlMenuTree.DataValueField = "MenuId";
            this.ddlMenuTree.DataTextField = "MenuTreeName";
            this.ddlMenuTree.DataBind();
            this.ddlMenuTree.Items.Insert(0, new ListItem("*根目录*", "0"));
        }

        private void BindddlPageUrlData(string funcPointId)
        {
            this.ddlPageUrl.DataSource = new SysPageFuncPointBiz().GetSysPageFuncPointList(funcPointId);
            this.ddlPageUrl.DataValueField = "PageUrl";
            this.ddlPageUrl.DataTextField = "PageUrl";
            this.ddlPageUrl.DataBind();
        }
        private void FillFormData()
        {
            try
            {
                this.hdMenuId.Value = Request["menuid"];
                SysMenuEntity entity = Biz.GetSysMenuEntity(int.Parse(this.hdMenuId.Value));
                if (entity != null)
                {
                    this.txtMenuName.Text = entity.MenuName;
                    this.ddlMenuTree.SelectedValue = entity.FatherNode.ToString();
                    this.txtMenuDescription.Text = entity.Description;
                    this.txtUrlParameter.Text = entity.UrlParameter;
                    this.cbMenuStatus.Checked = (bool)entity.ActivityFlag;
                    this.hdFatherNode.Value = entity.FatherNode.ToString();
                    this.txtModule.Text = entity.ModuleName;
                    this.ddlFuncPoint.SelectedValue = entity.FuncPointId;
                    BindddlPageUrlData(entity.FuncPointId);
                    this.ddlPageUrl.SelectedValue = entity.PageUrl;
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private SysMenuEntity PrepareFormData()
        {
            //校验参数的合法性
            this.txtMenuName.Text.InitValidation("菜单名称").NotEmpty().ShorterThan(25);
            this.txtMenuDescription.Text.InitValidation("菜单说明").ShorterThan(250);
            this.txtUrlParameter.Text.InitValidation("菜单参数").ShorterThan(250);

            var entity = new SysMenuEntity();
            entity.MenuId = int.Parse(this.hdMenuId.Value);
            entity.MenuName = this.txtMenuName.Text;
            entity.FatherNode = int.Parse(this.ddlMenuTree.SelectedValue);
            entity.Description = this.txtMenuDescription.Text;
            entity.UrlParameter = this.txtUrlParameter.Text;
            entity.PageUrl = this.ddlPageUrl.Items.Count > 0 ? this.ddlPageUrl.SelectedItem.Text : string.Empty;
            entity.ActivityFlag = this.cbMenuStatus.Checked;
            return entity;
        }

    }
}