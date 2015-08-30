using System;
using Stonefw.Biz.BaseModule;
using Stonefw.Entity.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcLogError
{
    public partial class BcLogErrorDetail : BasePage
    {
        private BcLogErrorBiz _biz;

        private BcLogErrorBiz Biz
        {
            get { return _biz ?? (_biz = new BcLogErrorBiz()); }
        }

        protected override bool InitPermission()
        {
            return LoadPermission(SysPermsPointEnum.View);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillFormData();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = PrepareFormData();
                if (txtId.Text == "-1")
                {
                    Biz.AddNewBcLogError(entity);
                }
                else
                {
                    Biz.UpdateBcLogError(entity);
                }
                FatherQuery();
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message);
            }
        }

        private void FillFormData()
        {
            try
            {
                txtId.Text = Request["id"];
                var entity = Biz.GetSingleBcLogError(int.Parse(txtId.Text));
                if (entity != null)
                {
                    txtId.Text = entity.Id.ToString();
                    txtUserId.Text = entity.UserId.ToString();
                    txtUserName.Text = entity.UserName;
                    txtOpUrl.Text = entity.OpUrl;
                    txtOpTime.Text = entity.OpTime.ToString();
                    txtOpHostAddress.Text = entity.OpHostAddress;
                    txtOpHostName.Text = entity.OpHostName;
                    txtOpUserAgent.Text = entity.OpUserAgent;
                    txtOpQueryString.Text = entity.OpQueryString;
                    txtOpHttpMethod.Text = entity.OpHttpMethod;
                    txtMessage.Text = entity.Message;
                }
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }

        private BcLogErrorEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            var entity = new BcLogErrorEntity();
            entity.Id = int.Parse(txtId.Text);
            entity.UserId = int.Parse(txtUserId.Text);
            entity.UserName = txtUserName.Text;
            entity.OpUrl = txtOpUrl.Text;
            entity.OpTime = DateTime.Parse(txtOpTime.Text);
            entity.OpHostAddress = txtOpHostAddress.Text;
            entity.OpHostName = txtOpHostName.Text;
            entity.OpUserAgent = txtOpUserAgent.Text;
            entity.OpQueryString = txtOpQueryString.Text;
            entity.OpHttpMethod = txtOpHttpMethod.Text;
            entity.Message = txtMessage.Text;
            return entity;
        }
    }
}