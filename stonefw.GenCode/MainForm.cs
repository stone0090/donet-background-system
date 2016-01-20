using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stonefw.CodeGenerate.SqlServer;

namespace Stonefw.CodeGenerate
{
    public partial class MainForm : Form
    {
        #region 全局变量

        private string _lastSelectFolder = null;

        #endregion

        #region 页面事件

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ShowDataTables();
            ShowNameSpaceList();
        }

        private void btnManageNameSpace_Click(object sender, EventArgs e)
        {
            NameSpaceManager nsm = new NameSpaceManager();
            nsm.ShowDialog();
            ShowNameSpaceList();
        }

        private void btnSeleteAll_Click(object sender, EventArgs e)
        {
            if (this.btnSeleteAll.Text == "全选")
            {
                this.chkEntity.Checked = true;
                this.chkBiz.Checked = true;
                this.chkUi.Checked = true;
                this.btnSeleteAll.Text = "反选";
            }
            else
            {
                this.chkEntity.Checked = false;
                this.chkBiz.Checked = false;
                this.chkUi.Checked = false;
                this.btnSeleteAll.Text = "全选";
            }
        }

        private void btnCreateCodeFile_Click(object sender, EventArgs e)
        {
            if (this.cbDataTableList.Text.Length == 0)
            {
                MessageBox.Show("请选择数据表！");
                return;
            }
            if (this.cbNameSpaceList.Text.Length == 0)
            {
                MessageBox.Show("请选择命名空间！");
                return;
            }
            if (this.txtEntityName.Text.Length == 0)
            {
                MessageBox.Show("请输入类的名称！");
                return;
            }

            var fbd = new FolderBrowserDialog {Description = "保存文件的目录"};
            if (_lastSelectFolder != null)
            {
                fbd.SelectedPath = _lastSelectFolder;
            }
            fbd.ShowDialog();

            string folder = fbd.SelectedPath;
            _lastSelectFolder = folder;

            if (!folder.EndsWith("\\"))
            {
                folder += "\\";
            }

            folder += "code\\";

            if (Directory.Exists(folder))
            {
                var result = MessageBox.Show("目录已存在，是否清空目录中的文件？", "", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Directory.Delete(folder, true);
                    Directory.CreateDirectory(folder);
                }
            }

            if (this.chkEntity.Checked) CreateCodeOfEntity(folder);
            if (this.chkBiz.Checked) CreateCodeOfBiz(folder);
            if (this.chkUi.Checked) CreateCodeOfUi(folder);

            MessageBox.Show("生成成功！");
        }

