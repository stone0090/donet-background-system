using System;
using System.Windows.Forms;

namespace stonefw.CodeGenerate
{
    public partial class NameSpaceManager : Form
    {
        public NameSpaceManager()
        {
            InitializeComponent();
        }

        private void NameSpaceManager_Load(object sender, EventArgs e)
        {
            ShowNameSpaceList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.txtKey.Text.Length == 0 || this.txtNameSpace.Text.Length == 0)
            {
                MessageBox.Show("请输入描述和命名空间！");
                return;
            }
            MyConfiguations.SaveNameSpace(this.txtKey.Text, this.txtNameSpace.Text);
            ShowNameSpaceList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.lbNameSpace.SelectedIndex < 0)
            {
                MessageBox.Show("请选择要删除的命名空间！");
                return;
            }
            string tmpValue = this.lbNameSpace.SelectedItem.ToString();
            string[] items = tmpValue.Split(':');
            MyConfiguations.DeleteNameSpace(items[0]);
            this.lbNameSpace.Items.RemoveAt(this.lbNameSpace.SelectedIndex);
            ShowNameSpaceList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.txtKey.Text = "";
            this.txtNameSpace.Text = "";
            this.txtKey.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.lbNameSpace.SelectedIndex < 0)
            {
                MessageBox.Show("请选择要编辑的命名空间！");
                return;
            }
            string tmpValue = this.lbNameSpace.SelectedItem.ToString();
            string[] items = tmpValue.Split(':');
            this.txtKey.Text = items[0];
            this.txtNameSpace.Text = items[1];
        }

        /// <summary>
        /// 显示命名空间列表
        /// </summary>
        private void ShowNameSpaceList()
        {
            this.lbNameSpace.Items.Clear();
            var nameSpaces = MyConfiguations.NameSpaceList;
            for (int i = 0; i < nameSpaces.Count; i++)
            {
                string loopKey = nameSpaces.Keys[i];
                string loopNameSpace = nameSpaces[loopKey];
                this.lbNameSpace.Items.Add(loopKey + ":" + loopNameSpace);
            }
        }
    }
}
