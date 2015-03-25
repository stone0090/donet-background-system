namespace stonefw.GenCode
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cbDataTableList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbNameSpaceList = new System.Windows.Forms.ComboBox();
            this.btnManageNameSpace = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEntityName = new System.Windows.Forms.TextBox();
            this.btnCreateCodeFile = new System.Windows.Forms.Button();
            this.chkEntity = new System.Windows.Forms.CheckBox();
            this.chkBiz = new System.Windows.Forms.CheckBox();
            this.chkDao = new System.Windows.Forms.CheckBox();
            this.chkUi = new System.Windows.Forms.CheckBox();
            this.btnSeleteAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据表";
            // 
            // cbDataTableList
            // 
            this.cbDataTableList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDataTableList.FormattingEnabled = true;
            this.cbDataTableList.Location = new System.Drawing.Point(14, 24);
            this.cbDataTableList.Name = "cbDataTableList";
            this.cbDataTableList.Size = new System.Drawing.Size(448, 20);
            this.cbDataTableList.TabIndex = 1;
            this.cbDataTableList.SelectedIndexChanged += new System.EventHandler(this.cbDataTableList_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "命名空间";
            // 
            // cbNameSpaceList
            // 
            this.cbNameSpaceList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbNameSpaceList.FormattingEnabled = true;
            this.cbNameSpaceList.Location = new System.Drawing.Point(14, 62);
            this.cbNameSpaceList.Name = "cbNameSpaceList";
            this.cbNameSpaceList.Size = new System.Drawing.Size(347, 20);
            this.cbNameSpaceList.TabIndex = 1;
            // 
            // btnManageNameSpace
            // 
            this.btnManageNameSpace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageNameSpace.Location = new System.Drawing.Point(367, 60);
            this.btnManageNameSpace.Name = "btnManageNameSpace";
            this.btnManageNameSpace.Size = new System.Drawing.Size(95, 23);
            this.btnManageNameSpace.TabIndex = 2;
            this.btnManageNameSpace.Text = "管理命名空间";
            this.btnManageNameSpace.UseVisualStyleBackColor = true;
            this.btnManageNameSpace.Click += new System.EventHandler(this.btnManageNameSpace_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "类名称";
            // 
            // txtEntityName
            // 
            this.txtEntityName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEntityName.Location = new System.Drawing.Point(14, 101);
            this.txtEntityName.Name = "txtEntityName";
            this.txtEntityName.Size = new System.Drawing.Size(448, 21);
            this.txtEntityName.TabIndex = 3;
            // 
            // btnCreateCodeFile
            // 
            this.btnCreateCodeFile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCreateCodeFile.Location = new System.Drawing.Point(397, 128);
            this.btnCreateCodeFile.Name = "btnCreateCodeFile";
            this.btnCreateCodeFile.Size = new System.Drawing.Size(65, 23);
            this.btnCreateCodeFile.TabIndex = 4;
            this.btnCreateCodeFile.Text = "生成";
            this.btnCreateCodeFile.UseVisualStyleBackColor = true;
            this.btnCreateCodeFile.Click += new System.EventHandler(this.btnCreateCodeFile_Click);
            // 
            // chkEntity
            // 
            this.chkEntity.AutoSize = true;
            this.chkEntity.Location = new System.Drawing.Point(16, 132);
            this.chkEntity.Name = "chkEntity";
            this.chkEntity.Size = new System.Drawing.Size(84, 16);
            this.chkEntity.TabIndex = 5;
            this.chkEntity.Text = "创建Entity";
            this.chkEntity.UseVisualStyleBackColor = true;
            // 
            // chkBiz
            // 
            this.chkBiz.AutoSize = true;
            this.chkBiz.Location = new System.Drawing.Point(107, 132);
            this.chkBiz.Name = "chkBiz";
            this.chkBiz.Size = new System.Drawing.Size(66, 16);
            this.chkBiz.TabIndex = 6;
            this.chkBiz.Text = "创建Biz";
            this.chkBiz.UseVisualStyleBackColor = true;
            // 
            // chkDao
            // 
            this.chkDao.AutoSize = true;
            this.chkDao.Location = new System.Drawing.Point(180, 132);
            this.chkDao.Name = "chkDao";
            this.chkDao.Size = new System.Drawing.Size(66, 16);
            this.chkDao.TabIndex = 7;
            this.chkDao.Text = "创建Dao";
            this.chkDao.UseVisualStyleBackColor = true;
            // 
            // chkUi
            // 
            this.chkUi.AutoSize = true;
            this.chkUi.Location = new System.Drawing.Point(253, 132);
            this.chkUi.Name = "chkUi";
            this.chkUi.Size = new System.Drawing.Size(60, 16);
            this.chkUi.TabIndex = 8;
            this.chkUi.Text = "创建UI";
            this.chkUi.UseVisualStyleBackColor = true;
            // 
            // btnSeleteAll
            // 
            this.btnSeleteAll.Location = new System.Drawing.Point(325, 128);
            this.btnSeleteAll.Name = "btnSeleteAll";
            this.btnSeleteAll.Size = new System.Drawing.Size(65, 23);
            this.btnSeleteAll.TabIndex = 9;
            this.btnSeleteAll.Text = "全选";
            this.btnSeleteAll.UseVisualStyleBackColor = true;
            this.btnSeleteAll.Click += new System.EventHandler(this.btnSeleteAll_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 167);
            this.Controls.Add(this.btnSeleteAll);
            this.Controls.Add(this.chkUi);
            this.Controls.Add(this.chkDao);
            this.Controls.Add(this.chkBiz);
            this.Controls.Add(this.chkEntity);
            this.Controls.Add(this.btnCreateCodeFile);
            this.Controls.Add(this.txtEntityName);
            this.Controls.Add(this.btnManageNameSpace);
            this.Controls.Add(this.cbNameSpaceList);
            this.Controls.Add(this.cbDataTableList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "实体类创建工具";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDataTableList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbNameSpaceList;
        private System.Windows.Forms.Button btnManageNameSpace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEntityName;
        private System.Windows.Forms.Button btnCreateCodeFile;
        private System.Windows.Forms.CheckBox chkEntity;
        private System.Windows.Forms.CheckBox chkBiz;
        private System.Windows.Forms.CheckBox chkDao;
        private System.Windows.Forms.CheckBox chkUi;
        private System.Windows.Forms.Button btnSeleteAll;
    }
}

