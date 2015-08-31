using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.SystemModule;
using Stonefw.Utility;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.SystemModule.SysMenu
{
    public partial class SysMenuDetail : BasePage
    {
        private SysMenuBiz _biz;

        private SysMenuBiz Biz
        {
            get { return _biz ?? (_biz = new SysMenuBiz()); }
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
                if (hdMenuId.Value == "-1")
                {
                    Biz.AddNewSysMenu(entity);
                }
                else
                {
                    Biz.UpdateSysMenu(entity, int.Parse(hdFatherNode.Value));
                }
                FatherQuery();
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message);
            }
        }

        protected void ddlFuncPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindddlPageUrlData(ddlFuncPoint.SelectedValue);
            txtMenuName.Text = ddlFuncPoint.SelectedItem.Text;
        }

        private void BindControlData()
        {
            var sysRelationList = new SysRelationBiz().GetSysRelationList();
            foreach (SysRelationEntity entity in sysRelationList)
            {
                ddlFuncPoint.Items.Add(new ListItem(entity.ModuleName + "-" + entity.FuncPointName, entity.FuncPointId));
            }
            ddlFuncPoint.Items.Insert(0, new ListItem("*请选择功能点*", "0"));

            ddlMenuTree.DataSource = Biz.GetSysMenuList();
            ddlMenuTree.DataValueField = "MenuId";
            ddlMenuTree.DataTextField = "MenuTreeName";
            ddlMenuTree.DataBind();
            ddlMenuTree.Items.Insert(0, new ListItem("*根目录*", "0"));
        }

        private void BindddlPageUrlData(string funcPointId)
        {
            ddlPageUrl.DataSource = new SysPageFuncPointBiz().GetSysPageFuncPointList(funcPointId);
            ddlPageUrl.DataValueField = "PageUrl";
            ddlPageUrl.DataTextField = "PageUrl";
            ddlPageUrl.DataBind();
        }

        private void FillFormData()
        {
            try
            {
                hdMenuId.Value = Request["menuid"];
                var entity = Biz.GetSysMenuEntity(int.Parse(hdMenuId.Value));
                if (entity != null)
                {
                    txtMenuName.Text = entity.MenuName;
                    ddlMenuTree.SelectedValue = entity.FatherNode.ToString();
                    txtMenuDescription.Text = entity.Description;
                    txtUrlParameter.Text = entity.UrlParameter;
                    cbMenuStatus.Checked = (bool)entity.ActivityFlag;
                    hdFatherNode.Value = entity.FatherNode.ToString();
                    txtModule.Text = entity.ModuleName;
                    ddlFuncPoint.SelectedValue = entity.FuncPointId;
                    BindddlPageUrlData(entity.FuncPointId);
                    ddlPageUrl.SelectedValue = entity.PageUrl;
                }
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }

        private SysMenuEntity PrepareFormData()
        {
            //校验参数的合法性
            txtMenuName.Text.InitValidation("菜单名称").NotEmpty().ShorterThan(25);
            txtMenuDescription.Text.InitValidation("菜单说明").ShorterThan(250);
            txtUrlParameter.Text.InitValidation("菜单参数").ShorterThan(250);

            var entity = new SysMenuEntity();
            entity.MenuId = int.Parse(hdMenuId.Value);
            entity.MenuName = txtMenuName.Text;
            entity.FatherNode = int.Parse(ddlMenuTree.SelectedValue);
            entity.Description = txtMenuDescription.Text;
            entity.UrlParameter = txtUrlParameter.Text;
            entity.PageUrl = ddlPageUrl.Items.Count > 0 ? ddlPageUrl.SelectedItem.Text : string.Empty;
            entity.ActivityFlag = cbMenuStatus.Checked;
            return entity;
        }
    }
}