        private void cbDataTableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtEntityName.Text = this.cbDataTableList.Text;
        }

        #endregion

        #region 生成代码

        //AutoGenEntity
        private void CreateCodeOfEntity(string folder)
        {
            var ns = this.cbNameSpaceList.Text.Split(':')[1];
            string fileOfCodeCreator = folder + ns + ".entity\\" + this.txtEntityName.Text + "Entity.cs";
            CreateDirectory(fileOfCodeCreator);

            StringBuilder codeBuffer = new StringBuilder(100000);
            codeBuffer.AppendLine("using System;");
            codeBuffer.AppendLine("using stonefw.Utility.EntitySql.Attribute;");
            codeBuffer.AppendLine("using stonefw.Utility.EntitySql.Entity;");
            codeBuffer.AppendLine("");
            codeBuffer.Append("namespace ").Append(ns).AppendLine(".Entity");
            codeBuffer.AppendLine("{"); //命名空间
            codeBuffer.AppendLine("    [Serializable]");
            codeBuffer.Append("    [Table(\"").Append(this.cbDataTableList.Text).AppendLine("\")]");
            codeBuffer.Append("    public class ").Append(this.txtEntityName.Text).AppendLine("Entity : BaseEntity");
            codeBuffer.AppendLine("    {"); //命名空间

            var columns = ColumnEnumerator.GetColumnsOfTable(this.cbDataTableList.Text);
            for (int i = 0; i < columns.Length; i++)
            {
                string objType = GetObjectTypeOfDBColumnForCode(columns[i].ColumnType);
                if (objType != "string")
                    objType += "?";

                string dbType = GetDBTypeFromRawType(columns[i].ColumnType);
                codeBuffer.AppendLine(string.Format("        [Field(\"{0}\")]", columns[i].ColumnName));
                codeBuffer.Append("        public ").Append(objType).Append(" ").Append(columns[i].ColumnName);
                codeBuffer.AppendLine(" { get; set; }");
            }

            codeBuffer.AppendLine("    }"); //Class
            codeBuffer.AppendLine("}"); //命名空间

            File.WriteAllText(fileOfCodeCreator, codeBuffer.ToString());
        }

        //AutoGenBiz
        private void CreateCodeOfBiz(string folder)
        {
            var n = this.txtEntityName.Text;
            var ns = this.cbNameSpaceList.Text.Split(':')[1];
            var columns = ColumnEnumerator.GetColumnsOfTable(this.cbDataTableList.Text);
            string identityColumns = ColumnEnumerator.GetIdentityColumn(this.cbDataTableList.Text);
            string[] primaryKeyColumns = ColumnEnumerator.GetPrimaryKeyColumns(this.cbDataTableList.Text);

            string fileName = folder + ns + ".biz\\" + this.txtEntityName.Text + "Biz.cs";
            CreateDirectory(fileName);

            var codeBuffer = new StringBuilder(100000);
            codeBuffer.AppendLine("using System;");
            codeBuffer.AppendLine("using System.Collections;");
            codeBuffer.AppendLine("using System.Collections.Generic;");
            codeBuffer.AppendLine("using System.Data;");
            codeBuffer.AppendLine("using System.Data.Common;");
            codeBuffer.AppendLine("using System.Linq;");
            codeBuffer.AppendLine("using System.Text;");
            codeBuffer.AppendLine("using stonefw.Entity;");
            codeBuffer.AppendLine("using stonefw.Entity.Enum;");
            codeBuffer.AppendLine("using stonefw.Entity.Extension;");
            codeBuffer.AppendLine("using stonefw.Entity.SystemModule;");
            codeBuffer.AppendLine("using stonefw.Utility;");
            codeBuffer.AppendLine("using stonefw.Utility.EntitySql;");
            codeBuffer.AppendLine("");
            codeBuffer.Append("namespace ").Append(ns).AppendLine(".Biz");
            codeBuffer.AppendLine("{");
            codeBuffer.Append("    public class ").Append(this.txtEntityName.Text).AppendLine("Biz ");
            codeBuffer.AppendLine("    {");

            //删除
            string strDeleteArg1 = "";
            string strDeleteArg2 = "";
            for (int i = 0; i < primaryKeyColumns.Length; i++)
            {
                var type =
                    GetObjectTypeOfDBColumnForCode(
                        columns.Where(m => m.ColumnName == primaryKeyColumns[i]).ToList()[0].ColumnType);
                strDeleteArg1 += string.Format("{0} {1},", type, FirstCharLower(primaryKeyColumns[i]));
                strDeleteArg2 += string.Format("{0} = {1},", primaryKeyColumns[i], FirstCharLower(primaryKeyColumns[i]));
            }
            strDeleteArg1 = strDeleteArg1.Substring(0, strDeleteArg1.Length - 1);
            strDeleteArg2 = strDeleteArg2.Substring(0, strDeleteArg2.Length - 1);

            codeBuffer.Append("        public void Delete").Append(n).Append("(").Append(strDeleteArg1).AppendLine(")");
            codeBuffer.AppendLine("        {");
            codeBuffer.Append("            ")
                .Append(n)
                .Append("Entity entity = new ")
                .Append(n)
                .Append("Entity() { ")
                .Append(strDeleteArg2)
                .AppendLine(" };");
            codeBuffer.AppendLine("            entity.Delete();");
            codeBuffer.AppendLine("        }");

            //新增
            codeBuffer.Append("        public void AddNew").Append(n).Append("(").Append(n).AppendLine("Entity entity)");
            codeBuffer.AppendLine("        {");
            //如果有自增长的字段，新增时设置为null
            if (!string.IsNullOrEmpty(identityColumns))
                codeBuffer.Append("            entity.").Append(identityColumns).AppendLine(" = null;");
            codeBuffer.AppendLine("            entity.Insert();");
            codeBuffer.AppendLine("        }");

            //更新
            codeBuffer.Append("        public void Update").Append(n).Append("(").Append(n).AppendLine("Entity entity)");
            codeBuffer.AppendLine("        {");
            codeBuffer.AppendLine("            entity.Update();");
            codeBuffer.AppendLine("        }");

            string strQueryArg1 = "";
            string strQueryArg2 = "";
            for (int i = 0; i < primaryKeyColumns.Length; i++)
            {
                var type =
                    GetObjectTypeOfDBColumnForCode(
                        columns.Where(m => m.ColumnName == primaryKeyColumns[i]).ToList()[0].ColumnType);
                strQueryArg1 += string.Format("{0} {1},", type, FirstCharLower(primaryKeyColumns[i]));
                strQueryArg2 += string.Format("n.{0} == {1}&&", primaryKeyColumns[i],
                    FirstCharLower(primaryKeyColumns[i]));
            }
            strQueryArg1 = strQueryArg1.Substring(0, strQueryArg1.Length - 1);
            strQueryArg2 = strQueryArg2.Substring(0, strQueryArg2.Length - 2);

            //查询
            codeBuffer.Append("        public ")
                .Append(n)
                .Append("Entity Get")
                .Append(n)
                .Append("Entity(")
                .Append(strQueryArg1)
                .AppendLine(")");
            codeBuffer.AppendLine("        {");
            codeBuffer.Append("            return EntityExecution.SelectOne<")
                .Append(n)
                .Append("Entity>(n => ")
                .Append(strQueryArg2)
                .AppendLine(");");
            codeBuffer.AppendLine("        }");

            //GetList
            codeBuffer.Append("        public List<").Append(n).Append("Entity> Get").Append(n).AppendLine("List()");
            codeBuffer.AppendLine("        {");
            codeBuffer.Append("            return EntityExecution.SelectAll<").Append(n).AppendLine("Entity>();");
            codeBuffer.AppendLine("        }");

            codeBuffer.AppendLine("");
            codeBuffer.AppendLine("        #region 扩展方法");
            codeBuffer.AppendLine("");

            codeBuffer.AppendLine("        private Database _db; ");
            codeBuffer.AppendLine("        private Database Db ");
            codeBuffer.AppendLine("        {");
            codeBuffer.AppendLine("            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }");
            codeBuffer.AppendLine("        }");
            codeBuffer.AppendLine("");
            codeBuffer.AppendLine("        #endregion");

            codeBuffer.AppendLine("    }"); //Class
            codeBuffer.AppendLine("}"); //命名空间

            File.WriteAllText(fileName, codeBuffer.ToString());
        }

        //AutoGenUi
        private void CreateCodeOfUi(string folder)
        {
            var ns = this.cbNameSpaceList.Text.Split(':')[1];
            string fileOfListAspx = folder + ns + ".web\\" + this.txtEntityName.Text + "List.aspx";
            string fileOfListAspxCs = folder + ns + ".web\\" + this.txtEntityName.Text + "List.aspx.cs";
            string fileOfListAspxDesignerCs = folder + ns + ".web\\" + this.txtEntityName.Text + "List.aspx.designer.cs";
            string fileOfDetailAspx = folder + ns + ".web\\" + this.txtEntityName.Text + "Detail.aspx";
            string fileOfDetailAspxCs = folder + ns + ".web\\" + this.txtEntityName.Text + "Detail.aspx.cs";
            string fileOfDetailAspxDesignerCs = folder + ns + ".web\\" + this.txtEntityName.Text +
                                                "Detail.aspx.designer.cs";
            CreateDirectory(fileOfListAspx);
            CreateCodeOfListAspx(fileOfListAspx);
            CreateCodeOfListAspxCs(fileOfListAspxCs);
            CreateCodeOfListAspxDesignerCs(fileOfListAspxDesignerCs);
            CreateCodeOfDetailAspx(fileOfDetailAspx);
            CreateCodeOfDetailAspxCs(fileOfDetailAspxCs);
            CreateCodeOfDetailAspxDesignerCs(fileOfDetailAspxDesignerCs);
        }

        private void CreateCodeOfListAspx(string fileName)
        {
            var n = this.txtEntityName.Text;
            var ns = this.cbNameSpaceList.Text.Split(':')[1];
            var columns = ColumnEnumerator.GetColumnsOfTable(this.cbDataTableList.Text);
            string identityColumns = ColumnEnumerator.GetIdentityColumn(this.cbDataTableList.Text);
            string[] primaryKeyColumns = ColumnEnumerator.GetPrimaryKeyColumns(this.cbDataTableList.Text);

            //").Append(n).Append("
            StringBuilder codeBuffer = new StringBuilder(100000);
            codeBuffer.Append("<%@ Page Language=\"C#\" AutoEventWireup=\"true\" CodeBehind=\"")
                .Append(n)
                .Append("List.aspx.cs\" Inherits=\"")
                .Append(ns)
                .Append(".web.")
                .Append(n)
                .AppendLine("List\" %>");
            codeBuffer.AppendLine("");
            codeBuffer.AppendLine("<!DOCTYPE html>");
            codeBuffer.AppendLine("<html>");
            codeBuffer.AppendLine("<head runat=\"server\">");
            codeBuffer.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            codeBuffer.AppendLine("<title></title>");
            codeBuffer.AppendLine("</head>");
            codeBuffer.AppendLine("<body>");
            codeBuffer.AppendLine("<form id=\"form1\" runat=\"server\">"); //form
            codeBuffer.AppendLine("<div class=\"query\"><table><tr><td>");
            codeBuffer.AppendLine(
                "<asp:LinkButton runat=\"server\" ID=\"btnQuery\" Text=\"查询\" class=\"easyui-linkbutton\" data-options=\"iconCls:'icon-search'\" OnClick=\"btnQuery_Click\" ></asp:LinkButton>");

            //新增
            string strAddArg = primaryKeyColumns.Aggregate(string.Empty,
                (current, primaryKey) => current + primaryKey.Replace("_", "").ToLower() + "=-1&");
            strAddArg = strAddArg.Substring(0, strAddArg.Length - 1);
            codeBuffer.Append(
                "<asp:LinkButton runat=\"server\" ID=\"btnAddNew\" Text=\"新增\" class=\"easyui-linkbutton\" data-options=\"iconCls:'icon-add'\" OnClientClick=\"return showDialog('新增', '")
                .Append(n)
                .Append("Detail.aspx?")
                .Append(strAddArg)
                .AppendLine("',this);\">新增</asp:LinkButton>");

            codeBuffer.AppendLine("</td></tr></table></div>");
            codeBuffer.Append("<asp:GridView ID=\"gv")
                .Append(n)
                .Append(
                    "\" runat=\"server\" AutoGenerateColumns=\"False\" AllowPaging=\"True\" Width=\"100%\" CssClass=\"gridview\"");
            codeBuffer.Append("OnRowCommand=\"gv")
                .Append(n)
                .Append("_RowCommand\" OnPageIndexChanged=\"gv")
                .Append(n)
                .Append("_PageIndexChanged\" OnPageIndexChanging=\"gv")
                .Append(n)
                .Append("_PageIndexChanging\" >");
            codeBuffer.AppendLine("<HeaderStyle HorizontalAlign=\"Center\"></HeaderStyle>");
            codeBuffer.AppendLine("<RowStyle HorizontalAlign=\"Center\"></RowStyle>");
            codeBuffer.AppendLine("<Columns>");

            //删除
            codeBuffer.AppendLine("<asp:TemplateField HeaderText=\"删除\" ItemStyle-Width=\"45px\" >");
            codeBuffer.AppendLine("<ItemTemplate>");
            string strDeleteArg = primaryKeyColumns.Aggregate(string.Empty,
                (current, primaryKey) => current + string.Format("Eval(\"{0}\")+\"|\"+", primaryKey));
            strDeleteArg = strDeleteArg.Substring(0, strDeleteArg.Length - 5);
            codeBuffer.Append(
                "<asp:LinkButton class=\"easyui-linkbutton\" runat=\"server\" CommandName=\"Row_Delete\" CommandArgument='<%# ")
                .Append(strDeleteArg)
                .AppendLine(" %>' OnClientClick=\"return deleteWarning(this);\">删除</asp:LinkButton>");
            codeBuffer.AppendLine("</ItemTemplate>");
            codeBuffer.AppendLine("</asp:TemplateField>");

            //修改
            codeBuffer.AppendLine("<asp:TemplateField HeaderText=\"修改\" ItemStyle-Width=\"45px\" >");
            codeBuffer.AppendLine("<ItemTemplate>");
            string strEditArg = primaryKeyColumns.Aggregate(string.Empty,
                (current, primaryKey) =>
                    current + string.Format(primaryKey.Replace("_", "").ToLower() + "=\"+Eval(\"{0}\")+\"&", primaryKey));
            strEditArg = strEditArg.Substring(0, strEditArg.Length - 1);
            codeBuffer.Append("<a href=\"#\" class=\"easyui-linkbutton\" onclick='<%# \"showDialog(\\\"修改\\\", \\\"")
                .Append(n)
                .Append("Detail.aspx?")
                .Append(strEditArg)
                .AppendLine("\\\");\" %>'>修改</a>");
            codeBuffer.AppendLine("</ItemTemplate>");
            codeBuffer.AppendLine("</asp:TemplateField>");

            foreach (SqlServerColumn column in columns)
            {
                codeBuffer.AppendLine(string.Format("<asp:BoundField DataField=\"{0}\" HeaderText=\"{0}\" />",
                    column.ColumnName));
            }

            codeBuffer.AppendLine("</Columns>");
            codeBuffer.AppendLine("</asp:GridView>");
            codeBuffer.AppendLine("<div class=\"error\"><asp:Label ID=\"lMessage\" runat=\"server\"></asp:Label></div>");
            codeBuffer.AppendLine("<div id=\"dlg\" class=\"easyui-dialog\" data-options=\"closed:'false'\"></div>");
            codeBuffer.AppendLine("</form>"); //form
            codeBuffer.AppendLine("</body>");
            codeBuffer.AppendLine("</html>");

            File.WriteAllText(fileName, codeBuffer.ToString());
        }

        private void CreateCodeOfListAspxCs(string fileName)
        {
            var n = this.txtEntityName.Text;
            var ns = this.cbNameSpaceList.Text.Split(':')[1];
            var columns = ColumnEnumerator.GetColumnsOfTable(this.cbDataTableList.Text);
            string identityColumns = ColumnEnumerator.GetIdentityColumn(this.cbDataTableList.Text);
            string[] primaryKeyColumns = ColumnEnumerator.GetPrimaryKeyColumns(this.cbDataTableList.Text);

            StringBuilder codeBuffer = new StringBuilder(100000);
            codeBuffer.AppendLine("using System;");
            codeBuffer.AppendLine("using System.Collections.Generic;");
            codeBuffer.AppendLine("using System.Linq;");
            codeBuffer.AppendLine("using System.Web;");
            codeBuffer.AppendLine("using System.Web.UI;");
            codeBuffer.AppendLine("using System.Web.UI.WebControls;");
            codeBuffer.AppendLine("using stonefw.Biz;");
            codeBuffer.AppendLine("using stonefw.Entity;");
            codeBuffer.AppendLine("using stonefw.Entity.Enum;");
            codeBuffer.AppendLine("using stonefw.Utility;");
            codeBuffer.AppendLine("using stonefw.Web.Utility.BaseClass;");

            codeBuffer.AppendLine("");
            codeBuffer.AppendLine("namespace " + ns + ".web");
            codeBuffer.AppendLine("{");
            codeBuffer.Append("public partial class ").Append(n).AppendLine("List : BasePage");
            codeBuffer.AppendLine("{");
            codeBuffer.Append("private ").Append(n).AppendLine("Biz _biz;");
            codeBuffer.Append("private ").Append(n).AppendLine("Biz Biz");
            codeBuffer.Append("{ get { return _biz ?? (_biz = new ").Append(n).AppendLine("Biz()); }");
            codeBuffer.AppendLine("}");
            codeBuffer.AppendLine("");
            codeBuffer.AppendLine("protected override bool InitPermission(){");
            codeBuffer.AppendLine("this.btnAddNew.Visible = LoadPermission(SysPermsPointEnum.Add);");
            codeBuffer.Append("this.gv")
                .Append(n)
                .AppendLine(".Columns[0].Visible = LoadPermission(SysPermsPointEnum.Delete);");
            codeBuffer.Append("this.gv")
                .Append(n)
                .AppendLine(".Columns[1].Visible = LoadPermission(SysPermsPointEnum.Edit);");
            codeBuffer.AppendLine("return LoadPermission(SysPermsPointEnum.View);");
            codeBuffer.AppendLine("}");
            codeBuffer.AppendLine("");
            codeBuffer.AppendLine("protected void Page_Load(object sender, EventArgs e)");
            codeBuffer.AppendLine("{if (!IsPostBack)");
            codeBuffer.AppendLine("BindData();");
            codeBuffer.AppendLine("}");
            codeBuffer.AppendLine("protected void btnQuery_Click(object sender, EventArgs e){BindData(); ");
            codeBuffer.AppendLine("this.lMessage.Text = \"执行成功！\";");
            codeBuffer.AppendLine("}");
            codeBuffer.Append("protected void gv")
                .Append(n)
                .AppendLine("_RowCommand(object sender, GridViewCommandEventArgs e){");
            codeBuffer.AppendLine("if (e.CommandName == \"Row_Delete\"){");
            codeBuffer.AppendLine("string[] arg = e.CommandArgument.ToString().Split('|');");

            //删除
            string strDeleteArg = "";
            for (int i = 0; i < primaryKeyColumns.Length; i++)
            {
                var type =
                    GetObjectTypeOfDBColumnForCode(
                        columns.Where(m => m.ColumnName == primaryKeyColumns[i]).ToList()[0].ColumnType);
                if (type == "int")
                    strDeleteArg += string.Format("{0}.Parse(arg[{1}]),", type, i);
                else
                    strDeleteArg += string.Format("arg[{0}],", i);
            }
            strDeleteArg = strDeleteArg.Substring(0, strDeleteArg.Length - 1);
            codeBuffer.Append("Biz.Delete").Append(n).Append("(").Append(strDeleteArg).AppendLine(");");

            codeBuffer.AppendLine("BindData();}}");
            codeBuffer.Append("protected void gv")
                .Append(n)
                .AppendLine("_PageIndexChanged(object sender, EventArgs e){BindData();");
            codeBuffer.AppendLine("}");
            codeBuffer.Append("protected void gv")
                .Append(n)
                .AppendLine("_PageIndexChanging(object sender, GridViewPageEventArgs e)");
            codeBuffer.Append("{this.gv").Append(n).AppendLine(".PageIndex = e.NewPageIndex;");
            codeBuffer.AppendLine("}");
            codeBuffer.AppendLine("");
            codeBuffer.AppendLine("private void BindData()");
            codeBuffer.AppendLine("{");
            codeBuffer.Append("gv")
                .Append(n)
                .AppendLine(".PageSize = int.Parse(base.SysGlobalSetting.GridViewPageSize);");
            codeBuffer.Append("gv").Append(n).Append(".DataSource = Biz.Get").Append(n).AppendLine("List();");
            codeBuffer.Append("gv").Append(n).AppendLine(".DataBind();");
            codeBuffer.AppendLine("}}}");
            File.WriteAllText(fileName, codeBuffer.ToString());
        }

        private void CreateCodeOfListAspxDesignerCs(string fileName)
        {
            StringBuilder codeBuffer = new StringBuilder(100000);
            File.WriteAllText(fileName, codeBuffer.ToString());
        }

        private void CreateCodeOfDetailAspx(string fileName)
        {
            var n = this.txtEntityName.Text;
            var ns = this.cbNameSpaceList.Text.Split(':')[1];
            var columns = ColumnEnumerator.GetColumnsOfTable(this.cbDataTableList.Text);
            string identityColumns = ColumnEnumerator.GetIdentityColumn(this.cbDataTableList.Text);
            string[] primaryKeyColumns = ColumnEnumerator.GetPrimaryKeyColumns(this.cbDataTableList.Text);

            //").Append(n).Append("
            StringBuilder codeBuffer = new StringBuilder(100000);
            codeBuffer.Append("<%@ Page Language=\"C#\" AutoEventWireup=\"true\" CodeBehind=\"")
                .Append(n)
                .Append("Detail.aspx.cs\" Inherits=\"")
                .Append(ns)
                .Append(".web.")
                .Append(n)
                .AppendLine("Detail\" %>");
            codeBuffer.AppendLine("");
            codeBuffer.AppendLine("<!DOCTYPE html>");
            codeBuffer.AppendLine("<html>");
            codeBuffer.AppendLine("<head runat=\"server\">");
            codeBuffer.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            codeBuffer.AppendLine("<title></title>");
            codeBuffer.AppendLine("</head>");
            codeBuffer.AppendLine("<body>");
            codeBuffer.AppendLine("<form id=\"form1\" runat=\"server\">");

            foreach (string column in primaryKeyColumns)
            {
                codeBuffer.AppendLine(string.Format("<asp:HiddenField runat=\"server\" ID=\"{0}\" />",
                    FirstCharLower("hd" + column)));
            }

            codeBuffer.AppendLine("<div class=\"form\">");
            codeBuffer.AppendLine("<table cellpadding=\"5\">");

            foreach (var column in columns)
            {
                string isReadonly = (column.ColumnName == identityColumns) ? "readonly=\"True\"" : string.Empty;
                codeBuffer.AppendLine("<tr>");
                codeBuffer.AppendLine(string.Format("<td>{0}：</td>", column.ColumnName));
                switch (GetObjectTypeOfDBColumnForCode(column.ColumnType))
                {
                    case "string":
                        codeBuffer.AppendLine(
                            string.Format(
                                "<td><asp:TextBox ID=\"{0}\" runat=\"server\" MaxLength=\"{1}\" {2}  class=\"easyui-textbox\" data-options=\"required:true\"></asp:TextBox></td>",
                                FirstCharLower("txt" + column.ColumnName), column.ColumnSize/2, isReadonly));
                        break;
                    case "DateTime":
                        codeBuffer.AppendLine(
                            string.Format(
                                "<td><asp:TextBox ID=\"{0}\" runat=\"server\" class=\"easyui-textbox\" data-options=\"required:true\"></asp:TextBox></td>",
                                FirstCharLower("txt" + column.ColumnName)));
                        break;
                    case "int":
                    case "short":
                    case "long":
                    case "decimal":
                    case "double":
                    case "byte":
                        codeBuffer.AppendLine(
                            string.Format(
                                "<td><asp:TextBox ID=\"{0}\" runat=\"server\" {1} class=\"easyui-textbox\" data-options=\"required:true\"></asp:TextBox></td>",
                                FirstCharLower("txt" + column.ColumnName), isReadonly));
                        break;
                    case "bool":
                        codeBuffer.AppendLine("<td>");
                        codeBuffer.AppendLine(string.Format(
                            "<asp:CheckBox runat=\"server\" ID=\"{0}\" Text=\"{1}\" />",
                            FirstCharLower("cb" + column.ColumnName), column.ColumnName));
                        codeBuffer.AppendLine("</td>");
                        break;
                }
                codeBuffer.AppendLine("</tr>");
            }

            codeBuffer.AppendLine("</table>");
            codeBuffer.AppendLine("<div class=\"error\"><asp:Label ID=\"lMessage\" runat=\"server\"></asp:Label></div>");
            codeBuffer.AppendLine("<div>");
            codeBuffer.AppendLine(
                "<asp:LinkButton ID=\"btnSave\" runat=\"server\" class=\"easyui-linkbutton\" data-options=\"iconCls:'icon-ok'\" OnClientClick=\"return saveForm(this);\" OnClick=\"btnSave_Click\">保存</asp:LinkButton>");
            codeBuffer.AppendLine(
                "<a class=\"easyui-linkbutton\" data-options=\"iconCls:'icon-cancel'\" href=\"#\" onclick=\"window.parent.closeDialog();\">取消</a>");
            codeBuffer.AppendLine("</div>");
            codeBuffer.AppendLine("</div>");
            codeBuffer.AppendLine("</form>");
            codeBuffer.AppendLine("</body>");
            codeBuffer.AppendLine("</html>");
            File.WriteAllText(fileName, codeBuffer.ToString());
        }

        private void CreateCodeOfDetailAspxCs(string fileName)
        {
            var n = this.txtEntityName.Text;
            var ns = this.cbNameSpaceList.Text.Split(':')[1];
            var columns = ColumnEnumerator.GetColumnsOfTable(this.cbDataTableList.Text);
            string identityColumns = ColumnEnumerator.GetIdentityColumn(this.cbDataTableList.Text);
            string[] primaryKeyColumns = ColumnEnumerator.GetPrimaryKeyColumns(this.cbDataTableList.Text);

            StringBuilder codeBuffer = new StringBuilder(100000);
            codeBuffer.AppendLine("using System;");
            codeBuffer.AppendLine("using System.Collections.Generic;");
            codeBuffer.AppendLine("using System.Linq;");
            codeBuffer.AppendLine("using System.Web;");
            codeBuffer.AppendLine("using System.Web.UI;");
            codeBuffer.AppendLine("using System.Web.UI.WebControls;");
            codeBuffer.AppendLine("using stonefw.Biz;");
            codeBuffer.AppendLine("using stonefw.Entity;");
            codeBuffer.AppendLine("using stonefw.Entity.Enum;");
            codeBuffer.AppendLine("using stonefw.Utility;");
            codeBuffer.AppendLine("using stonefw.Web.Utility.BaseClass;");

            codeBuffer.AppendLine("");
            codeBuffer.AppendLine("namespace " + ns + ".web{");

            codeBuffer.Append("public partial class ").Append(n).AppendLine("Detail : BasePage");
            codeBuffer.AppendLine("{");
            codeBuffer.Append("private ").Append(n).AppendLine("Biz _biz;");
            codeBuffer.Append("private ")
                .Append(n)
                .Append("Biz Biz{get { return _biz ?? (_biz = new ")
                .Append(n)
                .AppendLine("Biz()); }");
            codeBuffer.AppendLine("}");
            codeBuffer.AppendLine("");
            codeBuffer.AppendLine("protected override bool InitPermission()");
            codeBuffer.AppendLine("{");
            codeBuffer.AppendLine(
                "return LoadPermission(SysPermsPointEnum.Add) || LoadPermission(SysPermsPointEnum.Edit);");
            codeBuffer.AppendLine("}");
            codeBuffer.AppendLine("");

            codeBuffer.AppendLine("protected void Page_Load(object sender, EventArgs e)");
            codeBuffer.AppendLine("{");
            codeBuffer.AppendLine("if (!IsPostBack)");
            codeBuffer.AppendLine("{");
            codeBuffer.AppendLine("FillFormData();");
            codeBuffer.AppendLine("}");
            codeBuffer.AppendLine("}");
            codeBuffer.AppendLine("protected void btnSave_Click(object sender, EventArgs e){");
            codeBuffer.AppendLine("try{");
            codeBuffer.Append("").Append(n).AppendLine("Entity entity = PrepareFormData();");

            //新增
            string strAddArg = primaryKeyColumns.Aggregate(string.Empty,
                (current, primaryKey) =>
                    current + string.Format("this.{0}.Value == \"-1\"&&", FirstCharLower("hd" + primaryKey)));
            strAddArg = strAddArg.Substring(0, strAddArg.Length - 2);
            codeBuffer.Append("if (").Append(strAddArg).AppendLine("){");
            codeBuffer.Append("Biz.AddNew").Append(n).AppendLine("(entity);}");
            codeBuffer.AppendLine("else");
            codeBuffer.Append("{Biz.Update").Append(n).AppendLine("(entity);");
            codeBuffer.AppendLine("}");
            codeBuffer.AppendLine("base.FatherQuery();");
            codeBuffer.AppendLine(
                "}catch (Exception ex){this.lMessage.Text = string.Format(\"保存失败，原因：{0}\", ex.Message);}");
            codeBuffer.AppendLine("}");
            codeBuffer.AppendLine("");
            codeBuffer.AppendLine("private void FillFormData(){");
            codeBuffer.AppendLine("try{");

            foreach (string column in primaryKeyColumns)
            {
                codeBuffer.AppendLine(string.Format("this.{0}.Value = Request[\"{1}\"];", FirstCharLower("hd" + column),
                    column.Replace("_", "").ToLower()));
            }

            string strQeruyArg = "";
            for (int i = 0; i < primaryKeyColumns.Length; i++)
            {
                var type =
                    GetObjectTypeOfDBColumnForCode(
                        columns.Where(m => m.ColumnName == primaryKeyColumns[i]).ToList()[0].ColumnType);
                if (type == "int")
                    strQeruyArg += string.Format("{0}.Parse(this.{1}.Value),", type,
                        FirstCharLower("hd" + primaryKeyColumns[i]));
                else
                    strQeruyArg += string.Format("this.{0}.Value,", FirstCharLower("hd" + primaryKeyColumns[i]));
            }
            strQeruyArg = strQeruyArg.Substring(0, strQeruyArg.Length - 1);
            codeBuffer.AppendLine(string.Format("{0}Entity entity = Biz.Get{0}Entity({1});", n, strQeruyArg));
            codeBuffer.AppendLine("if (entity != null){");

            for (int i = 0; i < primaryKeyColumns.Length; i++)
            {
                strQeruyArg += string.Format("this.{0}.Enabled = false;", FirstCharLower("txt" + primaryKeyColumns[i]));
            }

            foreach (SqlServerColumn column in columns)
            {
                var type = GetObjectTypeOfDBColumnForCode(column.ColumnType);
                codeBuffer.AppendLine(type == "bool"
                    ? string.Format("this.{0}.Checked = (bool)entity.{1};", FirstCharLower("cb" + column.ColumnName),
                        column.ColumnName)
                    : string.Format("this.{0}.Text = entity.{1}.ToString();", FirstCharLower("txt" + column.ColumnName),
                        column.ColumnName));
            }

            codeBuffer.AppendLine(
                "}}catch (Exception ex){this.lMessage.Text = string.Format(\"数据加载失败，原因：{0}\", ex.Message);}");
            codeBuffer.AppendLine("}");

            codeBuffer.Append("private ").Append(n).AppendLine("Entity PrepareFormData(){");
            codeBuffer.AppendLine("//TODO:需要校验参数的合法性");
            codeBuffer.Append("var entity = new ").Append(n).AppendLine("Entity();");

            foreach (SqlServerColumn column in columns)
            {
                var type = GetObjectTypeOfDBColumnForCode(column.ColumnType);
                switch (type)
                {
                    case "string":
                        codeBuffer.AppendLine(string.Format("entity.{0} = this.{1}.Text;", column.ColumnName,
                            FirstCharLower("txt" + column.ColumnName)));
                        break;
                    case "bool":
                        codeBuffer.AppendLine(string.Format("entity.{0} = this.{1}.Checked;", column.ColumnName,
                            FirstCharLower("cb" + column.ColumnName)));
                        break;
                    default:
                        codeBuffer.AppendLine(string.Format("entity.{0} = {1}.Parse(this.{2}.Text);", column.ColumnName,
                            type, FirstCharLower("txt" + column.ColumnName)));
                        break;
                }
            }
            codeBuffer.AppendLine("return entity;}}}");

            File.WriteAllText(fileName, codeBuffer.ToString());
        }

        private void CreateCodeOfDetailAspxDesignerCs(string fileName)
        {
            StringBuilder codeBuffer = new StringBuilder(100000);
            File.WriteAllText(fileName, codeBuffer.ToString());
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 创建目标目录
        /// </summary>
        private void CreateDirectory(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        /// <summary>
        /// 根据数据库字段类型获取默认的数据类型(用于生成代码)
        /// </summary>
        /// <param name="dbType">数据库字段类型</param>
        /// <returns>数据类型</returns>
        private string GetObjectTypeOfDBColumnForCode(string dbType)
        {
            string retType = "string";
            if (string.IsNullOrEmpty(dbType))
                return retType;

            dbType = dbType.ToLower();

            if (dbType.Contains("char"))
                return retType;

            if (dbType.Contains("text"))
                return retType;

            switch (dbType)
            {
                case "bigint":
                    retType = "long";
                    break;
                case "bit":
                    retType = "bool";
                    break;
                case "date":
                    retType = "DateTime";
                    break;
                case "datetime":
                    retType = "DateTime";
                    break;
                case "datetime2":
                    retType = "DateTime";
                    break;
                case "decimal":
                    retType = "decimal";
                    break;
                case "float":
                    retType = "double";
                    break;
                case "int":
                    retType = "int";
                    break;
                case "money":
                    retType = "decimal";
                    break;
                case "numeric":
                    retType = "decimal";
                    break;
                case "smalldatetime":
                    retType = "DateTime";
                    break;
                case "smallint":
                    retType = "short";
                    break;
                case "smallmoney":
                    retType = "decimal";
                    break;
                case "tinyint":
                    retType = "byte";
                    break;
                default:
                    break;
            }

            return retType;
        }

        /// <summary>
        /// 根据数据库字段类型获取默认的DBType类型
        /// </summary>
        /// <param name="rawDbType">数据库字段类型</param>
        /// <returns>数据类型</returns>
        private string GetDBTypeFromRawType(string rawDbType)
        {
            string retType = "DbType.AnsiString";
            if (string.IsNullOrEmpty(rawDbType))
                return retType;
            rawDbType = rawDbType.ToLower();

            if (rawDbType.Contains("char"))
                return retType;

            if (rawDbType.Contains("text"))
                return retType;

            switch (rawDbType)
            {
                case "bigint":
                    retType = "DbType.Int64";
                    break;
                case "bit":
                    retType = "DbType.Boolean";
                    break;
                case "date":
                    retType = "DbType.DateTime";
                    break;
                case "datetime":
                    retType = "DbType.DateTime";
                    break;
                case "datetime2":
                    retType = "DbType.DateTime";
                    break;
                case "decimal":
                    retType = "DbType.Decimal";
                    break;
                case "float":
                    retType = "DbType.Double";
                    break;
                case "int":
                    retType = "DbType.Int32";
                    break;
                case "money":
                    retType = "DbType.Decimal";
                    break;
                case "numeric":
                    retType = "DbType.Decimal";
                    break;
                case "smalldatetime":
                    retType = "DbType.DateTime";
                    break;
                case "smallint":
                    retType = "DbType.Int16";
                    break;
                case "smallmoney":
                    retType = "DbType.Decimal";
                    break;
                case "tinyint":
                    retType = "DbType.Byte";
                    break;
                default:
                    break;
            }

            return retType;
        }

        /// <summary>
        /// 显示命名空间列表
        /// </summary>
        private void ShowNameSpaceList()
        {
            this.cbNameSpaceList.Items.Clear();
            var nameSpaces = MyConfiguations.NameSpaceList;
            for (int i = 0; i < nameSpaces.Count; i++)
            {
                string loopKey = nameSpaces.Keys[i];
                string loopNameSpace = nameSpaces[loopKey];
                this.cbNameSpaceList.Items.Add(loopKey + ":" + loopNameSpace);
            }
            if (nameSpaces.Count > 0)
            {
                this.cbNameSpaceList.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 显示数据表
        /// </summary>
        private void ShowDataTables()
        {
            string[] tables = TableEnumerator.GetUserTables();
            tables = tables.OrderBy(n => n).ToArray();
            this.cbDataTableList.Items.Clear();
            this.cbDataTableList.Items.AddRange(tables);
        }

        private string FirstCharLower(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("_", "");
                var first = str.Substring(0, 1);
                var last = str.Substring(1, str.Length - 1);
                return first.ToLower() + last;
            }
            return string.Empty;
        }

        #endregion
    }
}