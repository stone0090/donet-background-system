using System;
using System.Collections;
using System.Web.UI.WebControls;
using Stonefw.Biz.SystemModule;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.SystemModule.SysMenu
{
    public partial class SysMenuSorting : BasePage
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
                if (string.IsNullOrEmpty(hdSeqValue.Value))
                    FatherQuery();

                var ht = new Hashtable();
                var seats = hdSeqValue.Value.Split('|');
                for (var i = 0; i < seats.Length; i++)
                {
                    ht.Add(seats[i], i + 1);
                }
                Biz.RecalSeq(ht);
                FatherQuery();
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("位置调整失败，原因：{0}", ex.Message);
            }
        }

        protected void ddlMenuTree_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillFormData();
        }

        private void BindControlData()
        {
            var menuList = Biz.GetSysMenuList();
            ddlMenuTree.DataSource = menuList;
            ddlMenuTree.DataValueField = "MenuId";
            ddlMenuTree.DataTextField = "MenuTreeName";
            ddlMenuTree.DataBind();
            ddlMenuTree.Items.Insert(0, new ListItem("根目录", "0"));
        }

        private void FillFormData()
        {
            var seatList = Biz.GetSysMenuListByFatherNode(int.Parse(ddlMenuTree.SelectedValue));
            lbMenuTree.DataSource = seatList;
            lbMenuTree.DataValueField = "MenuId";
            lbMenuTree.DataTextField = "MenuName";
            lbMenuTree.DataBind();
        }
    }
}